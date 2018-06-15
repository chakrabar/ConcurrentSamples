using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Threading
{
    class ThreadSafeCollections_16
    {
        internal static void ConcurrentBagDemo()
        {
            //instantiate with values from IEnumerable<T>
            var safeBag = new ConcurrentBag<string>(new string[] { "a", "b", "c" });
            bool isBagEmpty = safeBag.IsEmpty; //false
            int itemsInBag = safeBag.Count; //3
            safeBag.Add("another item"); //add an item to bag

            //this will get items in any order
            while (safeBag.TryTake(out string result)) //C# 7.0 construct
            {
                Console.WriteLine($"Got item {result}"); //will iterate 4 times
            }
            //now that bag is empty, TryTake will return FALSE
            Console.WriteLine($"More items in bag? {safeBag.TryTake(out string item)}");
        }

        internal static void ConcurrentStackDemo()
        {
            var safeStack = new ConcurrentStack<string>();
            //add strings "1" - "5" to stack
            foreach (var item in Enumerable.Range(1, 5))
            {
                safeStack.Push(item.ToString());
            }
            bool isStackEmpty = safeStack.IsEmpty; //false
            int itemsInStack = safeStack.Count; //5
            //add multiple items with PushRange()
            safeStack.PushRange(new string[] { "x", "y", "z" });

            var data = new string[2]; //try pop 2 items at a time, into data
            while (safeStack.TryPopRange(data) > 0) //no. of items popped
            {
                Console.WriteLine($"Got items {String.Join(',', data)}");
                data = new string[2]; //no old data in case of partial read
            } //this loop will iterate 4 times, 4 * 2 = 8
        }

        internal static void ConcurrentDictionaryDemo()
        {
            var safeDict = new ConcurrentDictionary<int, string>(5, 10);

            safeDict.AddOrUpdate(1, "a", (oldKey, oldValue) => oldValue + "a");

            var val = safeDict.GetOrAdd(2, "b");

            bool hasKey = safeDict.TryGetValue(3, out string value);

            bool isDictEmpty = safeDict.IsEmpty; //false
            int itemsInDick = safeDict.Count; //2
        }
    }
}
