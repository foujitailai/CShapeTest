using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Test1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Main begin");
            new TestAsync().StartAsyncFunc();
            Console.WriteLine($"Main end");

            Console.ReadKey();

            new TestValueConvert().Test();
        }

    }
}
