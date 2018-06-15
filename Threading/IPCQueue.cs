namespace Threading
{
    interface IPCQueue<T>
    {
        T ConsumeItem();
        void ProduceItems(params T[] itemsToAdd);
    }
}