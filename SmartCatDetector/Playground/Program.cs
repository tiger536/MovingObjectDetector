using ObjectDetection;
using ObjectDetection.Implementations;
using ObjectDetection.MatQueuer;
using ObjectDetection.Models;
using System;
using System.Windows.Forms;

namespace Playground
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            DependencyInjector.AddSingleton<IMatReady, VideoMat>();
            //DependencyInjector.AddSingleton<IMatReady, CameraMat>();
            DependencyInjector.AddSingleton<IDnnProvider, DnnProvider>();
            DependencyInjector.AddSingleton<IWorker, Worker>();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(DependencyInjector.CreateInstance<TestForm>());
        }
    }
}
