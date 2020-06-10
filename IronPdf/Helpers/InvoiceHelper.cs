namespace codingfreaks.blogsamples.IronPdf.Helpers
{
    using System;
    using System.Linq;

    using MMLib.RapidPrototyping.Generators;
    using MMLib.RapidPrototyping.Generators.Repositories;

    using Models;

    public static class InvoiceHelper
    {
        #region methods

        public static Invoice GenerateRandomInvoice(int positionsAmount)
        {
            var random = new Random(DateTime.Now.Millisecond);
            var loremIpsumGenerator = new LoremIpsumGenerator();
            var wordGenerator = new WordGenerator(DateTime.Now.Millisecond);
            return new Invoice
            {
                Number = random.Next(10000, 20000).ToString(),
                InvoiceDate = DateTimeOffset.Now,
                Positions = Enumerable.Range(1, positionsAmount).ToList().Select(
                    i => new InvoicePosition
                    {
                        OrderNumber = i,
                        Title = loremIpsumGenerator.Next(1,1),
                        Quantity = random.Next(1, 20),
                        Unit = wordGenerator.Next().Substring(0, 3),
                        UnitPrice = random.Next(1, 100000)
                    }).ToArray()
            };
        }

        #endregion
    }
}