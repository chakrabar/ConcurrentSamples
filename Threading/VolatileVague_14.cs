using System.Threading;

namespace Threading
{
    class VolatileVague_14
    {
        //all threads/processors will always get latest value of this
        public volatile int _criticalCount1 = 1;

        private int _criticalCount2 = 0;
        public int UpdateAndRead(int value)
        {
            //write and make new value available to all threads/processors
            Thread.VolatileWrite(ref _criticalCount2, value);
            //read latest value, in case some thread/processor has updated it
            int updatedVal = Thread.VolatileRead(ref _criticalCount2);
            return updatedVal;
        }
    }
}
