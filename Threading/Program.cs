using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            //Interlocked_07.Execute();
            //AutoResetEvent_10.Execute2();
            //ManualResetEvent_11.Execute();
            //CountDownEvent_12.Execute();
            //ThreadAbort_15.Execute(); //not supported

            //ThreadSafeCollections_16.ConcurrentStackDemo();
            new WhatIsShared_17().Execute();

            //ProducerConsumerExample();

            Console.WriteLine("Press ENTER to exit...");
            Console.ReadLine();
        }

        private static void ProducerConsumerExample()
        {
            IPCQueue<string> pcQueue = new SimplePCQueue_3_19<string>(); //SimplePCQueue_Monitor_13<string>();
            Task[] tasks = new Task[10];
            var items = Enumerable.Range(1, 12).Select(i => i.ToString());

            for (int i = 0; i < 10; i++)
            {
                //Thread.Sleep(100);
                tasks[i] = Task.Run(() =>
                {
                    var item = pcQueue.ConsumeItem();
                    Console.WriteLine($"Consumed item = {item}");
                });
            }

            //add items in set of 3
            for (int i = 0; i < 4; i++)
            {
                Thread.Sleep(2000);
                pcQueue.ProduceItems(items.Skip(i * 3).Take(3).ToArray());
            }

            Task.WaitAll(tasks);
        }
    }
}
