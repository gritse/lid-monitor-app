using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace LidMonitorApp
{
    public static class DisplaySwitch
    {
        public enum DisplayMode
        {
            Internal = 1,
            Clone,
            Extend,
            External
        }
        
        public static void Switch(DisplayMode displayMode)
        {
            // Create process start info
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "DisplaySwitch.exe",
                Arguments = $"{(int)displayMode}"
            };

            // Create process
            using (var process = new Process())
            {
                process.StartInfo = processStartInfo;
                process.Start();
            }
        }
    }
}