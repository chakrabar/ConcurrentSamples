using System.Collections.Generic;

namespace Threading
{
    class SimplePCQueue_3_19<T> : IPCQueue<T>
    {
        readonly object _syncLock = new object();
        readonly object _syncLock2 = new object();
        Queue<T> _prodConsQueue = new Queue<T>();
        ThreadGate_20 _gate = new ThreadGate_20();

        public T ConsumeItem() //LOL ... CLASSIC DEADLOCK IF _syncLock == _syncLock2
        {
            lock (_syncLock)
            {
                while (_prodConsQueue.Count == 0)
                {
                    _gate.CloseAndWaitToOpen();
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
                _gate.Open();
            }
        }
    }
}
