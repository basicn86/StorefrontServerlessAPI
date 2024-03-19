using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerlessAPI.Repositories;

namespace ServerlessAPI.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> logger;
        private readonly IProductRepository productRepository;

        public ProductsController(ILogger<ProductsController> logger, IProductRepository productRepository)
        {
            this.logger = logger;
            this.productRepository = productRepository;
        }


        //Get products
        [HttpGet]
        public async Task<ActionResult<List<Entities.Product>>> Get([FromQuery] int limit = 10)
        {
            return Ok(await productRepository.GetProductsAsync(limit));
        }

        //POST method, clears the table and adds new products
        [HttpPost]
        public async Task<ActionResult> Post()
        {
            try
            {
                //clear the table
                await productRepository.DeleteAllProductsAsync();

                //create three new products
                var newProducts = new List<Entities.Product>
                {
                    new Entities.Product { Id = Guid.NewGuid(), Name = "Peanuts", Price = 10.99m },
                    new Entities.Product { Id = Guid.NewGuid(), Name = "Pineapple", Price = 20.99m },
                    new Entities.Product { Id = Guid.NewGuid(), Name = "Apples", Price = 30.99m }
                };

                await productRepository.AddProductsAsync(newProducts);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "fail to add products to DynamoDb Table");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
