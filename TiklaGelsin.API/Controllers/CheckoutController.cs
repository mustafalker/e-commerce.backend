using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TiklaGelsin.Application.DTOs;
using TiklaGelsin.Application.Interfaces;
using TiklaGelsin.Application.Services;

namespace TiklaGelsin.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CheckoutController : ControllerBase
    {
        private readonly CheckoutService _checkoutService;
        private readonly IOrderQueryService _orderQueryService;

        public CheckoutController(CheckoutService checkoutService, IOrderQueryService orderQueryService)
        {
            _checkoutService = checkoutService;
            _orderQueryService = orderQueryService;
        }

        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request)
        {
            var username = User.Identity?.Name ?? "Anonymous";
            var result = await _checkoutService.ProcessCheckoutAsync(request, username);

            if (result.Success)
            {
                return Ok(new { Message = "Sipariş başarıyla oluşturuldu ve ödendi.", OrderId = result.OrderId });
            }

            return BadRequest(new { Error = result.ErrorMessage });
        }

        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var orders = await _orderQueryService.GetAllOrdersAsync();
            return Ok(orders);
        }
    }
}
