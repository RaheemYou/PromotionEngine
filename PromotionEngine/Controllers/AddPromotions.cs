using Microsoft.AspNetCore.Mvc;
using PromotionEngine.Interfaces;
using PromotionEngine.Models.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionEngine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddPromotions : ControllerBase
    {
        private ICartService cartService { get; set; }

        public AddPromotions(ICartService cartService)
        {
            this.cartService = cartService;
        }

        [HttpPost]
        public CartModel PostCart(CartModel cart)
        {
            return this.cartService.CalculateTotalPromotionPrice(cart);
        }
    }
}
