using System;
using System.Threading;

namespace Threading
{
    class ReaderWriterLock_08
    {
        static ReaderWriterLock rwl = new ReaderWriterLock();
        // Define the shared resource protected by the ReaderWriterLock.
        static int resource = 0;

        const int numThreads = 26;
        static bool running = true;
        static Random rnd = new Random();

        // Statistics.
        static int readerTimeouts = 0;
        static int writerTimeouts = 0;
        static int reads = 0;
        static int writes = 0;

        public static void Execute()
        {
            Thread[] t = new Thread[20];
            for (int i = 0; i < 20; i++)
            {
                t[i] = new Thread(Driver);
                t[i].Name = new String(Convert.ToChar(i + 65), 1);
                t[i].Start();
            }
            for (int i = 0; i < 20; i++)
                t[i].Join();
        }

        static void Driver()
        {
            double action = rnd.NextDouble();
            Thread.Sleep(500);
            if (action < .8)
                ReadSharedResource(10);
            else
                WriteToSharedResource(100);
        }

        static void ReadSharedResource(int timeOutMilliSec)
        {
            try {
                rwl.AcquireReaderLock(timeOutMilliSec);
                try {
                    Console.WriteLine($"Thread[{Thread.CurrentThread.Name}] - Current resource value on read {resource}");
                }
                finally {
                    rwl.ReleaseReaderLock();
                }
            }
            catch (ApplicationException) {
                Console.WriteLine("Reader lock timed out");
            }
        }

        static void WriteToSharedResource(int timeOutMilliSec)
        {
            try {
                rwl.AcquireWriterLock(timeOutMilliSec);
                try {
                    resource = rnd.Next(500);
                    Console.WriteLine($"Thread[{Thread.CurrentThread.Name}] - Current resource value after write {resource}");
                }
                finally {
                    rwl.ReleaseWriterLock();
                }
            }
            catch (ApplicationException) {
                Console.WriteLine("Writer lock timed out");
            }
        }

        public static void Execute2()
        {
            // Start a series of threads to randomly read from and
            // write to the shared resource.
            Thread[] t = new Thread[numThreads];
            for (int i = 0; i < numThreads; i++)
            {
                t[i] = new Thread(new ThreadStart(ThreadProc));
                t[i].Name = new String(Convert.ToChar(i + 65), 1);
                t[i].Start();
                if (i > 10)
                    Thread.Sleep(300);
            }

            // Tell the threads to shut down and wait until they all finish.
            running = false;
            for (int i = 0; i < numThreads; i++)
                t[i].Join();

            // Display statistics.
            Console.WriteLine("\n{0} reads, {1} writes, {2} reader time-outs, {3} writer time-outs.",
                  reads, writes, readerTimeouts, writerTimeouts);
            Console.Write("Press ENTER to exit... ");
            Console.ReadLine();
        }

        static void ThreadProc()
        {
            // Randomly select a way for the thread to read and write from the shared
            // resource.
            while (running)
            {
                double action = rnd.NextDouble();
                if (action < .8)
                    ReadFromResource(10);
                else if (action < .81)
                    ReleaseRestore(50);
                else if (action < .90)
                    UpgradeDowngrade(100);
                else
                    WriteToResource(100);
            }
        }

        // Request and release a reader lock, and handle time-outs.
        static void ReadFromResource(int timeOut)
        {
            try
            {
                rwl.AcquireReaderLock(timeOut);
                try
                {
                    // It is safe for this thread to read from the shared resource.
                    Display("reads resource value " + resource);
                    Interlocked.Increment(ref reads);
                }
                finally
                {
                    // Ensure that the lock is released.
                    rwl.ReleaseReaderLock();
                }
            }
            catch (ApplicationException)
            {
                // The reader lock request timed out.
                Interlocked.Increment(ref readerTimeouts);
            }
        }

        // Request and release the writer lock, and handle time-outs.
        static void WriteToResource(int timeOut)
        {
            try
            {
                rwl.AcquireWriterLock(timeOut);
                try
                {
                    // It's safe for this thread to access from the shared resource.
                    resource = rnd.Next(500);
                    Display("writes resource value " + resource);
                    Interlocked.Increment(ref writes);
                }
                finally
                {
                    // Ensure that the lock is released.
                    rwl.ReleaseWriterLock();
                }
            }
            catch (ApplicationException)
            {
                // The writer lock request timed out.
                Interlocked.Increment(ref writerTimeouts);
            }
        }

        // Requests a reader lock, upgrades the reader lock to the writer
        // lock, and downgrades it to a reader lock again.
        static void UpgradeDowngrade(int timeOut)
        {
            try
            {
                rwl.AcquireReaderLock(timeOut);
                try
                {
                    // It's safe for this thread to read from the shared resource.
                    Display("reads resource value " + resource);
                    Interlocked.Increment(ref reads);

                    // To write to the resource, either release the reader lock and
                    // request the writer lock, or upgrade the reader lock. Upgrading
                    // the reader lock puts the thread in the write queue, behind any
                    // other threads that might be waiting for the writer lock.
                    try
                    {
                        LockCookie lc = rwl.UpgradeToWriterLock(timeOut);
                        try
                        {
                            // It's safe for this thread to read or write from the shared resource.
                            resource = rnd.Next(500);
                            Display("writes resource value " + resource);
                            Interlocked.Increment(ref writes);
                        }
                        finally
                        {
                            // Ensure that the lock is released.
                            rwl.DowngradeFromWriterLock(ref lc);
                        }
                    }
                    catch (ApplicationException)
                    {
                        // The upgrade request timed out.
                        Interlocked.Increment(ref writerTimeouts);
                    }

                    // If the lock was downgraded, it's still safe to read from the resource.
                    Display("reads resource value " + resource);
                    Interlocked.Increment(ref reads);
                }
                finally
                {
                    // Ensure that the lock is released.
                    rwl.ReleaseReaderLock();
                }
            }
            catch (ApplicationException)
            {
                // The reader lock request timed out.
                Interlocked.Increment(ref readerTimeouts);
            }
        }

        // Release all locks and later restores the lock state.
        // Uses sequence numbers to determine whether another thread has
        // obtained a writer lock since this thread last accessed the resource.
        static void ReleaseRestore(int timeOut)
        {
            int lastWriter;

            try
            {
                rwl.AcquireReaderLock(timeOut);
                try
                {
                    // It's safe for this thread to read from the shared resource,
                    // so read and cache the resource value.
                    int resourceValue = resource;     // Cache the resource value.
                    Display("reads resource value " + resourceValue);
                    Interlocked.Increment(ref reads);

                    // Save the current writer sequence number.
                    lastWriter = rwl.WriterSeqNum;

                    // Release the lock and save a cookie so the lock can be restored later.
                    LockCookie lc = rwl.ReleaseLock();

                    // Wait for a random interval and then restore the previous state of the lock.
                    Thread.Sleep(rnd.Next(250));
                    rwl.RestoreLock(ref lc);

                    // Check whether other threads obtained the writer lock in the interval.
                    // If not, then the cached value of the resource is still valid.
                    if (rwl.AnyWritersSince(lastWriter))
                    {
                        resourceValue = resource;
                        Interlocked.Increment(ref reads);
                        Display("resource has changed " + resourceValue);
                    }
                    else
                    {
                        Display("resource has not changed " + resourceValue);
                    }
                }
                finally
                {
                    // Ensure that the lock is released.
                    rwl.ReleaseReaderLock();
                }
            }
            catch (ApplicationException)
            {
                // The reader lock request timed out.
                Interlocked.Increment(ref readerTimeouts);
            }
        }

        // Helper method briefly displays the most recent thread action.
        static void Display(string msg)
        {
            Console.Write("Thread {0} {1}.       \r", Thread.CurrentThread.Name, msg);
        }
    }
}
