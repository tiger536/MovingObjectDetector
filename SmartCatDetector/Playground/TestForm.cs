using System;
using System.Drawing;
using System.Windows.Forms;
using ObjectDetection;
using ObjectDetection.Implementations;
using Emgu.CV;

namespace Playground
{
    public partial class TestForm : Form
    {
        private VideoCapture _capture = null;
        private IMatReady _matGenerator;

        private IWorker _worker;


        public TestForm(IMatReady matGenerator, IWorker iworker)
        {
            InitializeComponent();
            
            _matGenerator = matGenerator;
            _worker = iworker;
            _matGenerator.MatReady += MatReady;
            _worker.DnnResultsReady += DisplayResults;
        }


        private void DisplayResults(object sender, WorkerResultsReadyArgs arg)
        {
            CvInvoke.Resize(arg.NewestMat, arg.NewestMat, new Size(captureImageBox.Width, captureImageBox.Height));
            captureImageBox.Image = arg.NewestMat;
            if (arg.TestMat != null && !arg.TestMat.IsEmpty)
            {
                CvInvoke.Resize(arg.TestMat, arg.TestMat, new Size(captureImageBox.Width, captureImageBox.Height));
                out2.Image = arg.TestMat;
            }
            base.Invoke((Action)delegate { processingTimeBox.Text = arg.ProcessingTime.ToString(); });
        }

        private void MatReady(object sender, MatReadyCompletedArgs arg)
        {
            if (arg.Frame != null && !arg.Frame.IsEmpty)
            {
                _worker.AddWork(arg.Frame);
            }
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
