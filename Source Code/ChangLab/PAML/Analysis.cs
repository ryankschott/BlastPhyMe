using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChangLab.Common;
using ChangLab.LocalDatabase;

namespace ChangLab.PAML.CodeML
{
    public class AnalysisConfiguration
    {
        #region Properties
        public int ID { get; internal set; }
        public int TreeID { get; set; }

        public int _model;
        public int Model
        {
            get { return _model; }
            set
            {
                switch (value)
                {
                    case 0:
                    case 2:
                    case 3:
                        _model = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Model", "Invalid model option.");
                }
            }
        }
        public int ModelPresetID { get; set; }

        public int NCatG { get; set; }
        public int Rank { get; set; }
        public ReferenceItem2<Jobs.JobStatuses> Status { get; set; }

        public double KStart { get; set; }
        public double KEnd { get; set; }
        private double _kInterval;
        public double KInterval { get { return _kInterval; } set { _kInterval = value; } }
        public static double KIntervalDefault = 1;
        public bool FixedKappa { get; set; }

        public double WStart { get; set; }
        public double WEnd { get; set; }
        private double _wInterval;
        public double WInterval { get { return _wInterval; } set { _wInterval = value; } }
        public static double WIntervalDefault = 1;
        public bool FixedOmega { get; set; }

        public static string KappaOmegaDescription(double Start, double End, double Interval, bool Fixed, string Format)
        {
            return
                    ((Interval == 0 || End == Start)
                        ? Start.ToString(Format)
                        : Start.ToString(Format)
                            + " - " + End.ToString(Format)
                            + " @ " + Interval.ToString(Format))
                    + (Fixed ? " (fixed)" : string.Empty);
        }

        public List<int> NSSites { get; internal set; }

        /// <summary>
        /// In-memory only, for tracking whether all of the analyses options have been run.
        /// </summary>
        internal List<CodeMLAnalysisOption> Analyses { get; set; }
        #endregion

        public AnalysisConfiguration()
        {
            this.ID = 0;
            this._kInterval = AnalysisConfiguration.KIntervalDefault;
            this.FixedKappa = false;
            this._wInterval = AnalysisConfiguration.WIntervalDefault;
            this.FixedOmega = false;
            this.Status = Jobs.JobStatusCollection.Get(Jobs.JobStatuses.New);
            this.NSSites = new List<int>();
        }

        /// <summary>
        /// Creates a list of CodeMLAnalysis objects by splitting the k/w values by their intervals.
        /// </summary>
        /// <returns></returns>
        public List<CodeMLAnalysisOption> GenerateAnalysisShells(Tree Tree)
        {
            List<CodeMLAnalysisOption> analyses = new List<CodeMLAnalysisOption>();

            // This logic deals with the issue of Interval not dividing cleanly into (End - Start)
            double kInterval = (KInterval != 0 ? KInterval : AnalysisConfiguration.KIntervalDefault);
            int kCount = Convert.ToInt32(Math.Ceiling((KEnd - KStart) / kInterval));
            List<double> kValues = Enumerable.Range(0, kCount).Select(k => KStart + (Convert.ToDouble(k) * kInterval)).ToList();
            if (kValues.Count == 0 || kValues.Last() < KEnd) { kValues.Add(KEnd); } // The Count() == 0 check covers when End == Start

            double wInterval = (WInterval != 0 ? WInterval : AnalysisConfiguration.WIntervalDefault);
            int wCount = Convert.ToInt32(Math.Ceiling((WEnd - WStart) / wInterval));
            List<double> wValues = Enumerable.Range(0, wCount).Select(w => WStart + (Convert.ToDouble(w) * wInterval)).ToList();
            if (wValues.Count == 0 || wValues.Last() < WEnd) { wValues.Add(WEnd); } // The Count() == 0 check covers when End == Start

            kValues.ForEach(k =>
                {
                    wValues.ForEach(w =>
                        {
                            analyses.Add(new CodeMLAnalysisOption()
                            {
                                Kappa = k,
                                Omega = w,
                                Configuration = this
                            });
                        });
                });

            return analyses;
        }

        public AnalysisConfiguration Copy()
        {
            return Copy(this);
        }

        public static AnalysisConfiguration Copy(AnalysisConfiguration Source)
        {
            AnalysisConfiguration config = new AnalysisConfiguration()
                {
                    TreeID = Source.TreeID,
                    Model = Source.Model,
                    ModelPresetID = Source.ModelPresetID,
                    NCatG = Source.NCatG,
                    Rank = Source.Rank,
                    Status = Jobs.JobStatusCollection.Get(Source.Status.Key),
                    KStart = Source.KStart,
                    KEnd = Source.KEnd,
                    KInterval = Source.KInterval,
                    FixedKappa = Source.FixedKappa,
                    WStart = Source.WStart,
                    WEnd = Source.WEnd,
                    WInterval = Source.WInterval,
                    FixedOmega = Source.FixedOmega
                };
            config.NSSites.AddRange(Source.NSSites);
            return config;
        }

        #region Database
        public void Save()
        {
            using (DataAccess da = new DataAccess("PAML.AnalysisConfiguration_Edit"))
            {
                da.AddParameter("TreeID", this.TreeID);
                da.AddParameter("Model", this.Model);
                da.AddParameter("ModelPresetID", this.ModelPresetID);
                da.AddParameter("NCatG", this.NCatG);
                da.AddDoubleParameter("KStart", 9, 3, this.KStart);
                da.AddDoubleParameter("KEnd", 9, 3, this.KEnd, true);
                da.AddDoubleParameter("KInterval", 9, 3, (this.KStart != this.KEnd ? this.KInterval : 0), true);
                da.AddParameter("KFixed", System.Data.SqlDbType.Bit, this.FixedKappa);
                da.AddDoubleParameter("WStart", 9, 3, this.WStart);
                da.AddDoubleParameter("WEnd", 9, 3, this.WEnd, true);
                da.AddDoubleParameter("WInterval", 9, 3, (this.WStart != this.WEnd ? this.WInterval : 0), true);
                da.AddParameter("WFixed", System.Data.SqlDbType.Bit, this.FixedOmega);
                da.AddParameter("Rank", this.Rank);
                da.AddParameter("StatusID", this.Status.ID);
                da.AddListParameter("NSSites", this.NSSites);
                da.AddOutputParameter("ID", this.ID);

                this.ID = (int)da.ExecuteParameter("ID");
            }
        }

        public void UpdateStatus(Jobs.JobStatuses Status)
        {
            using (DataAccess da = new DataAccess("PAML.AnalysisConfiguration_UpdateStatus"))
            {
                da.AddParameter("ID", this.ID);
                da.AddParameter("StatusID", Jobs.JobStatusCollection.Get(Status).ID);
                da.ExecuteCommand();

                this.Status = Jobs.JobStatusCollection.Get(Status);
            }
        }
        #endregion
    }
}
