using System.Collections.Generic;
using System.Threading;

namespace Threading
{
    class SimplePCQueue_2_18<T> : IPCQueue<T>
    {
        readonly object _syncLock = new object();
        readonly object _syncLock2 = new object();
        readonly ManualResetEvent _gate = new ManualResetEvent(false);
        Queue<T> _prodConsQueue = new Queue<T>();

        public T ConsumeItem() //LOL ... CLASSIC DEADLOCK IF _syncLock == _syncLock2
        {
            lock(_syncLock)
            {
                while (_prodConsQueue.Count == 0)
                {
                    _gate.Reset();
                    _gate.WaitOne();
                }
                return _prodConsQueue.Dequeue();
            }            
        }

        public void ProduceItems(params T[] itemsToAdd)
        {
            lock (_syncLock2) //exclusively add items - single activity in class
            {
                foreach (var item in itemsToAdd)
                {
                    _prodConsQueue.Enqueue(item);
                }
                _gate.Set();
            }
        }
    }
}
