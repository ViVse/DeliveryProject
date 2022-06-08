using Application.Common.Models;
using Application.Orders.Commands.CreateOrder;
using Application.Orders.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private IMediator _mediator = null!;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

        [HttpPost]
        public async Task<ActionResult<string>> Create(CreateOrderCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<Order>>> Get([FromQuery] GetOrdersWithPaginationQuery command)
        {
            return await Mediator.Send(command);
        }
    }
}
