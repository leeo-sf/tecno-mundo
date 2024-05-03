using GeekShopping.Identity.Model;
using GeekShopping.Identity.Repository;
using GeekShopping.Identity.Response;
using GeekShopping.Identity.Service;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Identity.Controllers
{
    [ApiController]
    [Route("api/geek-shopping/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IDbRepository _repository;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AuthenticationController(IDbRepository repository,
            ITokenService tokenService,
            IConfiguration configuration)
        {
            _repository = repository;
            _tokenService = tokenService;
            _configuration = configuration;
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
                ExepiressInHours = Convert.ToInt32(_configuration.GetSection("AuthenticationSettings:ExpiressHours").Value)
            }));
        }
    }
}
