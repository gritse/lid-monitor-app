using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace LidMonitorApp
{
    public static class PowerCfg
    {
        public enum LidAction
        {
            Nothing = 0,
            Sleep = 1
        }
        
        public static void SetLidAction(LidAction action)
        {
            RunPowerCfg($"/setacvalueindex SCHEME_CURRENT SUB_BUTTONS LIDACTION {(int)action}");
            RunPowerCfg($"/setdcvalueindex SCHEME_CURRENT SUB_BUTTONS LIDACTION {(int)action}");
            RunPowerCfg("-setactive SCHEME_CURRENT");
        }
        
        public static LidAction GetLidAction()
        {
            var output = RunPowerCfg("/query SCHEME_CURRENT SUB_BUTTONS LIDACTION");
            return (LidAction)int.Parse(Regex.Match(output, @"(?<=0x)\d+").Value);
        }
        
        public static string RunPowerCfg(string parameters)
        {
            // Create process start info
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "powercfg.exe",
                Arguments = parameters,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,            
                StandardOutputEncoding = Encoding.GetEncoding(866)
            };

            // Create process
            using (var process = new Process())
            {
                process.StartInfo = processStartInfo;
                process.Start();

                // Capture output
                var output =  process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();

                // Wait for the process to exit
                process.WaitForExit();

                // Check for errors
                if (!string.IsNullOrEmpty(error))
                {
                    throw new Exception($"Error executing powercfg: {error}");
                }

                return output;
            }
        }
    }
}