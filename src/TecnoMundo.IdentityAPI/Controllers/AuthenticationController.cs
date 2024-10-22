using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TecnoMundo.Application.DTOs;
using TecnoMundo.Application.Interfaces;
using TecnoMundo.Identity.Service;

namespace TecnoMundo.Identity.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IIdentityService _repository;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AuthenticationController(
            IIdentityService repository,
            ITokenService tokenService,
            IConfiguration configuration
        )
        {
            _repository = repository;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Auth")]
        public async Task<ActionResult> GetToken([FromBody] AuthenticateVO vo)
        {
            var user = await _repository.ValidateUserEmailAndPassword(vo.UserEmail, vo.Password);

            if (user?.Id == Guid.Empty)
                return BadRequest(new { errorMessage = "Invalid email or password" });

            var accessToken = _tokenService.GenerateToken(user);

            return Ok(
                new
                {
                    AccessToken = accessToken,
                    ExpiressInHours = Convert.ToInt32(
                        _configuration.GetSection("AuthenticationSettings:ExpiressHours").Value
                    )
                }
            );
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Create-Account")]
        public async Task<ActionResult> CreateAccount([FromBody] UserVO user)
        {
            try
            {
                await _repository.Create(user);
                return Ok();
            }
            catch (Exception ex)
                when (ex is ApplicationException
                    || ex is ArgumentException
                    || ex is DbUpdateException
                )
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
