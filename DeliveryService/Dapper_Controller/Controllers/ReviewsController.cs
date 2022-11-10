using Dapper_BLL.DTO.Requests;
using Dapper_BLL.DTO.Responses;
using Dapper_BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dapper_Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ILogger<ReviewsController> _logger;
        private IReviewService reviewService;

        public ReviewsController(ILogger<ReviewsController> logger, IReviewService reviewService)
        {
            this.reviewService = reviewService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewResponse>>> GetAll()
        {
            try
            {
                var result = await reviewService.GetAsync();
                _logger.LogInformation($"Returned all reviews from database");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Transaction Failed! Something went wrong inside GetAll() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewResponse>> Get(int id)
        {
            try
            {
                var result = await reviewService.GetByIdAsync(id);
                if (result == null)
                {
                    _logger.LogError($"Review with id: {id} was not found.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Returned review with id {id}");
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Transaction Failed! Something went wrong inside Get() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ReviewRequest newReview)
        {
            try
            {
                if (newReview == null)
                {
                    _logger.LogError("Review sent by client is null");
                    return BadRequest("Review object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Review object is invalid");
                    return BadRequest("Ivalid model object");
                }
                await reviewService.InsertAsync(newReview);
                _logger.LogInformation("Inserted new review");
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Transaction Failed! Something went wrong inside Post() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ReviewRequest updatedReview)
        {
            try
            {
                if (updatedReview == null)
                {
                    _logger.LogError("Product sent by client is null");
                    return BadRequest("Product object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Product object is invalid");
                    return BadRequest("Ivalid model object");
                }
                var review = await reviewService.GetByIdAsync(id);
                if (review == null)
                {
                    _logger.LogError($"Review with id: {id} was not found.");
                    return NotFound();
                }
                await reviewService.UpdateAsync(updatedReview);
                _logger.LogInformation($"Updated review {id}");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Transaction Failed! Something went wrong inside Put() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var review = await reviewService.GetByIdAsync(id);
                if (review == null)
                {
                    _logger.LogError($"Review with id: {id} was not found.");
                    return NotFound();
                }
                await reviewService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Transaction Failed! Something went wrong inside Delete() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
