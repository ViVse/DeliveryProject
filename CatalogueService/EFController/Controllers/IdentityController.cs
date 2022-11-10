using BLL.DTO.Requests;
using BLL.DTO.Responses;
using BLL.Interfaces.Services;
using BLL.Validation.Requests;
using DAL.Exceptions;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;

namespace EFController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService identityService;

        public IdentityController(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        [HttpPost("signin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JwtResponse>> SignInAsync([FromBody] UserSignInRequest request)
        {
            try
            {
                ValidationResult results = new UserSignInRequestValidator().Validate(request);
                if (!results.IsValid)
                    return BadRequest(results.ToString("\n"));

                var token = await identityService.SignInAsync(request);
                return Ok(token);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(new { e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { e.Message });
            }
        }

        [HttpPost("signup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JwtResponse>> SignUpAsync([FromBody] UserSignUpRequest request)
        {
            try
            {
                ValidationResult results = new UserSignUpRequestValidator().Validate(request);
                if (!results.IsValid)
                    return BadRequest(results.ToString("\n"));

                var res = await identityService.SignUpAsync(request);
                return Ok(res);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { e.Message });
            }
        }
    }
}
