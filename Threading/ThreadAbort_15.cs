using System;
using System.Threading;

namespace Threading
{
    //========================================================================//
    //============= thread.Abort() is NOT supported in .NET Core =============//
    //========================================================================//
    class ThreadAbort_15
    {
        internal static void Execute()
        {
            Thread t = new Thread(Work);
            t.Start();
            Thread.Sleep(1000); t.Abort();
            Thread.Sleep(1000); t.Abort();
            Thread.Sleep(1000); t.Abort();
        }

        static void Work()
        {
            while (true)
            {
                try
                {
                    while (true) ;
                }
                catch (ThreadAbortException) { Thread.ResetAbort(); }
                Console.WriteLine("I will not die!");
            }
        }
    }
}
