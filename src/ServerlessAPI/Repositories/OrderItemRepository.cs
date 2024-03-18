using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using ServerlessAPI.Entities;

namespace ServerlessAPI.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly IDynamoDBContext context;
        private readonly ILogger<OrderItemRepository> logger;

        public OrderItemRepository(IDynamoDBContext context, ILogger<OrderItemRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        //Get orderitems by matching ID
        public async Task<IList<OrderItem>> GetOrderItemsAsync(int id)
        {
            var result = new List<OrderItem>();

            try
            {
                if (id <= 0)
                {
                    return result;
                }

                var filter = new ScanFilter();
                filter.AddCondition("OrderId", ScanOperator.Equal, id);
                var scanConfig = new ScanOperationConfig()
                {
                    Filter = filter
                };
                var queryResult = context.FromScanAsync<OrderItem>(scanConfig);

                do
                {
                    result.AddRange(await queryResult.GetNextSetAsync());
                }
                while (!queryResult.IsDone);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "fail to list from DynamoDb Table");
                return new List<OrderItem>();
            }

            return result;
        }
    }
}
