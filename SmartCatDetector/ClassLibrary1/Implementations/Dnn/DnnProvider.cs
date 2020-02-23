using ObjectDetection.Implementations.Dnn;
using ObjectDetection.Models;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ObjectDetection.Implementations
{
    public class DnnProvider : IDnnProvider
    {
        private readonly Net net;
        private readonly List<string> ListaClassi;

        public DnnProvider()
        {
            ListaClassi = File.ReadAllLines(ConfigurationManager.AppSettings["DnnClassesPath"]).ToList<string>();
            net = DnnInvoke.ReadNetFromDarknet(ConfigurationManager.AppSettings["DnnConfigPath"], ConfigurationManager.AppSettings["DnnWeightsPath"]);
            if (net.Empty)
            {
                throw new Exception("Can't load network!");
            }
        }

        public void SetInputMat( Mat myMat)
        {
            CvInvoke.Rotate(myMat, myMat, RotateFlags.Rotate90Clockwise);
            Mat blob = DnnInvoke.BlobFromImage(myMat, 0.00392, new Size(416, 416), new MCvScalar(0, 0, 0), true, false);
            if(blob == null | blob.IsEmpty)
            {
                throw new Exception("Blob is null or empty!");
            }

            net.SetInput(blob);
            myMat.Dispose();
        }
        public DnnOutput Forward()
        {
            var dnnOutput = new DnnOutput();
            var tensors = new VectorOfMat();

            net.Forward(tensors, GetOutputLayers(net));

            using (Mat boxes = tensors[0])
            using (Mat masks = tensors[1])
            {
                float[,] boxesData = boxes.GetData(true) as float[,];
                for (int i = 0; i < boxes.Rows; i++)
                {
                    var boxData = GetRow(boxesData, i);
                    var confidence = boxData.Skip(5).Max();

                    if (confidence > 0.4)
                    {
                        dnnOutput.ConfidenceList.Add(confidence);
                        dnnOutput.ClassesList.Add(ListaClassi[boxData.ToList().IndexOf(confidence) - 5]);
                    }
                }
            }

            float max = 0;
            for(int i=0; i< dnnOutput.ConfidenceList.Count;i++)
            {
                if (dnnOutput.ConfidenceList[i] > max)
                {
                    max = dnnOutput.ConfidenceList[i];
                    dnnOutput.bestIndex = i;
                }
            }

            return dnnOutput;
        }


        private string[] GetOutputLayers(Net net)
        {
            var lista = new List<string>();
            foreach (int layerID in net.UnconnectedOutLayers)
            {
                lista.Add(net.LayerNames[layerID - 1]);
            }
            return lista.ToArray();
        }

        private float[] GetRow(float[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                    .Select(x => matrix[rowNumber, x])
                    .ToArray();
        }
    }
}
