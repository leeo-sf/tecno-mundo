﻿using GeekShopping.ProductAPI.Data.ValueObjects;
using GeekShopping.ProductAPI.Repository;
using GeekShopping.ProductAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace GeekShopping.ProductAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private IProductRepository _repository;

        public ProductController(
            IProductRepository repository)
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
        public async Task<ActionResult<ProductVO>> Get(long id)
        {
            var product = await _repository.FindById(id);

            if (product is null) return NotFound();

            return Ok(product);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<CategoryVO>>> FindAllCategories()
        {
            var categories = await _repository.FindAllCategories();

            if (categories is null) return NotFound();

            return Ok(categories);
        }

        [HttpPost]
        [Authorize(Roles = nameof(EnumRole.Admin))]
        public async Task<ActionResult<ProductVO>> Create(ProductVO vo)
        {
            if (vo is null) return BadRequest();

            try
            {
                var product = await _repository.Create(vo);
                return Ok(product);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is MySqlException mysqlError)
                {
                    if (mysqlError.Message.Contains("category_id"))
                    {
                        return BadRequest("Unregistered category");
                    }
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = nameof(EnumRole.Admin))]
        public async Task<ActionResult<ProductVO>> Update(ProductVO vo)
        {
            if (vo is null) return BadRequest();

            try
            {
                var product = await _repository.Update(vo);
                return Ok(product);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is MySqlException mysqlError)
                {
                    if (mysqlError.Message.Contains("category_id"))
                    {
                        return BadRequest("Unregistered category");
                    }
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(EnumRole.Admin))]
        public async Task<ActionResult> Delete(long id)
        {
            var status = await _repository.Delete(id);
            if (!status)return BadRequest();
            return Ok(status);
        }
    }
}
