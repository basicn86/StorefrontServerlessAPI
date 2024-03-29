﻿namespace ServerlessAPI.Repositories
{
    public interface IOrderRepository
    {
        Task<IList<Entities.Order>> GetOrdersAsync(int limit = 10);

        //Update order
        Task UpdateOrderAsync(Entities.Order order);

        Task DeleteOrderAsync(Guid id);

        Task AddOrderAsync(Entities.Order order);
    }
}
