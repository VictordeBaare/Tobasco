using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tobasco.Manager
{
    public static class ProgressBarManager
    {
        private static string label;
        private static IVsStatusbar statusBar;
        private static uint cookie = 0;
        private static uint _nTotal;
        private static uint _nComplete;

        public static void Activate(IServiceProvider serviceProvider)
        {
            statusBar = (IVsStatusbar)serviceProvider.GetService(typeof(SVsStatusbar));
            Reset();
        }

        public static void Reset()
        {
            _nTotal = 0;
            _nComplete = 0;
            label = "Tobasco progress";
            statusBar.Progress(ref cookie, 1, label, _nComplete, _nTotal);
        }

        public static void SetTotal(uint nTotal, string subLabel)
        {
            label = $"{label} ==> {subLabel}";
            _nTotal = nTotal;
            statusBar.Progress(ref cookie, 1, label, _nComplete, _nTotal);
        }

        public static void SetProgress()
        {
            _nComplete = _nComplete + 1;
            statusBar.Progress(ref cookie, 1, label, _nComplete, _nTotal);
        }
    }
}
