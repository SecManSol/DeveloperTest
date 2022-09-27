using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Application
{
    public class DisplayResult
    {
        public string _Started { get; set; } = string.Empty;
        public double _Processed { get; set; }
        public double _Elapsed { get; set; }
        public object _GuidId { get; set; } 

        public DisplayResult(string started, double processed, double elapsed, object guid)
        {
            this._Started = started;
            this._Processed = processed;
            this._Elapsed = elapsed;
            this._GuidId = guid;
        }
    }
}
