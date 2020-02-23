using ObjectDetection.Implementations;
using Emgu.CV;
using System;

namespace ObjectDetection
{
    public interface IWorker
    {      
        event EventHandler<WorkerResultsReadyArgs> DnnResultsReady;

        public void AddWork(Mat mat);
    }
}
