using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TiklaGelsin.Application.DTOs;
using TiklaGelsin.Application.Interfaces;
using TiklaGelsin.Domain.Entities;

namespace TiklaGelsin.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICartQueryService _cartQueryService;

        public CartController(
            ICartRepository cartRepository,
            IProductRepository productRepository,
            IUserRepository userRepository,
            ICartQueryService cartQueryService)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _cartQueryService = cartQueryService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username)) return Unauthorized();

            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null) return Unauthorized();

            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null) return NotFound("Ürün bulunamadı.");

            var existingItem = await _cartRepository.GetItemAsync(user.Id, request.ProductId);
            
            if (existingItem != null)
            {
                existingItem.UpdateQuantity(existingItem.Quantity + request.Quantity);
                await _cartRepository.UpdateItemAsync(existingItem);
            }
            else
            {
                var newItem = new CartItem(Guid.NewGuid(), user.Id, product.Id, request.Quantity, product.Price);
                await _cartRepository.AddItemAsync(newItem);
            }

            return Ok(new { Message = "Ürün sepete eklendi." });
        }

        [HttpGet]
        public async Task<IActionResult> GetMyCart()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username)) return Unauthorized();

            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null) return Unauthorized();

            var cartItems = await _cartQueryService.GetCartItemsAsync(user.Id);
            return Ok(cartItems);
        }
    }
}
