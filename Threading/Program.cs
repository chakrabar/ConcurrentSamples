using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Threading
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //SimpleThread_01.Execute();
            //ThreadLocking_02.Execute();
            //Monitor_03.Execute();
            //ReaderWriterLock_12.Execute();
            //ReaderWriterLockSlim_14.Execute();
            //Mutex_05.Execute();
            //Semaphore_06.Execute();
            //AutoResetEvent_10.Execute2();
            //ManualResetEvent_11.Execute();
            CountDownEvent_12.Execute();

            Console.WriteLine("Press ENTER to exit...");
            Console.ReadLine();
        }
    }
}
