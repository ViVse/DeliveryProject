using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Orders.Commands.CreateOrder;
using Application.Orders.Commands.DeleteOrder;
using Application.Orders.Commands.UpdateOrder;
using Application.Orders.Queries.GetOrderById;
using Application.Orders.Queries.GetOrdersWithPagination;
using Domain.Entities;
using Infrastructure.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace CleanController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private IMediator _mediator = null!;
        private IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
        private readonly ILogger<OrdersController> _logger;


        /* 
         * Used to check whether ids are present in databases
         * add depandencies before uncommenting
         private readonly IUsersService _usersService;
         private readonly IProductService _productService;
         private readonly IDeliveryManService _deliveryManService;
         private readonly IAddressService _addressService;*/

        public OrdersController(
           ILogger<OrdersController> logger
         /*  IUsersService usersService,
           IProductService productService,
           IDeliveryManService deliveryManService,
           IAddressService addressService */
         )
        {
            _logger = logger;
           /* _usersService = usersService;
            _productService = productService;
            _deliveryManService = deliveryManService;
            _addressService = addressService;*/
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> Create(CreateOrderRequest request)
        {
            CreateOrderCommand command;
            try
            {
               /* var customer = await _usersService.GetByIdAsync(request.CustomerId);
                var deliveryMan = await _deliveryManService.GetByIdAsync(request.DeliveryManId);
                var address = await _addressService.GetByIdAsync(request.AddressId);*/
                float totalPrice = 0;
                List<Product> products = new List<Product>();
                foreach (int productId in request.Products.Keys)
                {
                    /*var product = await _productService.GetByIdAsync(productId);*/
                    var newProduct = new Product()
                    {
                        Id = productId,
                        Name = /*product.Name*/ "",
                        Price = /*product.Price*/ 0,
                        Amount = request.Products[productId]
                    };
                    totalPrice += newProduct.Price * newProduct.Amount;
                    products.Add(newProduct);
                }
                command = new CreateOrderCommand()
                {
                    TotalPrice = totalPrice,
                    OrderStatus = request.OrderStatus,
                    Customer = new Customer() { Id = request.CustomerId, FullName = /*customer.FullName*/ "", Phone = /*customer.PhoneNumber */ ""},
                    DeliveryMan = new DeliveryMan() { Id = request.DeliveryManId, FullName = /*deliveryMan.FirstName + " " + deliveryMan.LastName*/ "", Phone = /*deliveryMan.Phone */ ""},
                    Products = products,
                    Address = new Address() { Id = request.AddressId, AddressLine = /*$"{address.City}, {address.AddressLine}" */ ""}
                };
            }
            catch (NotFoundException ex)
            {
                string errorMessage = "Couldn't find entity with specified id: " + ex.Message;
                _logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }

            try
            {
                string createdId = await Mediator.Send(command);
                return Ok(createdId);
            }
            catch (ValidationException ex)
            {
                StringBuilder errorMessage = new StringBuilder("Validation Error! OrdersController.Create():\n");
                foreach (var key in ex.Errors.Keys)
                {
                    errorMessage.Append($"{key}: {ex.Errors[key][0]}\n");
                }
                _logger.LogError(errorMessage.ToString());
                return BadRequest(errorMessage.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server Error! OrdersController.Create(): {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> Update(UpdateOrderRequest request)
        {
            UpdateOrderCommand command;
            try
            {
               /* var customer = await _usersService.GetByIdAsync(request.CustomerId);
                var deliveryMan = await _deliveryManService.GetByIdAsync(request.DeliveryManId);
                var address = await _addressService.GetByIdAsync(request.AddressId);*/
                float totalPrice = 0;
                List<Product> products = new List<Product>();
                foreach (int productId in request.Products.Keys)
                {
                    /*var product = await _productService.GetByIdAsync(productId);*/
                    var newProduct = new Product()
                    {
                        Id = productId,
                        Name = /*product.Name*/ "",
                        Price = /*product.Price*/ 0,
                        Amount = request.Products[productId]
                    };
                    totalPrice += newProduct.Price * newProduct.Amount;
                    products.Add(newProduct);
                }
                command = new UpdateOrderCommand()
                {
                    Id = request.Id,
                    Date = request.Date,
                    TotalPrice = totalPrice,
                    OrderStatus = request.OrderStatus,
                    Customer = new Customer() { Id = request.CustomerId, FullName = /*customer.FullName*/ "", Phone = /*customer.PhoneNumber */ ""},
                    DeliveryMan = new DeliveryMan() { Id = request.DeliveryManId, FullName = /*deliveryMan.FirstName + " " + deliveryMan.LastName*/ "", Phone = /*deliveryMan.Phone */ ""},
                    Products = products,
                    Address = new Address() { Id = request.AddressId, AddressLine = /*$"{address.City}, {address.AddressLine}" */ ""}
                };
            }
            catch (NotFoundException ex)
            {
                string errorMessage = "Couldn't find entity with specified id: " + ex.Message;
                _logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }

            try
            {
                string updatedId = await Mediator.Send(command);
                return Ok(updatedId);
            }
            catch (ValidationException ex)
            {
                StringBuilder errorMessage = new StringBuilder("Validation Error! OrdersController.Update():\n");
                foreach (var key in ex.Errors.Keys)
                {
                    errorMessage.Append($"{key}: {ex.Errors[key][0]}\n");
                }
                _logger.LogError(errorMessage.ToString());
                return BadRequest(errorMessage.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server Error! OrdersController.Update(): {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedList<OrderBriefDto>>> Get([FromQuery] GetOrdersWithPaginationQuery command)
        {
            try
            {
                return Ok(await Mediator.Send(command));
            }
            catch (ValidationException ex)
            {
                StringBuilder errorMessage = new StringBuilder("Validation Error! OrdersController.Get():\n");
                foreach (var key in ex.Errors.Keys)
                {
                    errorMessage.Append($"{key}: {ex.Errors[key][0]}\n");
                }
                _logger.LogError(errorMessage.ToString());
                return BadRequest(errorMessage.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server Error! OrdersController.Get(): {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OrderDto>> GetById(string id)
        {
            try
            {
                return Ok(await Mediator.Send(new GetOrderByIdQuery(id)));
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"Validation Error! OrdersController.GetById(): {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server Error! OrdersController.GetById(): {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await Mediator.Send(new DeleteOrderCommand(id));
                return Ok();
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"Validation Error! OrdersController.Delete(): {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server Error! OrdersController.Delete(): {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
