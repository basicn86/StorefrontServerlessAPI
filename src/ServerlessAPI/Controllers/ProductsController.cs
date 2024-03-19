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
        public async Task<ActionResult> Post([FromBody] List<Entities.Product> products)
        {
            try
            {
                //clear the table
                await productRepository.ClearProductsAsync();

                //add new products
                await productRepository.AddProductsAsync(products);

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
