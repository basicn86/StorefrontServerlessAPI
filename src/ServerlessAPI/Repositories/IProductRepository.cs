namespace ServerlessAPI.Repositories
{
    public interface IProductRepository
    {
        Task<IList<Entities.Product>> GetProductsAsync(int limit = 10);
    }
}
