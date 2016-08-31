using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using SuperGlue.Configuration;

namespace SuperGlue.Hosting.Aurelia
{
    public class SetupAureliaConfiguration : ISetupConfigurations
    {
        public IEnumerable<ConfigurationSetupResult> Setup(string applicationEnvironment)
        {
            yield return new ConfigurationSetupResult("superglue.AureliaSetup", environment =>
            {
                var location = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "", "app");

                environment.AlterSettings<AureliaSettings>(x => x.UsingCommand("run")
                    .UsingFlag($"--env {applicationEnvironment}")
                    .AtLocation(location));

                return Task.CompletedTask;
            });
        }
    }
}