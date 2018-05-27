using System.Threading;

namespace Threading
{
    class Interlocked_07
    {
        static void Execute()
        {
            //update a value atomically
            int sharedVar = 1;
            int original = Interlocked.Exchange(ref sharedVar, 2);
            //add 2 numbers atomically
            int a = 5, b = 10; //a gets the sum value
            int sum = Interlocked.Add(ref a, b);
        }
    }
}
