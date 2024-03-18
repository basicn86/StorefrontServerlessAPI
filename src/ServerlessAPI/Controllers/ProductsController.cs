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
    }
}
