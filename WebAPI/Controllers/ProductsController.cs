using BLL.DTO.Requests;
using BLL.DTO.Responses;
using BLL.Interfaces.Services;
using BLL.Validation.Requests;
using DAL.Exceptions;
using DAL.Pagination;
using DAL.Parameters;
using Delivery_Entity.Extensions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Delivery_Entity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        IProductService productService;
        IShopService shopService;

        public ProductsController(IProductService productService, IShopService shopService)
        {
            this.productService = productService;
            this.shopService = shopService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedList<ProductResponse>>> GetPagedAsync([FromQuery] ProductParameters parameters)
        {
            try
            {
                if(!parameters.ValidPriceRange)
                {
                    return BadRequest("Max price cannot be less than min price.");
                } else if(!parameters.ValidProductionTimeRange)
                {
                    return BadRequest("Max production time cannot be less than min production time.");
                }

                var products = await productService.GetAsync(parameters);

                Response.Headers.Add("X-Pagination", products.SerializeMetadata());

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductResponse>> GetByIdAsync(int id)
        {
            try
            {
                return Ok(await productService.GetByIdAsync(id));
            } catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> InsertAsync([FromBody] ProductRequest request)
        {
            try
            {
                ValidationResult results = new ProductRequestValidator().Validate(request);
                if (!results.IsValid)
                    return BadRequest(results.ToString("\n"));

                var shop = await shopService.GetByIdAsync(request.ShopId);
                if (shop == null)
                    return BadRequest("Invalid shopId");

                await productService.InsertAsync(request);
                return Ok();
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateAsync([FromBody] ProductRequest request)
        {
            try
            {
                ValidationResult results = new ProductRequestValidator().Validate(request);
                if (!results.IsValid)
                    return BadRequest(results.ToString("\n"));

                var shop = await shopService.GetByIdAsync(request.ShopId);
                if (shop == null)
                    return BadRequest("Invalid shopId");

                await productService.UpdateAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                await productService.DeleteAsync(id);
                return Ok();
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
