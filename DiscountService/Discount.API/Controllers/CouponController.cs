using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ICouponRepository _repository;

        public CouponController(ICouponRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("{code}", Name = "GetCoupon")]
        [ProducesResponseType(typeof(Coupon), 200)]
        public async Task<ActionResult<Coupon>> GetCoupon(string code)
        {
            var coupon = await _repository.GetCoupon(code);
            return Ok(coupon);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Coupon), 201)]
        public async Task<ActionResult<Coupon>> CreateCoupon([FromBody] Coupon coupon)
        {
            await _repository.CreateCoupon(coupon);
            return CreatedAtRoute("GetCoupon", new {code = coupon.Code}, coupon);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Coupon), 200)]
        public async Task<ActionResult<Coupon>> UpdateCoupon([FromBody] Coupon coupon)
        {
            return Ok(await _repository.UpdateCoupon(coupon));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Coupon), 200)]
        public async Task<ActionResult<bool>> DeleteCoupon(int id)
        {
            return Ok(await _repository.DeleteCoupon(id));
        }
    }
}
