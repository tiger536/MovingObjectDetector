using Emgu.CV;
using System;
using System.Collections.Generic;

namespace ClassLibrary1.Implementations
{
    public class WorkerResultsReadyArgs : EventArgs
    {
        public Mat NewestMat { get; set; }
        public Mat TestMat { get; set; }
        public string BestMatch { get; set; }
        public List<string> DectectionOut { get; set; }
    }
}