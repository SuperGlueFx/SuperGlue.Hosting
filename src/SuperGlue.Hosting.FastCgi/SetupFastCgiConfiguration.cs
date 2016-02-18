using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using SuperGlue.Configuration;

namespace SuperGlue.Hosting.FastCgi
{
    public class SetupFastCgiConfiguration : ISetupConfigurations
    {
        public IEnumerable<ConfigurationSetupResult> Setup(string applicationEnvironment)
        {
            yield return new ConfigurationSetupResult("superglue.FastCgiSetup", environment =>
            {
                environment.AlterSettings<FastCgiSettings>(x =>
                {
                    x.FallbackTo("127.0.0.1", GetRandomUnusedPort());
                });

                environment[WebHostExtensions.WebHostConstants.Bindings] = (Func<IEnumerable<string>>)(() => environment.GetSettings<FastCgiSettings>().GetBindings().Select(x => $"{x.Ip}:{x.Port}"));

                return Task.CompletedTask;
            });
        }

        private static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Any, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
    }
}