using System;
using System.Threading;
using System.Threading.Tasks;

namespace Threading
{
    class WhatIsShared_17
    {
        public string InstanceLevel = "Instance was accesses by threads";
        public static string StaticLevel = "Static was accesses by threads";

        public string GetLocal()
        {
            var local = "Local was accesses by threads";
            local += " " + Thread.CurrentThread.ManagedThreadId;
            return local;
        }

        public string GetInstanceLevel()
        {
            InstanceLevel += " " + Thread.CurrentThread.ManagedThreadId;
            return InstanceLevel;
        }

        public string GetStaticLevel()
        {
            StaticLevel += " " + Thread.CurrentThread.ManagedThreadId;
            return StaticLevel;
        }

        public void Execute()
        {
            var test = new WhatIsShared_17();
            Task[] tasks = new Task[10];
            for (int i = 0; i < 10; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    Console.WriteLine(test.GetLocal());
                    Console.WriteLine(test.GetInstanceLevel());
                    Console.WriteLine(test.GetStaticLevel());
                });
            }
            Task.WaitAll(tasks);
        }

        public void ExecuteWithThreads()
        {
            var test = new WhatIsShared_17();
            Thread[] threads = new Thread[10];
            for (int i = 0; i < 10; i++)
            {
                threads[i] = new Thread(() => {
                    Console.WriteLine(test.GetLocal());
                    Console.WriteLine(test.GetInstanceLevel());
                    Console.WriteLine(test.GetStaticLevel());
                });
                threads[i].Start();
            }
            
            foreach (var t in threads)
            {
                t.Join();
            }
        }

        public void ExecuteWithOwnInstances()
        {
            Task[] tasks = new Task[10];
            for (int i = 0; i < 10; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    var test = new WhatIsShared_17();
                    Console.WriteLine(test.GetLocal());
                    Console.WriteLine(test.GetInstanceLevel());
                    Console.WriteLine(test.GetStaticLevel());
                });
            }
            Task.WaitAll(tasks);
        }
    }
}
