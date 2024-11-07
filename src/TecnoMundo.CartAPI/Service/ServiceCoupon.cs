using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using TecnoMundo.Application.DTOs;

namespace TecnoMundo.CartAPI.Service
{
    public class ServiceCoupon : IServiceCoupon
    {
        private readonly HttpClient _client;

        public ServiceCoupon(HttpClient client)
        {
            _client = client;
        }

        public async Task<CouponVO> GetCouponByCouponCode(string couponCode, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token
            );
            var response = await _client.GetAsync($"/api/v1/Coupon/{couponCode}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
                return new CouponVO();
            return JsonSerializer.Deserialize<CouponVO>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                ) ?? new CouponVO();
        }
    }
}
