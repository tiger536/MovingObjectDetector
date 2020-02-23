using ObjectDetection.Implementations;
using Emgu.CV;
using System;

namespace ObjectDetection
{
    public interface IMatReady
    {      
        event EventHandler<MatReadyCompletedArgs> MatReady;

        void Pause();
        void Start();
    }
}
