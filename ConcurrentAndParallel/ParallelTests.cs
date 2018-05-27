using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentAndParallel
{
    class ParallelTests
    {
        public void SlowTyper(int value, string name)
        {
            Thread.Sleep(1000);
            Console.WriteLine($"{name}: Value: {value}");
        }

        public void RunAll()
        {
            var sw = new Stopwatch();

            sw.Start();
            Sequential();
            sw.Stop();
            Console.WriteLine($"Time taken in Sequential = {sw.ElapsedMilliseconds}");

            sw.Restart();
            ParallelFor();
            sw.Stop();
            Console.WriteLine($"Time taken in ParallelFor = {sw.ElapsedMilliseconds}");

            sw.Restart();
            ParallelForEach();
            sw.Stop();
            Console.WriteLine($"Time taken in ParallelForEach = {sw.ElapsedMilliseconds}");

            sw.Restart();
            ParallelInvoke();
            sw.Stop();
            Console.WriteLine($"Time taken in ParallelInvoke = {sw.ElapsedMilliseconds}");

            sw.Restart();
            PLinq();
            sw.Stop();
            Console.WriteLine($"Time taken in PLinq = {sw.ElapsedMilliseconds}");
        }

        public void Sequential()
        {
            for (int i = 1; i <= 5; i++)
            {
                SlowTyper(i, nameof(Sequential));
            }
        }

        public void ParallelFor()
        {
            var k = Parallel.For(1, 6,
                (idx) => SlowTyper(idx, nameof(ParallelFor)));
        }

        public void ParallelForEach()
        {
            Parallel.ForEach(Enumerable.Range(1, 5), 
                (idx) => SlowTyper(idx, nameof(ParallelForEach)));
        }

        public void ParallelInvoke()
        {
            var n = nameof(ParallelInvoke);
            Parallel.Invoke(() => SlowTyper(1, n), () => SlowTyper(2, n), 
                () => SlowTyper(3, n), () => SlowTyper(4, n), () => SlowTyper(5, n));
        }

        public void PLinq()
        {
            Enumerable.Range(1, 5).AsParallel().ForAll(i => SlowTyper(i, "PLinq"));
        }
    }
}
