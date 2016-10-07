namespace SuperGlue.Hosting.Katana
{
    public class KatanaSettings
    {
        private int _port;

        public KatanaSettings BindTo(int port)
        {
            _port = port;

            return this;
        }

        internal int GetPort()
        {
            return _port;
        }
    }
}