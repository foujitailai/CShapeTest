using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Test1
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
	public class ColumnAttribute : System.Attribute
	{
		public string ColumnName { get; set; }
		public string ColumnAlias { get; set; }
		public ColumnAttribute(string columnName)
		{
			this.ColumnName = columnName;
		}
	}

	public class FirstClass
	{
		public static int _FirstVal = FirstClass2();

		static int FirstClass2()
		{
			Console.WriteLine($"FirstClass begin");
			return  SecondClass._SecondVal + 1;

			//Console.WriteLine($"FirstClass end");
		}
	}


	public class SecondClass
	{
		public static int _SecondVal = SecondClass2();

		static int SecondClass2()
		{
			Console.WriteLine($"SecondClass begin/end");
			return 2;
		}
	}
    class Program
    {
        static void Main(string[] args)
        {
			Console.WriteLine($"Main begin");
			t5();
			Console.WriteLine($"Main end");
            Console.ReadKey();
        }

		static void t1()
		{
			new TestAsync().StartAsyncFunc();
		}

		static void t2()
		{
			Console.WriteLine($"t2 begin");
			Console.WriteLine("FirstVal={0}, SecondVal={1}", FirstClass._FirstVal, SecondClass._SecondVal);
			Console.WriteLine($"t2 end");
		}

		static void t3()
		{
			var t = Type.GetType("Test1.FirstClass");
			var ti = Activator.CreateInstance(t) as FirstClass;
			Console.WriteLine(ti.ToString());
		}

        static void t4()
        {
            new TestValueConvert().Test();
        }
        static void t5()
        {
            new testAttribute().Test();
        }
    }
}
