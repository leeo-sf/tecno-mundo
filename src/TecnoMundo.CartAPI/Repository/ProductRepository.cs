﻿using System.Net;
using System.Text.Json;
using GeekShopping.CartAPI.Data.ValueObjects;

namespace GeekShopping.CartAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly HttpClient _httpClient;

        public ProductRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProductVO> GetProductById(Guid productId)
        {
            var response = await _httpClient.GetAsync($"/api/v1/Product/{productId}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
                return new ProductVO();
            return JsonSerializer.Deserialize<ProductVO>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
        }
    }
}
