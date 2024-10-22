using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TecnoMundo.Application.DTOs;
using TecnoMundo.Application.Interfaces;
using TecnoMundo.ProductAPI.Utils;

namespace TecnoMundo.ProductAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private IProductService _repository;

        public ProductController(IProductService repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(_repository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductVO>>> FindAll()
        {
            var products = await _repository.FindAll();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductVO>> Get(Guid id)
        {
            var product = await _repository.FindById(id);

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
        [Authorize(Roles = nameof(EnumRole.Admin))]
        public async Task<ActionResult<ProductVO>> Create(CreateProductVO vo)
        {
            if (vo is null)
                return BadRequest();

            try
            {
                var product = await _repository.Create(vo);
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
        [Authorize(Roles = nameof(EnumRole.Admin))]
        public async Task<ActionResult<ProductVO>> Update(ProductVO vo)
        {
            if (vo is null)
                return BadRequest();

            try
            {
                var product = await _repository.Update(vo);
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
        [Authorize(Roles = nameof(EnumRole.Admin))]
        public async Task<ActionResult> Delete(Guid id)
        {
            var status = await _repository.Delete(id);
            if (!status)
                return BadRequest();
            return Ok(status);
        }
    }
}
