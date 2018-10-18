using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChangLab.Jobs;

namespace ChangLab.Common
{
    public delegate void ProgressUpdateEventHandler(ProgressUpdateEventArgs e);
    public class ProgressUpdateEventArgs : EventArgs
    {
        public bool Setup { get; set; }
        public string ProgressMessage { get; set; }
        public string StatusMessage { get; set; }
        public object Source { get; set; }
        public bool Cancel { get; set; }

        public int _currentProgress;
        public int CurrentProgress { get { return _currentProgress; } set { _currentProgress = value; _currentChanged = true; } }
        public int _currentMax;
        public int CurrentMax { get { return _currentMax; } set { _currentMax = value; _currentChanged = true; } }
        private bool _currentChanged;
        public bool CurrentChanged { get { return _currentChanged; } }

        public int _totalProgress;
        public int TotalProgress { get { return _totalProgress; } set { _totalProgress = value; _totalChanged = true; } }
        public int _totalMax;
        public int TotalMax { get { return _totalMax; } set { _totalMax = value; _totalChanged = true; } }
        private bool _totalChanged;
        public bool TotalChanged { get { return _totalChanged; } }

        public ProgressUpdateEventArgs()
        {
            Setup = false;
            Cancel = false;
            _currentChanged = false;
            _totalChanged = false;
        }
    }

    public delegate void StatusUpdateEventHandler(Job sender, StatusUpdateEventArgs e);
    public class StatusUpdateEventArgs : EventArgs
    {
        public JobStatuses Status { get; set; }
        public object Source { get; set; }

        public StatusUpdateEventArgs(JobStatuses Status) : this(null, Status) { }

        public StatusUpdateEventArgs(object Source, JobStatuses Status)
        {
            this.Source = Source;
            this.Status = Status;
        }
    }
}
