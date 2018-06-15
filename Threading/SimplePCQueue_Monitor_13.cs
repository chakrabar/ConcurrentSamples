using System.Collections.Generic;
using System.Threading;

namespace Threading
{
    class SimplePCQueue_Monitor_13<T> : IPCQueue<T>
    {
        readonly object _syncLock = new object();
        Queue<T> _prodConsQueue = new Queue<T>();
    
        public T ConsumeItem()
        {
            lock(_syncLock) //safely get one item
            {
                while(_prodConsQueue.Count == 0)
                {
                    //if no item, release lock but block the thread
                    //move it to the wait-queue and wait for signal
                    //when item is available, it'll get signal
                    Monitor.Wait(_syncLock); //it's NOT LOCKED at this point
                    //once signalled, it'll go and try to regain the lock
                    //all such threads will queue up in ready-queue
                    //because of the lock, only one thread will consume at a time
                    //whichever thread gets the lock...will continue from here
                }
                return _prodConsQueue.Dequeue();
            }
        }

        public void ProduceItems(params T[] itemsToAdd)
        {
            lock(_syncLock) //exclusively add items - single activity in class
            {
                foreach (var item in itemsToAdd)
                {
                    _prodConsQueue.Enqueue(item);
                }
                //once items are added, signal all threads that are waiting
                Monitor.PulseAll(_syncLock);
                //this will move all threads from wait-queue to ready-queue
            }
        }
    }
}
