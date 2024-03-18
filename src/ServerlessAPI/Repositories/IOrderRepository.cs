namespace ServerlessAPI.Repositories
{
    public interface IOrderRepository
    {
        Task<IList<Entities.Order>> GetOrdersAsync(int limit = 10);
    }
}
