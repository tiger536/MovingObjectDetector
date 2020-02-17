using ClassLibrary1.Implementations;
using Emgu.CV;
using System;

namespace ClassLibrary1
{
    public interface IMatReady
    {      
        event EventHandler<MatReadyCompletedArgs> MatReady;

        void Pause();
        void Start();
    }
}
