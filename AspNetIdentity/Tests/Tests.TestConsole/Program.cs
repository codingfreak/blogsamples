namespace codingfreaks.AspNetIdentity.Tests.TestConsole
{
    using System;
    using System.Linq;

    using Autofac;

    using Logic.Core.Utils;
    using Logic.Shared.Interfaces;
    using Logic.Shared.TransportModels;

    internal class Program
    {
        #region methods

        private static void CheckUserTest()
        {
            var instance = StartupUtil.Container.Resolve<IUserRepository>();
            Console.WriteLine(instance.UserExistsAsync("testuser").Result);
        }

        private static void CreateUserTest()
        {
            var instance = StartupUtil.Container.Resolve<IUserRepository>();
            var result = instance.AddUserAsnyc(
                new UserTransportModel
                {
                    UserName = "testuser2",
                    Email = "test2@test.de",
                    PasswordHash = "fjdklfjdsdsdlfjlsdjfklsdjfklsdjflsdk"
                },
                "User").Result;
            Console.WriteLine($"User with id {result} created.");
        }

        private static void Main(string[] args)
        {
            StartupUtil.InitLogic();
            //CreateUserTest();
            //CheckUserTest();            
            Console.ReadKey();
        }

        #endregion
    }
}