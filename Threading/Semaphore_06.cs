using System.Threading;

namespace Threading
{
    class Semaphore_06
    {
        //class level semaphore with initialCount = 1, maxCount = 3
        static readonly SemaphoreSlim smps = new SemaphoreSlim(1, 3);

        internal static void Execute()
        {
            var t = new Thread(ReleaseSemaphore);
            t.Start();
            //current count = 1, as set with initialCount = 1
            smps.Wait(); //take the semaphore immediately, count = 0
            //next thread waits for other thread to releases it
            //ReleaseSemaphore releases semaphore, making count 1 = 1
            smps.Wait(2000); //try to take with 2 second timeout, count = 0          
            smps.Release(3); //count = 3
            smps.Wait(); //count = 2
            smps.Wait(); //count = 1
            smps.Release(3); //exception (count = 4 > maxCount 3)
        }

        static void ReleaseSemaphore()
        {
            Thread.Sleep(1000);
            smps.Release(); //count++
        }

        internal static void Execute2()
        {
            var smp = new Semaphore(1, 10, "AC's cool Semaphore");
            smp.WaitOne(1000); //with 1 second timeout
            smp.Release();
            smp.Dispose();            
        }
    }
}
