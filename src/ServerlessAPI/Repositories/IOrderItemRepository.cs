namespace ServerlessAPI.Repositories
{
    public interface IOrderItemRepository
    {
        Task<IList<Entities.OrderItem>> GetOrderItemsAsync(int id);
        Task UpdateOrderItemsAsync(IList<Entities.OrderItem> orderItems);
    }
}
