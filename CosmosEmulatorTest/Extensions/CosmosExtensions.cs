namespace codingfreaks.blogsamples.CosmosEmulatorTest.Extensions
{
    using Microsoft.Azure.Cosmos;

    internal static class CosmosExtensions
    {
        extension(Container container)
        {
            #region methods

            public async ValueTask<(int, double)> GetCountAsync()
            {
                using var iterator = container.GetIterator<int>("SELECT VALUE COUNT(1) FROM c");
                var totalCount = 0;
                double totalCharge = 0;
                while (iterator.HasMoreResults)
                {
                    var response = await iterator.ReadNextAsync();
                    totalCount += response.Resource.First();
                    totalCharge += response.RequestCharge;
                }
                return (totalCount, totalCharge);
            }

            public FeedIterator<T> GetIterator<T>(string queryText)
            {
                var query = new QueryDefinition(queryText);
                return container.GetItemQueryIterator<T>(query, requestOptions: new QueryRequestOptions());
            }

            #endregion
        }
    }
}