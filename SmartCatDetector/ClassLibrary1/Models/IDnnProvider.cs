using ClassLibrary1.Implementations.Dnn;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1.Models
{
    public interface IDnnProvider
    {
        void SetNet();
        void SetInputMat(Mat myMat);
        DnnOutput Forward();
    }
}
