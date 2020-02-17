using ClassLibrary1;
using ClassLibrary1.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Playground
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DependencyInjector.AddSingleton<IMatReady, VideoMat>();
            //DependencyInjector.AddSingleton<IMatReady, CameraMat>();


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(DependencyInjector.CreateInstance<CameraCapture>());
        }
    }
}
