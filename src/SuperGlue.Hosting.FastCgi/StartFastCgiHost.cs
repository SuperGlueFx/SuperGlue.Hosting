using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Fos;
using SuperGlue.Configuration;
using Owin;

namespace SuperGlue.Hosting.FastCgi
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class StartFastCgiHost : IStartApplication
    {
        private FosSelfHost _webServer;

        public string Chain => "chains.Web";

        public Task Start(AppFunc chain, IDictionary<string, object> settings, string environment)
        {
            _webServer = new FosSelfHost(x => x.Use<RunAppFunc>(new RunAppFuncOptions(chain)));

            var fastCgiSettings = settings.GetSettings<FastCgiSettings>();

            var bindings = fastCgiSettings.GetBindings();

            foreach (var binding in bindings)
                _webServer.Bind(IPAddress.Parse(binding.Ip), binding.Port);

            _webServer.Start(false);

            return Task.CompletedTask;
        }

        public Task ShutDown(IDictionary<string, object> settings)
        {
            _webServer?.Dispose();

            return Task.CompletedTask;
        }

        public AppFunc GetDefaultChain(IBuildAppFunction buildApp, IDictionary<string, object> settings, string environment)
        {
            return null;
        }
    }
}