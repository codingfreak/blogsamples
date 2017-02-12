using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codingfreaks.AspNetIdentity.Tests.Logic.Core
{
    using AspNetIdentity.Logic.Core.Extensions;
    using AspNetIdentity.Logic.Core.Utils;
    using AspNetIdentity.Logic.Shared.Interfaces;
    using Autofac;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Contains unit tests for the type <see cref="UserExtensions" />
    /// </summary>
    [TestClass]
    public class UserExtensionTests
    {
        #region methods

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            StartupUtil.InitLogic(true);
        }

        [TestMethod]
        public void ToTransportModelTest()
        {
            // arrange
            var instance = StartupUtil.Container.Resolve<IUserRepository>();
            var userId = 1;
            // act
            var testData = instance.GetUserByIdAsync(userId).Result;
            // assert
            Assert.IsNotNull(testData);
            Assert.AreEqual(userId, testData.Id);
        }

        #endregion
    }
}
