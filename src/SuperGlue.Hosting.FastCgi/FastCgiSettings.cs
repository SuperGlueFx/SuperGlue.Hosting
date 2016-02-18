using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SuperGlue.Hosting.FastCgi
{
    public class FastCgiSettings
    {
        private readonly IList<Binding> _bindings = new List<Binding>();
        private readonly IList<Binding> _fallbackBindings = new List<Binding>();

        public FastCgiSettings BindTo(string ip, int port)
        {
            _bindings.Add(new Binding(ip, port));

            return this;
        }

        internal FastCgiSettings FallbackTo(string ip, int port)
        {
            _fallbackBindings.Add(new Binding(ip, port));

            return this;
        }

        internal IReadOnlyCollection<Binding> GetBindings()
        {
            return !_bindings.Any() ? new ReadOnlyCollection<Binding>(_fallbackBindings) : new ReadOnlyCollection<Binding>(_bindings);
        }

        public class Binding
        {
            public Binding(string ip, int port)
            {
                Ip = ip;
                Port = port;
            }

            public string Ip { get; private set; }
            public int Port { get; private set; } 
        }
    }
}