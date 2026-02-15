namespace codingfreaks.blogsamples.CosmosEmulatorTest.Models
{
    public class Product
    {
        #region properties

        public long Id { get; set; }

        public ProductCategory? Category { get; set; } = null!;

        public long CategoryId { get; set; }

        public string Number { get; set; } = null!;

        public string Label { get; set; } = null!;

        public double Price { get; set; }

        #endregion
    }
}