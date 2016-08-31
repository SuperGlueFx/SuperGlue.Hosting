using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SuperGlue.Hosting.Aurelia
{
    public class AureliaSettings
    {
        private readonly ICollection<string> _flags = new Collection<string>();

        public string Command { get; private set; }
        public string Location { get; private set; }
        public IReadOnlyCollection<string> Flags => new List<string>(_flags);

        public AureliaSettings AtLocation(string location)
        {
            Location = location;

            return this;
        }

        public AureliaSettings UsingCommand(string command)
        {
            Command = command;

            return this;
        }

        public AureliaSettings UsingFlag(string flag)
        {
            _flags.Add(flag);

            return this;
        }

        internal string GetFlags()
        {
            return string.Join(" ", Flags);
        }
    }
}