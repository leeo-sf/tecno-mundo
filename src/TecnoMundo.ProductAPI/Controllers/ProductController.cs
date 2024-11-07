using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TecnoMundo.Application.DTOs;
using TecnoMundo.Application.Interfaces;
using TecnoMundo.Domain.Enums;

namespace TecnoMundo.ProductAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private IProductService _repository;
        private readonly IConfiguration _configuration;
        private readonly DistributedCacheEntryOptions _options;
        private readonly string _keyCache;

        public ProductController(IProductService repository, IConfiguration configuration)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(_repository));
            _configuration = configuration;
            _keyCache =
                _configuration.GetSection("Redis").GetSection("Key_Cache_Products").Value
                ?? "products";
            _options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(
                    double.Parse(
                        _configuration.GetSection("Redis").GetSection("Absolute_Expire").Value
                            ?? "3600"
                    )
                ),
                SlidingExpiration = TimeSpan.FromSeconds(
                    double.Parse(
                        _configuration.GetSection("Redis").GetSection("Sliding_Expire").Value
                            ?? "600"
                    )
                )
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductVO>>> FindAll()
        {
            var products = await _repository.FindAll(_keyCache, _options);

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductVO>> Get(Guid id)
        {
            var keyCache = $"product-{id}";
            var product = await _repository.FindById(id, keyCache, _options);

            if (product is null)
                return NotFound();

            return Ok(product);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<CategoryVO>>> FindAllCategories()
        {
            var categories = await _repository.FindAllCategories();

            if (categories is null)
                return NotFound();

            return Ok(categories);
        }

        [HttpGet("by-category/{idCategory}")]
        public async Task<ActionResult<IEnumerable<ProductVO>>> FindProductsByCategoryId(
            Guid idCategory
        )
        {
            var products = await _repository.FindProductsByCategoryId(idCategory);

            if (products == null)
                return NotFound();

            return Ok(products);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<ProductVO>>> ProductFilter(
            string? name,
            decimal? priceOf,
            decimal? priceUpTo
        )
        {
            var product = await _repository.ProductFilter(
                string.IsNullOrEmpty(name) ? "" : name,
                !priceOf.HasValue ? 1 : priceOf,
                !priceUpTo.HasValue ? 50000 : priceUpTo
            );

            if (product.IsNullOrEmpty())
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<ActionResult<ProductVO>> Create(CreateProductVO vo)
        {
            if (vo is null)
                return BadRequest();

            try
            {
                var product = await _repository.Create(vo, _keyCache, _options);
                return Ok(product);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("category_id"))
                {
                    return BadRequest("Unregistered category");
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<ActionResult<ProductVO>> Update(ProductVO vo)
        {
            if (vo is null)
                return BadRequest();

            try
            {
                var product = await _repository.Update(vo, _keyCache, _options);
                return Ok(product);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("category_id"))
                {
                    return BadRequest("Unregistered category");
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<ActionResult> Delete(Guid id)
        {
            var status = await _repository.Delete(id, _keyCache, _options);
            if (!status)
                return BadRequest(
                    new { errorMessage = "Product not found or unable to be removed" }
                );
            return Ok(status);
        }
    }
}
