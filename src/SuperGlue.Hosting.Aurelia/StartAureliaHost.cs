using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Threading.Tasks;
using SuperGlue.Configuration;

namespace SuperGlue.Hosting.Aurelia
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class StartAureliaHost : IStartApplication
    {
        private Process _process;
        private bool _shouldBeStarted;

        public Task Start(AppFunc chain, IDictionary<string, object> settings, string environment, string[] arguments)
        {
            var location = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "", "app");

            var startInfo = new ProcessStartInfo("cmd.exe")
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                WorkingDirectory = location,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };

            _process = new Process
            {
                StartInfo = startInfo,
                EnableRaisingEvents = true
            };

            _process.OutputDataReceived += (x, y) => Console.WriteLine(y.Data);

            _process.Exited += async (x, y) => await StartProcess(arguments).ConfigureAwait(false);

            _shouldBeStarted = true;

            return StartProcess(arguments);
        }

        public Task ShutDown(IDictionary<string, object> settings)
        {
            _shouldBeStarted = false;

            if (_process != null)
                KillProcessAndChildren(_process.Id);

            return Task.CompletedTask;
        }

        public AppFunc GetDefaultChain(IBuildAppFunction buildApp, IDictionary<string, object> settings, string environment)
        {
            return x => Task.CompletedTask;
        }

        public string Name => "aurelia";

        public string Chain => "chains.Aurelia";

        private async Task StartProcess(string[] arguments)
        {
            if (!_shouldBeStarted || !_process.Start())
                return;

            await _process.StandardInput.WriteAsync($"au {string.Join(" ", arguments)}").ConfigureAwait(false);

            _process.BeginOutputReadLine();
        }

        private static void KillProcessAndChildren(int pid)
        {
            var searcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + pid);
            var moc = searcher.Get();

            foreach (var mo in moc.Cast<ManagementObject>())
                KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));

            try
            {
                var proc = Process.GetProcessById(pid);
                proc.Kill();
            }
            catch (ArgumentException)
            { /* process already exited */ }
        }
    }
}