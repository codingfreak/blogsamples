namespace codingfreaks.blogsamples.CosmosEmulatorTest.Models
{
    public class ProductCategory
    {
        #region properties

        public long Id { get; set; }

        public string Label { get; set; } = null!;

        public ICollection<Product> Products { get; set; }

        #endregion
    }
}