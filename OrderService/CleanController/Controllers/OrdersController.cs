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

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private IMediator _mediator = null!;
        private IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(
           ILogger<OrdersController> logger
         )
        {
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> Create(CreateOrderRequest request)
        {
            CreateOrderCommand command = new CreateOrderCommand()
            {
                TotalPrice = request.TotalPrice,
                OrderStatus = request.OrderStatus,
                UserId = request.CustomerId,
                DeliveryManId = request.DeliveryManId,
                Products = request.Products,
                AddressLine = request.AddressLine
            };

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
            UpdateOrderCommand command = new UpdateOrderCommand()
            {
                Id = request.Id,
                Date = request.Date,
                TotalPrice = request.TotalPrice,
                OrderStatus = request.OrderStatus,
                UserId = request.CustomerId,
                DeliveryManId = request.DeliveryManId,
                Products = request.Products,
                AddressLine = request.AddressLine
            }; 

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
