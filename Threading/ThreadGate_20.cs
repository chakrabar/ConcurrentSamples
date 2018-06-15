using System.Threading;

namespace Threading
{
    class ThreadGate_20
    {
        readonly ManualResetEvent _gate = new ManualResetEvent(false);

        public void Open()
        {
            _gate.Set();
        }

        public void Close()
        {
            _gate.Reset();
        }

        public void CloseAndWaitToOpen()
        {
            _gate.Reset();
            _gate.WaitOne();
        }

        public void WaitToOpen()
        {
            _gate.WaitOne();
        }
    }
}
