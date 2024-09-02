using TecnoMundo.Identity.Commands;
using TecnoMundo.Identity.Data.ValueObjects;
using TecnoMundo.Identity.Model;
using TecnoMundo.Identity.Repository;
using TecnoMundo.Identity.Response;
using TecnoMundo.Identity.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TecnoMundo.IdentityAPI.Data.ValueObjects;

namespace TecnoMundo.Identity.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IDbRepository _repository;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IInsertUser _insert;

        public AuthenticationController(IDbRepository repository,
            ITokenService tokenService,
            IConfiguration configuration,
            IInsertUser insert)
        {
            _repository = repository;
            _tokenService = tokenService;
            _configuration = configuration;
            _insert = insert;
        }

        [HttpPost]
        [Route("Auth")]
        public async Task<ActionResult> GetToken([FromBody] AuthenticateVO userLogin)
        {
            var user = await _repository.ValidateUserEmailAndPassword(userLogin);

            if (user is null) return NotFound(Json("Invalid email or password"));

            var accessToken = _tokenService.GenerateToken(user);

            return Ok(new
            {
                AccessToken = accessToken,
                ExpiressInHours = Convert.ToInt32(_configuration.GetSection("AuthenticationSettings:ExpiressHours").Value)
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Create-Account")]
        public async Task<ActionResult> CreateAccount([FromBody] UserVO user)
        {
            try
            {
                await _insert.Execute(user);
                return Ok();
            }
            catch (Exception ex) when (ex is ApplicationException || ex is ArgumentException || ex is DbUpdateException)
            {
                return BadRequest(new
                {
                    Error = ex.Message
                });
            }
        }
    }
}
