namespace codingfreaks.DatabaseUnitTests.Tests.Data.Core
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using DatabaseUnitTests.Data.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Provides unit tests for customers.
    /// </summary>
    [TestClass]
    public class CustomerTests
    {
        #region methods

        /// <summary>
        /// Checks if the current amount of customers in database matches the expected one.
        /// </summary>
        [TestMethod]
        public async Task CheckCustomerCount()
        {
            // arrange
            const int ExpectedCount = 3;
            var realCount = -1;
            // act
            using (var ctx = new UnitTestSampleEntities())
            {
                realCount = await ctx.Customers.CountAsync();
            }
            // assert
            Assert.AreEqual(ExpectedCount, realCount);
        }

        #endregion
    }
}