using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services.Interfaces;

namespace Shopping.Aggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly IShopService _shopService;

        public ShoppingController(IUserService userService, IOrderService orderService, IShopService shopService)
        {
            _userService = userService;
            _orderService = orderService;
            _shopService = shopService;
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<ExtendedOrderModel>> GetOrder(string orderId)
        {
            var order = await _orderService.GetOrder(orderId);
            var user = await _userService.GetUser(order.UserId);

            List<ExtendedProductModel> extendedProducts = new List<ExtendedProductModel>();

            foreach(var product in order.Products)
            {
                var shop = await _shopService.GetShop(product.ShopId);
                ExtendedProductModel newProduct = new ExtendedProductModel
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Quantity = product.Quantity,
                    Price = product.Price,
                    Shop = shop
                };
                extendedProducts.Add(newProduct);
            }

            var res = new ExtendedOrderModel
            {
                Id = orderId,
                User = user,
                TotalPrice = order.TotalPrice,
                Date = order.Date,
                DeliveryManId = order.DeliveryManId,
                AddressLine = order.AddressLine,
                Products = extendedProducts
            };

            return Ok(res);
        }
    }
}
