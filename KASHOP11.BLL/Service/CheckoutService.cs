using KASHOP11.DAL.DTO.Request;
using KASHOP11.DAL.DTO.Response;
using KASHOP11.DAL.Models;
using KASHOP11.DAL.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Stripe.Checkout;
using System.Collections.Generic;

namespace KASHOP11.BLL.Service
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartService _cartService;
        private readonly IProductRepository _productRepository;
        private readonly IEmailSender _EmailSender;



        public CheckoutService(
            ICartRepository cartRepository,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor, IOrderRepository orderRepository,
            ICartService cartService, IProductRepository productRepository, IEmailSender emailSender)
        {
            _cartRepository = cartRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _orderRepository = orderRepository;
            _cartService = cartService;
            _productRepository = productRepository;
            _EmailSender = emailSender;
        }

        public async Task<CheckoutResponse> ProcessCheckout(string userId, CheckoutRequest request)
        {
            // Check userId
            if (string.IsNullOrEmpty(userId))
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "User not found"
                };
            }

            // Get cart items
            var cartItems = await _cartRepository.GetAllAsync(
                c => c.UserId == userId,
                includes: new[]
                {
                    nameof(Cart.Product),
                    $"{nameof(Cart.Product)}.{nameof(Product.Translations)}"
                });

            if (cartItems == null || !cartItems.Any())
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "Cart is empty"
                };
            }

            
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "User not found"
                };
            }

            // Address Data
            var city = request.City ?? user.city;
            if (string.IsNullOrEmpty(city))
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "City is required"
                };
            }

            var street = request.Street ?? user.street;
            if (string.IsNullOrEmpty(street))
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "Street is required"
                };
            }

            var phoneNumber = request.PhoneNumber ?? user.PhoneNumber;
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "Phone number is required"
                };
            }

            // Check stock
            foreach (var item in cartItems)
            {
                if (item.Product == null)
                {
                    return new CheckoutResponse
                    {
                        Success = false,
                        Error = "Product not found"
                    };
                }

                if (item.count > item.Product.Quantity)
                {
                    return new CheckoutResponse
                    {
                        Success = false,
                        Error = "Not enough stock"
                    };
                }
            }
             
            var order = new Order()
            {
                UserId=userId,
                City=city,
                Street=street,
                PhoneNumber=phoneNumber,
                PaymentMethod=request.PaymentMethod,
                AmountPaid= cartItems.Sum(c => c.Product.Price * c.count),
                OrderItems=cartItems.Select(c=>new OrderItem
                {
                    ProductId=c.ProductId,
                    Quantity=c.count,
                    UnitPrice=c.Product.Price,
                    TotalPrice=c.Product.Price*c.count,
                }).ToList()

            };
            await _orderRepository.CreateAsync(order);
            // Cash payment
            if (request.PaymentMethod == PaymentMethodEnum.cash)
            {
                return new CheckoutResponse
                {
                    Success = true
                };
            }

            // Visa payment
            if (request.PaymentMethod == PaymentMethodEnum.visa)
            {
                var http = _httpContextAccessor.HttpContext;

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    Mode = "payment",
                    //SuccessUrl = $"{http.Request.Scheme}://{http.Request.Host}/checkouts/success?sessionId={{CHECKOUT_SESSION_ID}}",
                    //CancelUrl = $"{http.Request.Scheme}://{http.Request.Host}/checkouts/cancel",
                    SuccessUrl = $"{http.Request.Scheme}://{http.Request.Host}/api/Checkouts/success?sessionId={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = $"{http.Request.Scheme}://{http.Request.Host}/api/Checkouts/cancel",
                    LineItems = new List<SessionLineItemOptions>()
                };

                foreach (var item in cartItems)
                {
                    var productName = item.Product.Translations?
                        .FirstOrDefault(t => t.Language == "en")?.Name
                        ?? "Product";

                    options.LineItems.Add(
                        new SessionLineItemOptions
                        {
                            Quantity = item.count,
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                Currency = "usd",
                                UnitAmount = (long)(item.Product.Price * 100),
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = productName
                                }
                            }
                        });
                }

                var service = new SessionService();
                var session = service.Create(options);
                order.StripeSessionId = session.Id;
                await _orderRepository.UpdateAsync(order);
                return new CheckoutResponse
                {
                    Success = true,
                    StripeUrl = session.Url
                };
            }

            return new CheckoutResponse
            {
                Success = false,
                Error = "Invalid payment method"
            };
        }
   
       public async Task <CheckoutResponse> HandleSuccess(string sessionId)
        {
            var order = await _orderRepository.GetOne(o=>o.StripeSessionId==sessionId,
                includes: new[]
                {
                    nameof(Order.OrderItems),
                    $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}",
                     $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}.{nameof(Product.Translations)}"
                });
            order.OrderStatus = OrderStatusEnum.Paid;
            await _orderRepository.UpdateAsync(order);
            await _cartService.ClearCart(order.UserId);
            var user = await _userManager.FindByIdAsync(order.UserId);
           // var lowStockProducts = await _productRepository.DecreaseQuaintityAsync(order.OrderItems);
            await _EmailSender.SendEmailAsync(user.Email,"order confirmed","<h2>your order has been confirmed successfully</h2>");
            var isLowStock = await _productRepository.DecreaseQuaintityAsync(order.OrderItems);
            foreach(var item in isLowStock)
            {
                if (isLowStock != null)
                {
                    await _EmailSender.SendEmailAsync(
                             "jasminabualrub1999@gmail.com",
                             "low stock alert",
                             $"<h2>Product {item.Translations.FirstOrDefault(t=>t.Language=="en")} current quantity: {item.Quantity}</h2>"
                         );
                }
            }
           
          

            return new CheckoutResponse()
            {
                Success = true,
                OrderId = order.Id
            };
        }

    }
    }
