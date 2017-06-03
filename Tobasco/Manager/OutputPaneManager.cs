using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace Tobasco.Manager
{
    public static class OutputPaneManager
    {
        private static Guid _tobascoOutputWindow = Guid.Parse("97c98fd2-7789-4af4-afaf-28e3cd2f0340");

        private static IVsOutputWindowPane _pane;

        public static void Activate(IServiceProvider provider)
        {
            IVsOutputWindow output = (IVsOutputWindow)provider.GetService(typeof(SVsOutputWindow));
            CreatePane(output);
        }

        public static void WriteToOutputPane(string message)
        {
            _pane.OutputStringThreadSafe($"{DateTime.Now}: {message} \n");
        }

        private static void CreatePane(IVsOutputWindow output)
        {
            // Retrieve the Tobasco pane.  
            output.GetPane(ref _tobascoOutputWindow, out _pane);

            if (_pane == null)
            {
                // Create a new pane.  
                output.CreatePane(ref _tobascoOutputWindow, "Tobasco", 1, 1);
                output.GetPane(ref _tobascoOutputWindow, out _pane);
            }

            _pane.Activate();

            _pane.Clear();
        }

    }
}
