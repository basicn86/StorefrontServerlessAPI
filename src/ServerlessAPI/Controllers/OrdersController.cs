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

        //POST order
        [HttpPost]
        public async Task<ActionResult<Order>> Post([FromBody] Order order)
        {
            //Verify that there at least one item in the order with a quantity greater than 0
            if (order.OrderItems == null || order.OrderItems.Count == 0 || order.OrderItems.All(x => x.Quantity <= 0))
            {
                return BadRequest("Order must contain at least one item with a quantity greater than 0");
            }

            try
            {
                await orderRepository.UpdateOrderAsync(order);
                //very that the order items are assigned to the order
                foreach (var orderItem in order.OrderItems) orderItem.OrderId = order.Id;
                await orderItemRepository.UpdateOrderItemsAsync(order.OrderItems);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "fail to create order");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(order);
        }

        //Update order
        [HttpPut]
        public async Task<ActionResult<Order>> Put([FromBody] Order order)
        {
            //Verify that there at least one item in the order with a quantity greater than 0
            if (order.OrderItems == null || order.OrderItems.Count == 0 || order.OrderItems.All(x => x.Quantity <= 0))
            {
                return BadRequest("Order must contain at least one item with a quantity greater than 0");
            }

            try
            {
                await orderRepository.UpdateOrderAsync(order);
                //very that the order items are assigned to the order
                foreach (var orderItem in order.OrderItems) orderItem.OrderId = order.Id;
                await orderItemRepository.UpdateOrderItemsAsync(order.OrderItems);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "fail to update order");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(order);
        }

        //delete order
        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery] int id)
        {
            try
            {
                await orderItemRepository.DeleteOrderItemsAsync(id);
                await orderRepository.DeleteOrderAsync(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "fail to delete order");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok();
        }
    }
}
