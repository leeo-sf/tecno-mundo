using System.Net;
using System.Text.Json;
using TecnoMundo.Application.DTOs;
using TecnoMundo.Domain.Entities;

namespace TecnoMundo.CartAPI.Service
{
    public class ServiceProduct : IServiceProduct
    {
        private readonly HttpClient _httpClient;

        public ServiceProduct(HttpClient httpClient)
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
                ) ?? new ProductVO();
        }

        public async Task<CartVO> GetProductsByListCart(CartVO vo)
        {
            foreach (var cartItem in vo.CartDetails)
            {
                var response = await _httpClient.GetAsync($"/api/v1/Product/{cartItem.ProductId}");
                var content = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new ApplicationException(
                        "It was not possible to obtain some data. try again later"
                    );
                var deserializeProduct = JsonSerializer.Deserialize<ProductVO>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
                cartItem.Product = deserializeProduct;
            }

            return vo;
        }
    }
}
