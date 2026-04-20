using KASHOP11.DAL.DTO.Request;
using KASHOP11.DAL.DTO.Response;
using KASHOP11.DAL.Models;
using KASHOP11.DAL.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KASHOP11.BLL.Service
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CheckoutService(
            ICartRepository cartRepository,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _cartRepository = cartRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CheckoutResponse> ProcessCheckout(string userId, CheckoutRequest request)
        {
            var cartItems = await _cartRepository.GetAllAsync(
                c => c.UserId == userId,
                includes: new[] { nameof(Cart.Product) }
            );

            if (!cartItems.Any())
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "cart is empty"
                };
            }

            var user = await _userManager.FindByIdAsync(userId);

            var city = request.City ?? user.city;
            if (city == null)
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "city is required"
                };
            }

            var street = request.Street ?? user.street;
            if (street == null)
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "street is required"
                };
            }

            var phoneNumber = request.PhoneNumber ?? user.PhoneNumber;
            if (phoneNumber == null)
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "phone number is required"
                };
            }

            foreach (var item in cartItems)
            {
                if (item.count > item.Product.Quantity)
                {
                    return new CheckoutResponse
                    {
                        Success = false,
                        Error = "doesn't have enough stock"
                    };
                }
            }

            if (request.PaymentMethod == PaymentMethodEnum.cash)
            {
                return new CheckoutResponse
                {
                    Success = true
                };
            }

            if (request.PaymentMethod == PaymentMethodEnum.visa)
            {
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    Mode = "payment",
                    SuccessUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/checkout/success",
                    CancelUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/checkout/cancel",
                    LineItems = new List<SessionLineItemOptions>()
                };

                foreach (var item in cartItems)
                {
                    options.LineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Translations
                                    .FirstOrDefault(t => t.Language == "en")?.Name ?? "Product"
                            },
                            UnitAmount = (long)(item.Product.Price * 100)
                        },
                        Quantity = item.count
                    });
                }

                var service = new SessionService();
                var session = await service.CreateAsync(options);

                return new CheckoutResponse
                {
                    Success = true,
                    StripeUrl = session.Url
                };
            }

            return new CheckoutResponse
            {
                Success = false,
                Error = "invalid payment method"
            };
        }
    }
}