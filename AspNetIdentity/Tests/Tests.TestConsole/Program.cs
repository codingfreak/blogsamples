using System;
using System.Linq;

namespace codingfreaks.AspNetIdentity.Tests.TestConsole
{
    using System;
    using System.Linq;

    using Data.Core;

    class Program
    {
        static void Main(string[] args)
        {                       
            using (var ctx = ContextUtil.Context)
            {                
                Console.WriteLine(ctx.Roles.Count());
            }
            Console.ReadKey();
        }
    }
}
