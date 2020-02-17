//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ClassLibrary1;
using ClassLibrary1.Implementations;
using ClassLibrary1.Models;
using Emgu.CV;
using Emgu.CV.BgSegm;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Playground
{
    public partial class CameraCapture : Form
    {
        private VideoCapture _capture = null;
        private IMatReady _matGenerator;
        private Mat first = null;
        private IBackgroundSubtractor _forgroundDetector  = new BackgroundSubtractorGMG(5,50);
        private MotionHistory _motionHistory = new MotionHistory(
                0.1, //in second, the duration of motion history you wants to keep
                0.05, //in second, maxDelta for cvCalcMotionGradient
                1); //in second, minDelta for cvCalcMotionGradient;

        public CameraCapture(IMatReady matGenerator)
        {
            InitializeComponent();

            _matGenerator = matGenerator;
            _matGenerator.MatReady += matReady;        
        }

        private void matReady(object sender, MatReadyCompletedArgs arg)
        {
            var cloned = arg.Frame.Clone();

            List<IMatOperation> filterList = new List<IMatOperation>()
            {
                new GrayScaleConverter(cloned),
                new GaussianBlur(cloned)
                //new CannyFilter(cloned)               
            };

            foreach(var filter in filterList)
            {
                filter.Execute();
            }
            if (first == null)
            {
                first = cloned;
                return;
            }
            Mat ahaha = new Mat();

            //_forgroundDetector.Apply(cloned, ahaha);
            //_motionHistory.Update(ahaha);

            CvInvoke.AbsDiff(first, cloned,ahaha);

            CvInvoke.Threshold(ahaha, ahaha, 25, 255, ThresholdType.Binary);
            first = cloned.Clone() ;
            CvInvoke.Resize(arg.Frame, arg.Frame, new Size(captureImageBox.Width, captureImageBox.Height));
            CvInvoke.Resize(cloned, cloned, new Size(captureImageBox.Width, captureImageBox.Height));
            CvInvoke.Resize(ahaha, ahaha, new Size(captureImageBox.Width, captureImageBox.Height));


            captureImageBox.Image = arg.Frame;
            outputImageBox.Image = cloned;
            out2.Image = ahaha;

            base.Invoke((Action)delegate { processingTimeBox.Text = arg.ProcessingTime.ToString(); });
        }

        private void captureButtonClick(object sender, EventArgs e)
        {
            _matGenerator.Start();
        }

        private void ReleaseData()
        {
            if (_capture != null)
                _capture.Dispose();
        }

        private void PauseCapureButtonClick(object sender, EventArgs e)
        {
            _matGenerator.Pause();
        }
    }
}
