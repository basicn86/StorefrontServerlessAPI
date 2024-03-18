using ServerlessAPI.Entities;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace ServerlessAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDynamoDBContext context;
        private readonly ILogger<ProductRepository> logger;

        public ProductRepository(IDynamoDBContext context, ILogger<ProductRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<IList<Product>> GetProductsAsync(int limit = 10)
        {
            var result = new List<Product>();

            try
            {
                if (limit <= 0)
                {
                    return result;
                }

                var filter = new ScanFilter();
                filter.AddCondition("Id", ScanOperator.IsNotNull);
                var scanConfig = new ScanOperationConfig()
                {
                    Limit = limit,
                    Filter = filter
                };
                var queryResult = context.FromScanAsync<Product>(scanConfig);

                do
                {
                    result.AddRange(await queryResult.GetNextSetAsync());
                }
                while (!queryResult.IsDone && result.Count < limit);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "fail to list books from DynamoDb Table");
                return new List<Product>();
            }

            return result;
        }
    }
}
