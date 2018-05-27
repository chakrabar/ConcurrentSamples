using System.Threading;

namespace Threading
{
    static class Deadlock_04
    {
        static readonly object locker1 = new object();
        static readonly object locker2 = new object();

        internal static void Execute()
        {
            
            new Thread(() => { //worker thread
                lock (locker1)
                {
                    Thread.Sleep(1000);
                    lock (locker2) { } // Deadlock
                }
            }).Start();

            lock (locker2) //main thread
            {
                Thread.Sleep(1000);
                lock (locker1) { } // Deadlock
            }
        }
    }
}
