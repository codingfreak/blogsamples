namespace codingfreaks.blogsamples.CosmosEmulatorTest
{
    using System.Diagnostics;

    using Bogus;

    using Extensions;

    using Microsoft.Azure.Cosmos;
    using Microsoft.EntityFrameworkCore;

    using Models;

    internal class Program
    {
        #region constants

        private static readonly int CategoriesToCreate = 20;

        private static readonly int ProductsPerCategoryToCreate = 50;

        #endregion

        #region methods

        private static IEnumerable<ProductCategory> GenerateCategories()
        {
            var categoryIds = 1;
            var productIds = 1;
            var productNumbers = 1;
            var productGenerator = new Faker<Product>().RuleFor(p => p.Id, _ => productIds++)
                .RuleFor(p => p.Price, f => f.Random.Double(0, 20))
                .RuleFor(
                    p => p.Number,
                    _ =>
                    {
                        var num = productNumbers++;
                        return $"P{num:D8}";
                    })
                .RuleFor(p => p.Label, f => f.Commerce.Product());
            var categoryGenerator = new Faker<ProductCategory>().StrictMode(true)
                .RuleFor(c => c.Id, _ => categoryIds++)
                .RuleFor(
                    c => c.Label,
                    f => f.Commerce.Categories(1)
                        .First())
                .RuleFor(c => c.Products, f => productGenerator.Generate(ProductsPerCategoryToCreate));
            return categoryGenerator.Generate(CategoriesToCreate);
        }

        private static async Task GetAveragePriceForCategoryAsync(SampleContext context, long categoryId)
        {
            var category = await context.ProductCategories.FindAsync(categoryId);
            var container = context.Database.GetCosmosClient()
                .GetContainer(Constants.CosmosDb, "Products");
            using var iterator = container.GetIterator<dynamic>(
                $"SELECT AVG(c.Price) AS AveragePrice FROM c WHERE c.CategoryId = {categoryId}");
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                foreach (var item in response)
                {
                    Console.WriteLine(
                        $"Category '{category?.Label ?? "?"}' average Price = {item.AveragePrice} (RU {response.RequestCharge})");
                }
            }
        }

        private static async Task GetCountAsync(string containerName)
        {
            using (var client = new CosmosClient(Constants.CosmosEndpoint, Constants.CosmosKey))
            {
                var container = client.GetContainer(Constants.CosmosDb, containerName);
                var result = await container.GetCountAsync();
                Console.WriteLine($"Items in {containerName}: {result.Item1} (RU {result.Item2})");
            }
        }

        private static async Task GetProductsAsync(SampleContext context)
        {
            var categories = await context.ProductCategories.AsNoTracking()
                .ToListAsync();
            var products = await context.Products.AsNoTracking()
                .Skip(ProductsPerCategoryToCreate - 1)
                .Take(ProductsPerCategoryToCreate)
                .ToListAsync();
            var categoriesArray = categories.Cast<ProductCategory>()
                .ToArray();
            products.ForEach(p => p.Category = categoriesArray.SingleOrDefault(c => c.Id == p.CategoryId));
            var first = products.First();
            var last = products.Last();
            Console.WriteLine($"First product with id {first.Id} has category '{first.Category?.Label}'.");
            Console.WriteLine($"Last product with id {last.Id} has category '{last.Category?.Label}'.");
        }

        private static async Task InitDatabaseAsync(SampleContext context)
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            await WriteThroughputsAsync(context);
        }

        private static async Task Main(string[] args)
        {
            await RunFillBenchAsync();
            await RunReadBenchAsync();
        }

        private static async Task RunDemoAsync(ProductCategory[] categories, bool oneByOne = false)
        {
            using (var context = new SampleContext())
            {
                await InitDatabaseAsync(context);
                Console.WriteLine("Database created");
                await SeedEntriesAsync(context, categories, oneByOne);
                Console.WriteLine("Seeding completed.");
            }
        }

        private static async Task RunFillBenchAsync()
        {
            var categories = GenerateCategories()
                .ToArray();
            await RunDemoAsync(categories);
            //await RunDemoAsync(categories, true);
        }

        private static async Task RunReadBenchAsync()
        {
            await GetCountAsync("ProductCategories");
            await GetCountAsync("Products");
            using (var context = new SampleContext())
            {
                //await WriteThroughputsAsync(context);
                await GetProductsAsync(context);
                await GetAveragePriceForCategoryAsync(context, 1);
            }
        }

        private static async Task SeedEntriesAsync(
            SampleContext context,
            ProductCategory[] categories,
            bool oneByOne = false)
        {
            Randomizer.Seed = new Random(DateTime.Now.Microsecond);
            var watch = new Stopwatch();
            watch.Start();
            var amount = categories.Count();
            if (!oneByOne)
            {
                await context.ProductCategories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
                Console.WriteLine($"Stored {amount} categories at once in {watch.Elapsed}.");
                return;
            }
            foreach (var category in categories)
            {
                await context.ProductCategories.AddAsync(category);
                await context.SaveChangesAsync();
            }
            Console.WriteLine($"Stored {amount} categories one by one in {watch.Elapsed}.");
        }

        private static async Task WriteThroughputsAsync(SampleContext context)
        {
            var client = context.Database.GetCosmosClient();
            var tpCategories = await client.GetDatabase("Sample")
                .GetContainer("ProductCategories")
                .ReadThroughputAsync();
            var tpProducts = await client.GetDatabase("Sample")
                .GetContainer("Products")
                .ReadThroughputAsync();
            Console.WriteLine($"Category container throughput is {tpCategories}");
            Console.WriteLine($"Product container throughput is {tpProducts}");
        }

        #endregion
    }
}