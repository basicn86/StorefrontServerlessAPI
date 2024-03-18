﻿using Amazon.DynamoDBv2.DataModel;
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
    }
}