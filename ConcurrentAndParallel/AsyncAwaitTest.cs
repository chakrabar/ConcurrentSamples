using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentAndParallel
{
    class AsyncAwaitTest
    {
        public int SlowReturnigMethod()
        {
            Thread.Sleep(10000);
            return DateTime.Now.Millisecond;
        }

        //the async keyword tells compiler that this method can run asynchronous code. 
        //It doesn't make the code asynchronous automatically, but gives the capability to do CONTINUATION of tasks asynchronously.
        //Inside, either await SomeAsyncTask() to wait for it non-blockingly. Or do await Task.Run() to run some heavy work on background thread
        public async void AnotherSmartMethodAsync()
        {
            var result = await Task.Run(() => SlowReturnigMethod());
            //await waits for the task to complete, and then gets the result
            //If task being awaited throws exception, await doesn't throw the exception. The exception is attached to the resulting Task (like Task.Run()). Not the one who called it

            //if an async method throws exception, that is being awaited, that await statement throws exception. 
            //But SomeAsync() methods do not throw exception outside, rather it just attaches the exception to the returning Task.
        }
    }
}
