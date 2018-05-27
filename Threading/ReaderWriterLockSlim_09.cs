using System;
using System.Collections.Generic;
using System.Threading;

namespace Threading
{
    class ReaderWriterLockSlim_09
    {
        static ReaderWriterLockSlim rwLockSlim = new ReaderWriterLockSlim();
        static Random rnd = new Random();
        private Dictionary<int, string> innerCache = new Dictionary<int, string>();
        const int totalThreads = 26;
        static int resource = 0; //shared resource

        public static void Execute()
        {
            Thread[] t = new Thread[totalThreads];
            for (int i = 0; i < totalThreads; i++)
            {
                t[i] = new Thread(Driver);
                t[i].Name = new String(Convert.ToChar(i + 65), 1);
                t[i].Start();
            }
            for (int i = 0; i < totalThreads; i++)
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
            rwLockSlim.EnterReadLock();
            try
            {
                Console.WriteLine($"Thread[{Thread.CurrentThread.Name}] - Current resource value on read {resource}");
            }
            finally
            {
                rwLockSlim.ExitReadLock();
            }
        }

        static void WriteToSharedResource(int timeOutMilliSec)
        {
            if (rwLockSlim.TryEnterWriteLock(timeOutMilliSec))
            {
                try
                {
                    resource = rnd.Next(500);
                    Console.WriteLine($"Thread[{Thread.CurrentThread.Name}] - Current resource value after write {resource}");
                }
                finally
                {
                    rwLockSlim.ExitWriteLock();
                }
            }
            else
            {
                Console.WriteLine("Writer lock timed out");
            }
        }

        public int Count
        { get { return innerCache.Count; } }

        public string Read(int key)
        {
            rwLockSlim.EnterReadLock();
            try
            {
                return innerCache[key];
            }
            finally
            {
                rwLockSlim.ExitReadLock();
            }
        }

        public void Add(int key, string value)
        {
            rwLockSlim.EnterWriteLock();
            try
            {
                innerCache.Add(key, value);
            }
            finally
            {
                rwLockSlim.ExitWriteLock();
            }
        }

        public bool AddWithTimeout(int key, string value, int timeout)
        {
            if (rwLockSlim.TryEnterWriteLock(timeout))
            {
                try
                {
                    innerCache.Add(key, value);
                }
                finally
                {
                    rwLockSlim.ExitWriteLock();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public AddOrUpdateStatus AddOrUpdate(int key, string value)
        {
            rwLockSlim.EnterUpgradeableReadLock();
            try
            {
                string result = null;
                if (innerCache.TryGetValue(key, out result))
                {
                    if (result == value)
                    {
                        return AddOrUpdateStatus.Unchanged;
                    }
                    else
                    {
                        rwLockSlim.EnterWriteLock();
                        try
                        {
                            innerCache[key] = value;
                        }
                        finally
                        {
                            rwLockSlim.ExitWriteLock();
                        }
                        return AddOrUpdateStatus.Updated;
                    }
                }
                else
                {
                    rwLockSlim.EnterWriteLock();
                    try
                    {
                        innerCache.Add(key, value);
                    }
                    finally
                    {
                        rwLockSlim.ExitWriteLock();
                    }
                    return AddOrUpdateStatus.Added;
                }
            }
            finally
            {
                rwLockSlim.ExitUpgradeableReadLock();
            }
        }

        public void Delete(int key)
        {
            rwLockSlim.EnterWriteLock();
            try
            {
                innerCache.Remove(key);
            }
            finally
            {
                rwLockSlim.ExitWriteLock();
            }
        }

        public enum AddOrUpdateStatus
        {
            Added,
            Updated,
            Unchanged
        };

        ~ReaderWriterLockSlim_09()
        {
            if (rwLockSlim != null) rwLockSlim.Dispose();
        }
    }
}
