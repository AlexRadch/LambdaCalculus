using System;

namespace CompilerIssues
{
    class Program
    {
        static void Main(string[] args)
        {
            new DynParamsCallsTypedMethod().Test();
            Console.ReadLine();
        }
    }
}
