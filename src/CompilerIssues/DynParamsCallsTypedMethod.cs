using System;

namespace CompilerIssues
{
    public class DynParamsCallsTypedMethod
    {
        public void Test()
        {
            dynamic dParam;
            
            dParam = true;
            Foo(dParam);
            Foo((dynamic)dParam);
            Foo((object)dParam);

            dParam = "string";
            Foo(dParam);
            Foo((dynamic)dParam);
            Foo((object)dParam);

            dParam = 123;
            Foo(dParam);
            Foo((dynamic)dParam);
            Foo((object)dParam);
        }

        //public void Foo(object value)
        //{
        //    Console.WriteLine($"Foo(object {value}) called.");
        //}

        public void Foo(dynamic value)
        {
            Console.WriteLine($"Foo(dynamic {value}) called.");
        }

        public void Foo<T>(T value)
        {
            Console.WriteLine($"Foo<{typeof(T)}>({typeof(T)} {value}) called.");
        }

        public void Foo(bool value)
        {
            Console.WriteLine($"Foo(bool {value}) called.");
        }

        public void Foo(string value)
        {
            Console.WriteLine($"Foo(string \"{value}\") called.");
        }
    }
}
