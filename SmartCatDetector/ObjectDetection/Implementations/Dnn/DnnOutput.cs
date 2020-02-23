using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectDetection.Implementations.Dnn
{
    public class DnnOutput
    {
        public List<float> ConfidenceList = new List<float>();
        public List<string> ClassesList = new List<string>();
        public int bestIndex = -1;
    }
}
