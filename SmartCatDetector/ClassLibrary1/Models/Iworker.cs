using ClassLibrary1.Implementations;
using Emgu.CV;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public interface IWorker
    {      
        event EventHandler<WorkerResultsReadyArgs> DnnResultsReady;

        public void AddWork(Mat mat);
    }
}
