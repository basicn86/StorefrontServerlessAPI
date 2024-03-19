namespace ServerlessAPI.Repositories
{
    public interface IOrderItemRepository
    {
        Task<IList<Entities.OrderItem>> GetOrderItemsAsync(Guid id);
        Task UpdateOrderItemsAsync(IList<Entities.OrderItem> orderItems);

        Task DeleteOrderItemsAsync(int id);

        Task AddOrderItemsAsync(IList<Entities.OrderItem> orderItem);
    }
}
