using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using SuperGlue.Configuration;

namespace SuperGlue.Hosting.Katana
{
    public class SetupKatanaConfiguration : ISetupConfigurations
    {
        public IEnumerable<ConfigurationSetupResult> Setup(string applicationEnvironment)
        {
            yield return new ConfigurationSetupResult("superglue.KatanaSetup", environment =>
            {
                environment.AlterSettings<KatanaSettings>(x =>
                {
                    x.BindTo(GetRandomUnusedPort());
                });

                environment[WebHostExtensions.WebHostConstants.Bindings] = (Func<IEnumerable<string>>)(() =>
                {
                    var port = environment.GetSettings<KatanaSettings>().GetPort();

                    return new List<string>
                    {
                        $"http://localhost:{port}"
                    };
                });

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