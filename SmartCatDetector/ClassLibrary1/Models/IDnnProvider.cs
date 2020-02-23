using ObjectDetection.Implementations.Dnn;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectDetection.Models
{
    public interface IDnnProvider
    {
        void SetInputMat(Mat myMat);
        DnnOutput Forward();
    }
}
