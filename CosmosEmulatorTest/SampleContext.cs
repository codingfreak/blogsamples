namespace codingfreaks.blogsamples.CosmosEmulatorTest
{
    using Microsoft.EntityFrameworkCore;

    using Models;

    public class SampleContext : DbContext
    {
        #region methods

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.LogTo(Console.WriteLine);
            optionsBuilder.UseCosmos(Constants.CosmosEndpoint, Constants.CosmosKey, Constants.CosmosDb);
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>()
                .ToContainer("ProductCategories")
                .HasPartitionKey(e => e.Id)
                .UseETagConcurrency();
            modelBuilder.Entity<Product>()
                .ToContainer("Products")
                .HasPartitionKey(e => e.CategoryId)
                .UseETagConcurrency();
        }

        #endregion

        #region properties

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<Product> Products { get; set; }

        #endregion
    }
}