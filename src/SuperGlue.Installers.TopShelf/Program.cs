using Topshelf;

namespace SuperGlue.Installers.TopShelf
{
    class Program
    {
        static void Main(string[] args)
        {
            var environment = "local";

            HostFactory.Run(x =>
            {
                x.AddCommandLineDefinition("appname", y =>
                {
                    x.SetServiceName(y);
                    x.SetDisplayName(y);
                });

                x.AddCommandLineDefinition("environment", y => environment = y);

                x.ApplyCommandLine(string.Join(" ", args));

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
