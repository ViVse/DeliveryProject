using BLL.DTO.Requests;
using BLL.DTO.Responses;
using BLL.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryMenController : ControllerBase
    {
        private readonly ILogger<DeliveryMenController> _logger;
        private IDeliveryManService deliveryManService;

        public DeliveryMenController(ILogger<DeliveryMenController> logger, IDeliveryManService deliveryManService)
        {
            this.deliveryManService = deliveryManService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryManResponse>>> GetAll()
        {
            try
            {
                var result = await deliveryManService.GetAsync();
                _logger.LogInformation($"Returned all deliveryMen from database");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Transaction Failed! Something went wrong inside GetAll() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryManResponse>> Get(int id)
        {
            try
            {
                var result = await deliveryManService.GetByIdAsync(id);
                if (result == null)
                {
                    _logger.LogError($"DeliveryMan with id: {id} was not found.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Returned deliveryMan with id {id}");
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
        public async Task<ActionResult> Post([FromBody] DeliveryManRequest newDeliveryMan)
        {
            try
            {
                if (newDeliveryMan == null)
                {
                    _logger.LogError("DeliveryMan sent by client is null");
                    return BadRequest("DeliveryMan object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("DeliveryMan object is invalid");
                    return BadRequest("Ivalid model object");
                }
                await deliveryManService.InsertAsync(newDeliveryMan);
                _logger.LogInformation("Inserted new deliveryMan");
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Transaction Failed! Something went wrong inside Post() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] DeliveryManRequest updatedDeliveryMan)
        {
            try
            {
                if (updatedDeliveryMan == null)
                {
                    _logger.LogError("DeliveryMan sent by client is null");
                    return BadRequest("DeliveryMan object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("DeliveryMan object is invalid");
                    return BadRequest("Ivalid model object");
                }
                var deliveryMan = await deliveryManService.GetByIdAsync(id);
                if (deliveryMan == null)
                {
                    _logger.LogError($"DeliveryMan with id: {id} was not found.");
                    return NotFound();
                }
                await deliveryManService.UpdateAsync(updatedDeliveryMan);
                _logger.LogInformation($"Updated deliveryMan {id}");
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
                var deliveryMan = await deliveryManService.GetByIdAsync(id);
                if (deliveryMan == null)
                {
                    _logger.LogError($"DeliveryMan with id: {id} was not found.");
                    return NotFound();
                }
                await deliveryManService.DeleteAsync(id);
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
