using System;
using System.Threading;
using System.Threading.Tasks;

namespace Threading
{
    class CountDownEvent_12
    {
        static readonly CountdownEvent countdownEvent = new CountdownEvent(3);

        internal static void Execute()
        {
            Console.WriteLine($"Current count = {countdownEvent.CurrentCount}");
            Console.WriteLine($"Countdown is now set to {countdownEvent.IsSet}");

            var fork = Task.Run(() =>
            {
                //countdownEvent.Signal(5); //EXCEPTION
                countdownEvent.Wait();
                Console.WriteLine("me too");
                countdownEvent.Reset();
            });

            var tasks = new Task[4];
            for (int i = 0; i < 3; i++)
            {
                var unClosure = i;
                tasks[i] = Task.Run(() => DownCount(unClosure));
            }

            tasks[3] = fork;
            
            countdownEvent.Wait();
            Console.WriteLine($"Countdown is now set to {countdownEvent.IsSet}");
            Task.WaitAll(tasks);
            Console.WriteLine($"Countdown is now set to {countdownEvent.IsSet}");
        }

        private static void DownCount(int delaySecond)
        {
            Task.Delay(delaySecond * 2000)
                .ContinueWith((t) =>
                {
                    Console.WriteLine($"delay = {delaySecond}");
                    countdownEvent.Signal();
                    Console.WriteLine($"Count now = {countdownEvent.CurrentCount}");
                });
        }
    }
}
