using System;
using System.Threading.Tasks;

namespace AsyncAwait
{
    class AsyncException_07
    {
        public async static void ExecuteAsync()
        {
            try
            {
                var message = await GetNameAsync(); //.Result for sync
            }
            catch(AggregateException ae)
            {
                Console.WriteLine("We knew this! " + ae.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Oh snap! " + ex.Message); //THIS IS HIT on await
            }
        }

        static async Task<string> GetNameAsync()
        {
            await Task.Delay(3500);
            var k = 0;
            var yo = 10 / k;
            return "SuperHero";
        }
    }
}
