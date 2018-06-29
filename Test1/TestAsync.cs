using System;
using System.Threading;
using System.Threading.Tasks;

namespace Test1
{
    class TestAsync
    {
        public TestAsync()
        {

        }

        public async void StartAsyncFunc()
        {
            Console.WriteLine($"StartAsyncFunc begin");

            int v = await InnerFunc();

            Console.WriteLine($"StartAsyncFunc end  : {v}");
        }

        private async Task<int> InnerFunc()
        {
            Console.WriteLine($"InnerFunc begin");

            await Task.Run(() =>
            {
                for (int i = 0; i < 4; i++)
                {
                    Console.WriteLine($"Task run {i}");
                    Thread.Sleep(100);
                }
            });

            Console.WriteLine($"InnerFunc end");

            return 3;
        }

    }
}