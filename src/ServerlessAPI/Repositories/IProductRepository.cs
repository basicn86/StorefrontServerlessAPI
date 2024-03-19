namespace ServerlessAPI.Repositories
{
    public interface IProductRepository
    {
        Task<IList<Entities.Product>> GetProductsAsync(int limit = 10);

        //delete all
        Task DeleteAllProductsAsync();

        Task AddProductsAsync(IList<Entities.Product> products);
    }
}
