using AutoMapper;
using Cart.API.Entities;
using Cart.API.GrpcServices;
using Cart.API.Repositories.Interfaces;
using Cart.API.Requests;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Cart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ProductDiscountGrpcService _discountGrpcService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ICartRepository _repository;
        private readonly IMapper _mapper;

        public CartController(ICartRepository repository, 
            ProductDiscountGrpcService discountGrpcService, 
            IPublishEndpoint publishEndpoint,
            IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        [HttpGet("{userId}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userId)
        {
            var basket = await _repository.GetBasket(userId);
            return Ok(basket ?? new ShoppingCart(userId));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            //Comunicate with ProductDiscount Grpc
            foreach(var item in basket.Items)
            {
                var discount = await _discountGrpcService.GetDiscount(item.ProductId);
                if(discount.IsPercent)
                {
                    var discountAmount = item.Price / 100 * discount.Amount;
                    item.Price -= discountAmount;
                } else
                {
                    item.Price -= discount.Amount;
                }
            }

            return Ok(await _repository.UpdateBasket(basket));
        }

        [HttpDelete("{userId}", Name = "DeleteBasket")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userId)
        {
            await _repository.DeleteBasket(userId);
            return Ok();
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request)
        {
            var basket = await _repository.GetBasket(request.UserId);
            if (basket == null)
                return BadRequest();

            BasketCheckoutEvent checkout = new BasketCheckoutEvent()
            {
                UserId = request.UserId,
                AddressLine = request.AddressLine,
                CardName  = request.CardName,
                CardNumber = request.CardNumber,
                Expiration = request.Expiration,
                CVV = request.CVV,
                TotalPrice = basket.TotalPrice,
                Products = basket.Items.Select(_mapper.Map<ShoppingCartItem, Product>).ToList()
            };
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(checkout);
            await _publishEndpoint.Publish(eventMessage);

            await _repository.DeleteBasket(request.UserId);
            return Accepted();
        }
    }
}
