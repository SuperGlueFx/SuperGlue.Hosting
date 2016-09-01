using System.Collections.Generic;
using SuperGlue.Configuration;

namespace SuperGlue.Installers.TopShelf
{
    public class SuperGlueApplication
    {
        private readonly string _environment;
        private readonly SuperGlueBootstrapper _bootstrapper;

        public SuperGlueApplication(string environment)
        {
            _environment = environment;

            _bootstrapper = SuperGlueBootstrapper.Find();
        }

        public void Start()
        {
            //TODO:Handle host arguments
            _bootstrapper.StartApplications(new Dictionary<string, object>(), _environment, new Dictionary<string, string[]>()).Wait();
        }

        public void Stop()
        {
            _bootstrapper?.ShutDown().Wait();
        }
    }
}