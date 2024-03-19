using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using ServerlessAPI.Entities;

namespace ServerlessAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        //Constructor
        private readonly IDynamoDBContext context;
        private readonly ILogger<OrderRepository> logger;

        public OrderRepository(IDynamoDBContext context, ILogger<OrderRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        //Get orders
        public async Task<IList<Order>> GetOrdersAsync(int limit = 10)
        {
            var result = new List<Order>();

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
                var queryResult = context.FromScanAsync<Order>(scanConfig);

                do
                {
                    result.AddRange(await queryResult.GetNextSetAsync());
                }
                while (!queryResult.IsDone && result.Count < limit);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "fail to list from DynamoDb Table");
                return new List<Order>();
            }

            return result;
        }

        //Add new order
        public async Task AddOrderAsync(Order order)
        {
            try
            {
                //make sure the order will receive a new id
                order.Id = Guid.NewGuid();

                //give all order items the order id, and a new GUID for the order item id
                foreach (var orderItem in order.OrderItems)
                {
                    orderItem.Id = Guid.NewGuid();
                    orderItem.OrderId = order.Id;
                }

                await context.SaveAsync(order);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "fail to create order");
                throw;
            }
        }

        //Update order
        public async Task UpdateOrderAsync(Order order)
        {
            try
            {
                await context.SaveAsync(order);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "fail to update order");
                throw;
            }
        }

        //Delete order
        public async Task DeleteOrderAsync(Guid id)
        {
            try
            {
                await context.DeleteAsync<Order>(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "fail to delete order");
                throw;
            }
        }
    }
}
