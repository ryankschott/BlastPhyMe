using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChangLab.Common
{
    public class Range
    {
        private bool _enforceLogicalValues;

        private int _start = 0;
        public int Start
        {
            get { return _start; }
            set
            {
                if (_enforceLogicalValues)
                {
                    if (_end != 0) // End has been set.
                    {
                        if (value >= End)
                        {
                            throw new ArgumentOutOfRangeException("Start", "Start must be less than End.");
                        }
                    }
                    if (value <= 0)
                    {
                        throw new ArgumentOutOfRangeException("Start", "Start must be greater than zero.");
                    }
                }

                _start = value;
            }
        }

        private int _end = 0;
        public int End
        {
            get { return _end; }
            set
            {
                if (_enforceLogicalValues)
                {
                    if (_start != 0) // Start has been set.
                    {
                        if (value <= Start)
                        {
                            throw new ArgumentOutOfRangeException("End", "End must be greater than Start.");
                        }
                    }
                    if (value <= 0)
                    {
                        throw new ArgumentOutOfRangeException("End", "End must be greater than zero.");
                    }
                }

                _end = value;
            }
        }

        public int Length
        {
            get
            {
                int length = 0;
                if (End >= Start)
                {
                    length = End - Start;
                }
                else
                {
                    length = Start - End;
                }

                if (length != 0)
                {
                    // The Start and End refer to indexes in the sequence, so the actual count of nucleotides is Start-to-End plus one.
                    return length + 1;
                }
                else
                {
                    // A Length of 0 is used as a test to see if the Range has been configured, so if we have a 0 we want to return a 0, not a 1.
                    return 0;
                }
            }
        }

        public Range()
        {
            _enforceLogicalValues = false;
        }

        public Range(bool EnforceLogicalValues)
        {
            _enforceLogicalValues = EnforceLogicalValues;
        }

        public Range(int Start, int End, bool EnforceLogicalValues = false)
        {
            this._start = Start;
            this._end = End;

            _enforceLogicalValues = EnforceLogicalValues;
            if (_enforceLogicalValues)
            {
                // This will trigger the validation.
                this.Start = Start;
                this.End = End;
            }
        }

        /// <summary>
        /// Returns a deep copy of the current instance.
        /// </summary>
        public Range Copy()
        {
            // Range as of yet has no reference types; if it gets any this function will need to be updated to accomodate that.
            return (Range)this.MemberwiseClone();
        }
    }

    public class RangeWithInterval
    {
        public double Start { get; set; }
        public double End { get; set; }
        public double Interval { get; set; }
        public bool Fixed { get; set; }

        public double[] Values
        {
            get { return new double[] { Start, End, Interval, (Fixed ? 1.00 : 0.00) }; }
            set { Start = value[0]; End = value[1]; Interval = value[2]; Fixed = (value[3] == 1.00); }
        }

        public RangeWithInterval() { }

        public RangeWithInterval(double Start, double End, double Interval, bool Fixed = false)
        {
            this.Start = Start;
            this.End = End;
            this.Interval = Interval;
            this.Fixed = Fixed;
        }

        public RangeWithInterval Copy()
        {
            return (RangeWithInterval)this.MemberwiseClone();
        }

        public override string ToString()
        {
            return this.Start.ToString() + "|" + this.End.ToString() + "|" + this.Interval.ToString() + "|" + this.Fixed.ToString();
        }

        public static RangeWithInterval FromString(string Value)
        {
            string[] pieces = Value.Split(new char[] { '|' });
            return new RangeWithInterval(double.Parse(pieces[0]), double.Parse(pieces[1]), double.Parse(pieces[2]), bool.Parse(pieces[3]));
        }
    }
}
