using GeekShopping.Identity.Commands;
using GeekShopping.Identity.Data.ValueObjects;
using GeekShopping.Identity.Model;
using GeekShopping.Identity.Repository;
using GeekShopping.Identity.Response;
using GeekShopping.Identity.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Identity.Controllers
{
    [ApiController]
    [Route("api/geek-shopping/[controller]")]
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
        [Route("auth")]
        public async Task<ActionResult> GetToken([FromBody] UserLogin userLogin)
        {
            var user = await _repository.ValidateUserEmailAndPassword(userLogin);

            if (user is null) return NotFound(Json("Invalid email or password"));

            var accessToken = _tokenService.GenerateToken(user);

            return Ok(Json(new InformacoesToken
            {
                AcessToken = accessToken,
                ExpiressInHours = Convert.ToInt32(_configuration.GetSection("AuthenticationSettings:ExpiressHours").Value)
            }));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("create-account")]
        public async Task<ActionResult> CreateAccount([FromBody] UserVO user)
        {
            try
            {
                await _insert.Execute(user);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(Json(ex.Message));
            }
            catch (ApplicationException ex)
            {
                return BadRequest(Json(ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(Json(ex.Message));
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(Json(ex.Message));
            }
        }
    }
}
