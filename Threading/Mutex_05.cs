using System;
using System.Threading;

namespace Threading
{
    static class Mutex_05
    {
        static Mutex mutex = new Mutex();

        internal static void Execute()
        {
            //test
            var sw = new SpinWait(); //hola!
            WaitHandle.WaitAll(new WaitHandle[] { new Mutex(), new Mutex() });
            WaitHandle.WaitAny(new WaitHandle[] { new Mutex(), new Mutex() });
            
            //actual
            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread(AquireMutex);
                thread.Name = $"Thread{i + 1}";
                thread.Start();
            }
        }

        private static void AquireMutex(object obj)
        {
            mutex.WaitOne(); //get the mutex, wait until available
            DoSomething();
            mutex.ReleaseMutex();
            Console.WriteLine($"Mutex has been released by {Thread.CurrentThread.Name}");
        }

        private static void DoSomething()
        {
            Thread.Sleep(1500);
            Console.WriteLine($"Mutex has been aquired by {Thread.CurrentThread.Name}");
        }

        private static void AquireMutex2(object obj)
        {
            if (mutex.WaitOne(2000))
            {
                Console.WriteLine("Entered mutex");
                //do the work
                Console.WriteLine("Releasing mutex");
            }
            else
                Console.WriteLine("Timeout expired");
        }
    }
}
