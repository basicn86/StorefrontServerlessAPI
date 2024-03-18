using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerlessAPI.Entities;
using ServerlessAPI.Repositories;

namespace ServerlessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<ProductsController> logger;
        private readonly IOrderRepository orderRepository;
        private readonly IOrderItemRepository orderItemRepository;

        public OrdersController(ILogger<ProductsController> logger, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            this.logger = logger;
            this.orderRepository = orderRepository;
            this.orderItemRepository = orderItemRepository;
        }

        //GET api/orders
        [HttpGet]
        public async Task<ActionResult<List<Order>>> Get([FromQuery] int limit = 10)
        {
            //get orders
            IList<Order> orders = await orderRepository.GetOrdersAsync(limit);

            //get order items
            foreach (var order in orders)
            {
                order.OrderItems = await orderItemRepository.GetOrderItemsAsync(order.Id);
            }

            return Ok(orders);
        }
    }
}
