using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Test1
{
    namespace Attribute.Atts
    {
        //这个特性可以标记在类上也可以标记在方法上
        //是通过 AttributeTargets.Class，AttributeTargets.Method 约束的
        [System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
        public class Att1 : System.Attribute
        {
            public Att1(string name)
            {
                m_name = name;
                version = "1.0";
                other = "";
            }
            /// <summary>
            /// 名称（定位参数）
            /// </summary>
            private string m_name;
            /// <summary>
            /// 获取名称（定位参数）
            /// </summary>
            public string GetName
            {
                get { return m_name; }
            }
            /// <summary>
            /// 版本号（命名参数）
            /// </summary>
            public string version;
            /// <summary>
            /// 其它（命名参数）
            /// </summary>
            public string other { set; get; }
        }
    }

    namespace Attribute.utility
    {
        //为了测试方便，定义了一个公共接口
        public interface ITestBase
        {
            string UserName();
        }

        //同样也是为了测试方便，定义了派生公共接口的基类
        //供测试的类型继承，这样测试时候就直接使用多态特性调用类方法就OK了！
        public class TestBase : ITestBase
        {
            public TestBase(string userName)
            {
                m_userName = userName;
            }
            private string m_userName = "";
            public string UserName()
            {
                return m_userName;
            }
        }

        //注意了！这个就是我们想得到的那个类哦！
        [Attribute.Atts.Att1("Test1", other = "test1", version = "1.1")]
        public class Test1 : TestBase
        {
            public Test1()
                : base("Test1")
            {

            }
        }

        //测试类 Test2
        public class Test2 : TestBase
        {
            public Test2()
                : base("Test2")
            {

            }

            [Attribute.Atts.Att1("Test2", other = "test1", version = "1.1")]
            public void FunTest()
            {
            }
        }

        //测试类 Test3
        public class Test3 : TestBase
        {

            [Attribute.Atts.Att1("Test3", other = "test1", version = "1.1")]
            public int TestInt { get; set; }

            public Test3()
                : base("Test3")
            {

            }
        }
    }

    public class FindAttribute
    {
        private static bool HasAttribute<T,T2>(T2[] o)
        {
            foreach (T2 a in o)
                if (a is T)
                    return true;

            return false;
        }

        //private static bool HasAttribute<T>(object[] o)
        //{
        //    foreach (System.Attribute a in o)
        //        if (a is T)
        //            return true;

        //    return false;
        //}

        public static List<Type> FindClassHasAttribute<T>(System.Reflection.Assembly assembly)
        {
            var types = assembly.GetExportedTypes();
            return types.Where(
                o => HasAttribute<T, System.Attribute>(System.Attribute.GetCustomAttributes(o, true))
            ).ToList();
        }

        public static List<MethodInfo> FindMethedHasAttribute<T>(Type target)
        {
            //var t = typeof(T);
            var t = target;
            var methods = t.GetMethods();
            return methods.Where(
                method => HasAttribute<T, object>(method.GetCustomAttributes(true))
            ).ToList();
        }

        public static List<PropertyInfo> FindPropertyHasAttribute<T>(Type target)
        {
            //var t = typeof(T);
            var t = target;
            var props = t.GetProperties();
            return props.Where(
                prop => HasAttribute<T, object>(prop.GetCustomAttributes(true))
            ).ToList();
        }
    }

    public class testAttribute
    {
        public void Test()
        {
            {
                //加载程序集信息
                var asm = System.Reflection.Assembly.GetExecutingAssembly();

                var types = FindAttribute.FindClassHasAttribute<Attribute.Atts.Att1>(asm);

                //遍历具有指定特性的类型
                foreach (var t in types)
                {
                    //实例化了一个当前类型的实例
                    var obj = t.Assembly.CreateInstance(t.FullName);
                    if (obj != null)
                    {
                        Console.WriteLine(((Attribute.utility.ITestBase)obj).UserName());
                    }
                }
            }

            {
                var types = FindAttribute.FindMethedHasAttribute<Attribute.Atts.Att1>(typeof(Attribute.utility.Test2));
                foreach (var t in types)
                {
                    Console.WriteLine(t.ToString());
                }
            }

            {
                var types = FindAttribute.FindPropertyHasAttribute<Attribute.Atts.Att1>(typeof(Attribute.utility.Test3));
                foreach (var t in types)
                {
                    Console.WriteLine(t.ToString());
                }
            }

        }


        /// <summary>
        /// 通过反射筛选具有指定特性的类型
        /// </summary>
        public void Test2()
        {
            //加载程序集信息
            var asm = System.Reflection.Assembly.GetExecutingAssembly();

            //Type[] allTypes = asm.GetTypes();    //这个得到的类型有点儿多
            Type[] types = asm.GetExportedTypes(); //还是用这个比较好，得到的都是自定义的类型

            // 验证指定自定义属性（使用的是 4.0 的新语法，匿名方法实现的，不知道的同学查查资料吧！）
            Func<System.Attribute[], bool> IsAtt1 = o =>
            {
                foreach (System.Attribute a in o)
                {
                    if (a is Attribute.Atts.Att1)
                        return true;
                }
                return false;
            };

            //查找具有 Attribute.Atts.Att1 特性的类型（使用的是 linq 语法）
            Type[] CosType = types.Where(o =>
            {
                return IsAtt1(System.Attribute.GetCustomAttributes(o, true));
            }).ToArray();

            //遍历具有指定特性的类型
            object obj;
            foreach (Type t in CosType)
            {
                //实例化了一个当前类型的实例
                obj = t.Assembly.CreateInstance(t.FullName);

                if (obj != null)
                {
                    //这里封装了一个方法叫 ShowMsg 接收的是一个 string 参数，就是将 string 打印到出来
                    Console.WriteLine(
                        //这里就用到了多态特性了，调用了 UserName 方法（用接口是方便哈！）
                        ((Attribute.utility.ITestBase)obj).UserName()
                    );
                }
            }
        }
    }
}
