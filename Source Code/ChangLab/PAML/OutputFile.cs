using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ChangLab.Common;

namespace ChangLab.PAML.CodeML
{
    public class OutputFile
    {
        private string FilePath { get; set; }
        public Version PAMLVersion { get; private set; }
        private int Model { get; set; }
        private int NSSite { get; set; }

        public List<Result> Results { get; private set; }
        public List<Exception> Exceptions { get; private set; }

        public OutputFile(string FilePath, int Model, int NSSite = -1)
        {
            this.FilePath = FilePath;
            this.Model = Model;
            this.NSSite = NSSite;

            this.Results = new List<Result>();
            this.Exceptions = new List<Exception>();
        }

        public List<Result> Parse()
        {
            try
            {
                this.PAMLVersion = new Version(0, 0);

                // First we seek and extract the version number to ensure this is a version of codeml.exe that we can handle.
                // Yes, that means parsing the file twice, but it's just a text stream and the files don't get that big so it should be relatively fast.
                using (StreamReader reader = new StreamReader(this.FilePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();

                        if (line.StartsWith("CODONML") && line.Contains("version ")) // CODONML (in paml version 4.8, March 2014)...
                        {
                            Version version = null;
                            Match versionMatch = Regex.Match(line.Substring(line.IndexOf("version ") + 8), "^([0-9]{1,}\\.[0-9]{1,})(\\.[0-9]{1,}){0,2}");
                            if (versionMatch.Success && Version.TryParse(versionMatch.Value, out version))
                            {
                                this.PAMLVersion = version;
                            }

                            break;
                        }
                    }
                }

                if (this.PAMLVersion.Major == 0)
                {
                    // Raise this as a warning.
                    this.Exceptions.Add(new WarningException("Unable to parse PAML version number. Results may not be correctly parsed from the output file."));
                }
                // Similarly to the following comments, this is all just hard-coded for now.
                else if (!(new Version[] { new Version(4, 8), new Version(4, 9) }).Any(v => v.ToString() == this.PAMLVersion.ToString()))
                {
                    this.Exceptions.Add(new WarningException("PAML version in use is not explicitly supported by " + ApplicationProperty.GetEntryAssemblyProductName() + ". Results may not be correctly parsed from the output file."));
                }

                // The if... is commented out because right now the only version being supported is 4.8, and the most we're doing is warning the 
                // user that their files might not be parsed correctly if they're running a different version.  All of this is really just a
                // place-holder for some kind of actual logic to route output files into classes or methods that are version-specific.
                //if (this.PAMLVersion.Major == 4 && this.PAMLVersion.Minor == 8)
                //{
                Parse_4_8();
                //}
            }
            catch (Exception ex)
            {
                this.Exceptions.Add(ex);
            }

            return this.Results;
        }

        public void Parse_4_8()
        {
            try
            {
                using (StreamReader reader = new StreamReader(this.FilePath))
                {
                    Result result = null;
                    int nsSite = this.NSSite;

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(line)) { continue; }

                        if (line.StartsWith("Model "))
                        {
                            // This regex will ensure the line starts with "Model n:" where n is an integer of any length
                            if (Regex.IsMatch(line, "^Model {1}[0-9]+:"))
                            {
                                // If multiple NSsites were specified in the control file, the first line relevant to a result set will start with Model.
                                // If only one NSsites value was specified, there will be no Model line.
                                nsSite = int.Parse(line.Mid(line.IndexOf(" ") + 1, line.IndexOf(":")));
                            }
                        }
                        if (line.StartsWith("lnL("))
                        {
                            line = line.Trim(); // Just to make sure that we don't have any trailing spaces that would mess up the lnL value extraction
                            result = new Result()
                            {
                                NSSite = nsSite,
                                np = int.Parse(line.Mid(line.IndexOf("np:") + 3, line.IndexOf(")")).Trim()),
                                lnL = double.Parse(line.Mid(line.LastIndexOf(":") + 1, line.LastIndexOf(" ")).Trim())
                            };
                            this.Results.Add(result);
                        }
                        else if (line.StartsWith("kappa (ts/tv)"))
                        {
                            line = line.Trim();
                            result.k = double.Parse(line.Substring(line.LastIndexOf(" ") + 1));
                        }
                        else if (line.StartsWith("Time used:"))
                        {
                            line = line.Replace("Time used:", "").Trim();
                            if (line.Split(new char[] { ':' }).Length == 2)
                            {
                                // PAML outputs a timestamp less than one hour as mm:ss, which TimeStamp.Parse will assume is hh:mm.
                                line = "00:" + line;
                            }

                            TimeSpan duration = TimeSpan.MinValue;
                            if (TimeSpan.TryParse(line, out duration))
                            {
                                // This is in a TryParse on the off chance we encounter a timestamp that isn't in a parsable format.
                                // Timespans of hours, minutes, seconds all work, but I don't know what PAML outputs if a process runs for longer than
                                // 24 hours, and it would be a pain if the whole party crashed just because of a bad timestamp format.
                                // Also, one of the sample CmC files had no Time Used: line, though that might have been an anomaly.
                                result.Duration = duration;
                            }
                        }

                        if (result != null)
                        {
                            switch (this.Model)
                            {
                                case 0:
                                    // NSSite 0, 1, 2, 3, 7, 8 - 2a and 8a have the same output format as 2 and 8 respectively
                                    switch (result.NSSite)
                                    {
                                        case 0:
                                            if (line.StartsWith("omega (dN/dS)"))
                                            {
                                                line = line.Trim();
                                                result.Values.Add(new ResultdNdSValue()
                                                {
                                                    ValueType = ResultdNdSValueTypeCollection.Get(ResultdNdSValueTypes.w_value),
                                                    Value = double.Parse(line.Substring(line.LastIndexOf(" ") + 1))
                                                });
                                            }
                                            break;
                                        case 1:
                                        case 2:
                                        case 22: // 2a_rel
                                        case 3:
                                            if (line.StartsWith("dN/dS (w) for site classes"))
                                            {
                                                line = reader.ReadLine(); // Now we're on an empty line
                                                result.Values.AddRange(ParseSiteClassValues(reader.ReadLine(), ResultdNdSValueTypes.p_value));
                                                result.Values.AddRange(ParseSiteClassValues(reader.ReadLine(), ResultdNdSValueTypes.w_value));
                                            }
                                            break;
                                        case 7:
                                            if (line.StartsWith("Parameters in M7 (beta)"))
                                            {
                                                line = reader.ReadLine(); // Now we're on the value line
                                                string[] values = line.Replace("=", "").Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                                result.Values.Add(new ResultdNdSValue(ResultdNdSValueTypes.p_value, double.Parse(values[1])));
                                                result.Values.Add(new ResultdNdSValue(ResultdNdSValueTypes.q_value, double.Parse(values[3])));
                                            }
                                            break;
                                        case 8:
                                            if (line.StartsWith("Parameters in M8"))
                                            {
                                                line = reader.ReadLine(); // Now we're on the p0, p, q line
                                                string[] values = line.Replace("=", "").Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                                result.Values.Add(new ResultdNdSValue(ResultdNdSValueTypes.p_value, double.Parse(values[3])));
                                                result.Values.Add(new ResultdNdSValue(ResultdNdSValueTypes.q_value, double.Parse(values[5])));

                                                line = reader.ReadLine(); // Now we're on the p1, w line
                                                values = Regex.Replace(line, "[\\(\\)\\=]", "")
                                                                                                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                                result.Values.Add(new ResultdNdSValue(ResultdNdSValueTypes.p1_value, double.Parse(values[1])));
                                                result.Values.Add(new ResultdNdSValue(ResultdNdSValueTypes.w_value, double.Parse(values[3])));
                                            }
                                            break;
                                    }
                                    break;

                                case 2:
                                    if (line.StartsWith("w (dN/dS)") && result.NSSite == 0)
                                    {
                                        // Branch
                                        line = line.Substring(line.IndexOf(":") + 1).Trim();

                                        result.Values.Add(new ResultdNdSValue(ResultdNdSValueTypes.background_w, double.Parse(line.SplitByEmptySpace()[0])));
                                        result.Values.Add(new ResultdNdSValue(ResultdNdSValueTypes.foreground_w, double.Parse(line.SplitByEmptySpace()[1])));
                                    }
                                    else if (line.StartsWith("dN/dS (w)") && result.NSSite == 2)
                                    {
                                        // Branch-Site
                                        line = reader.ReadLine(); // Now we're on an empty line
                                        line = reader.ReadLine(); // Now we're on the header line

                                        string[] headers = line.SplitByEmptySpace().Skip(2).ToArray();
                                        result.Values.AddRange(ParseSiteClassValues(reader.ReadLine(), headers, ResultdNdSValueTypes.p_value));
                                        result.Values.AddRange(ParseSiteClassValues(reader.ReadLine(), headers, ResultdNdSValueTypes.background_w));
                                        result.Values.AddRange(ParseSiteClassValues(reader.ReadLine(), headers, ResultdNdSValueTypes.foreground_w));
                                    }
                                    break;

                                case 3:
                                    // Clade model C
                                    if (line.StartsWith("dN/dS (w) for site classes"))
                                    {
                                        line = reader.ReadLine(); // Now we're on an empty line
                                        line = reader.ReadLine(); // Now we're on the header line

                                        string[] headers = line.SplitByEmptySpace().Skip(2).ToArray();
                                        result.Values.AddRange(ParseSiteClassValues(reader.ReadLine(), headers, ResultdNdSValueTypes.p_value));

                                        line = reader.ReadLine();
                                        while (line.StartsWith("branch type"))
                                        {
                                            result.Values.AddRange(ParseSiteClassValues(line, headers, ResultdNdSValueTypes.branch_type));
                                            line = reader.ReadLine();
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Exceptions.Add(ex);
            }
        }

        private List<ResultdNdSValue> ParseSiteClassValues(string Line, ResultdNdSValueTypes ValueType)
        {
            return ParseSiteClassValues(Line, null, ValueType);
        }

        private List<ResultdNdSValue> ParseSiteClassValues(string Line, string[] Headers, ResultdNdSValueTypes ValueType)
        {
            string[] pieces = Line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            IEnumerable<string> values = null;

            ReferenceItem2<ResultdNdSValueTypes> valueType = ResultdNdSValueTypeCollection.Get(ValueType);
            int rank = 0;

            switch (ValueType)
            {
                case ResultdNdSValueTypes.background_w:
                case ResultdNdSValueTypes.foreground_w:
                    values = pieces.Skip(2);
                    break;
                case ResultdNdSValueTypes.branch_type:
                    rank = int.Parse(pieces[2].Replace(":", ""));
                    values = pieces.Skip(3);
                    break;
                default:
                    values = pieces.Skip(1);
                    break;
            }

            return values
                .Select((s, i) => new ResultdNdSValue()
                {
                    SiteClass = (Headers == null ? i.ToString() : Headers[i]),
                    ValueType = valueType,
                    Value = double.Parse(s),
                    Rank = rank
                })
                .ToList();
        }
    }
}
