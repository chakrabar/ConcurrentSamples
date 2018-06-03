using System.Threading.Tasks;

namespace AsyncAwait
{
    class MultipleAwait_06
    {
        public static async Task<string> GetPerson1Async()
        {
            int id = await GetIdAsync();
            string name = await GetNameAsync();
            return $"Person Id:{id}, Name:{name}";
        }

        public static async Task<string> GetPerson2Async()
        {
            Task<int> idTask = GetIdAsync();
            Task<string> nameTask = GetNameAsync();
            await Task.WhenAll(idTask, nameTask);
            return $"Person Id:{idTask.Result}, Name:{nameTask.Result}";
        }

        static async Task<int> GetIdAsync()
        {
            //var intTask = Task.Run(async () =>
            //{
            //    await Task.Delay(2500);
            //    return 1;
            //});
            await Task.Delay(2500);
            return 1;
        }

        static async Task<string> GetNameAsync()
        {
            await Task.Delay(3500);
            return "SuperHero";
        }
    }
}
