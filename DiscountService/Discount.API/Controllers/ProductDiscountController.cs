using Discount.API.Entities;
using Discount.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDiscountController : ControllerBase
    {
        private readonly IProductDiscountRepository _repository;

        public ProductDiscountController(IProductDiscountRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("{id}", Name = "GetDiscount")]
        [ProducesResponseType(typeof(ProductDiscount), 200)]
        public async Task<ActionResult<ProductDiscount>> GetDiscount(int id)
        {
            var discount = await _repository.GetDiscount(id);
            return Ok(discount);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductDiscount), 201)]
        public async Task<ActionResult<ProductDiscount>> CreateDiscount([FromBody] ProductDiscount discount)
        {
            await _repository.CreateDiscount(discount);
            return CreatedAtRoute("GetDiscount", new { id = discount.ProductId }, discount);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ProductDiscount), 200)]
        public async Task<ActionResult<ProductDiscount>> UpdateDiscount([FromBody] ProductDiscount discount)
        {
            return Ok(await _repository.UpdateDiscount(discount));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ProductDiscount), 200)]
        public async Task<ActionResult<bool>> DeleteDiscount(int id)
        {
            return Ok(await _repository.DeleteDiscount(id));
        }
    }
}
