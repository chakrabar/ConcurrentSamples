using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentAndParallel
{
    class ParallelExamples
    {
        public void SlowTyper(int value)
        {
            Thread.Sleep(1000);
            Console.WriteLine($"Value: {value}");
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
                SlowTyper(i);
            }
        }

        public void ParallelFor()
        {
            var k = Parallel.For(1, 6,
                (idx) => SlowTyper(idx));
        }

        public void ParallelForEach()
        {
            Parallel.ForEach(Enumerable.Range(1, 5),
                (idx) => SlowTyper(idx));
        }

        public void ParallelInvoke()
        {
            Parallel.Invoke(() => SlowTyper(1), () => SlowTyper(2),
                () => SlowTyper(3), () => SlowTyper(4), () => SlowTyper(5));
        }

        public void PLinq()
        {
            Enumerable.Range(1, 5)
                .AsParallel()
                .ForAll(i => SlowTyper(i));
        }
    }
}
