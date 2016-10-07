using System.Configuration;
using Topshelf;

namespace SuperGlue.Installers.TopShelf
{
    class Program
    {
        static void Main(string[] args)
        {
            var environment = ConfigurationManager.AppSettings["App.Environment"];

            HostFactory.Run(x =>
            {
                x.Service<SuperGlueApplication>(s =>
                {
                    s.ConstructUsing(name => new SuperGlueApplication(environment));
                    s.WhenStarted(r => r.Start());
                    s.WhenStopped(r => r.Stop());
                    s.WhenPaused(r => r.Stop());
                    s.WhenContinued(r => r.Start());
                    s.WhenShutdown(r => r.Stop());
                });
            });
        }
    }
}
