using KASHOP11.DAL.DTO.Request;
using KASHOP11.DAL.DTO.Response;
using KASHOP11.DAL.Models;
using KASHOP11.DAL.Repository;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.BLL.Service
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepoitory;
        private readonly IProductRepository _productRepository;

       

        public CartService(ICartRepository cartRepoitory, IProductRepository productRepository)
        {
            _cartRepoitory = cartRepoitory;
            _productRepository = productRepository;
        }
        public async Task <bool> AddToCart(AddToCartRequest request, string UserId)
        {
            var product = await _productRepository.GetOne(p=>p.Id==request.ProductId);
            if (product is null) return false;
            var existingItem = await _cartRepoitory.GetOne(
                c => c.ProductId == request.ProductId && c.UserId == UserId);
            var currentcount = existingItem?.count ?? 0;
            var newcount = currentcount + request.count;
            if (newcount > product.Quantity) return false;

            if (existingItem != null)
            {
                existingItem.count =newcount;
                await _cartRepoitory.UpdateAsync(existingItem);
            }
            else
            {
                var cartitem = request.Adapt<Cart>();
                cartitem.UserId = UserId;

                await _cartRepoitory.CreateAsync(cartitem);
            }
            return true;
        }

        public async Task<bool> ClearCart(string userId)
        {
            var items = await _cartRepoitory.GetAllAsync(
                c=>c.UserId == userId
                );
            if (!items.Any()) return false;
             await _cartRepoitory.DeleteRangeAsync(items);
            return true;
        }

        //public async Task<List<CartResponse>> GetCart(string UserId)
        //{
        //    var items = await _cartRepoitory.GetAllAsync(
        //       c=>c.UserId == UserId,
        //       new string[] {nameof(Cart.Product),
        //       $"{nameof(Cart.Product)}.{nameof(Product.Translations)}"



        //       });
        //    return items.Adapt<List<CartResponse>>();
        //}
        public async Task<List<CartResponse>> GetCart(string UserId)
        {
            var items = await _cartRepoitory.GetUserCartWithProduct(UserId);

            return items.Select(c => new CartResponse
            {
                ProductId = c.ProductId,

                ProductName = c.Product?.Translations?
                    .Where(t => t.Language == CultureInfo.CurrentCulture.Name)
                    .Select(t => t.Name)
                    .FirstOrDefault() ?? "No Name",

                ProductImage = $"https://localhost:7245/images/{c.Product?.MainImage}",

                Price = c.Product?.Price ?? 0,

                Discount = c.Product?.Discount ?? 0,

                Count = c.count
            }).ToList();
        }
        public async Task<bool> RemoveItem(int productId, string userId)
        {
            var item = await _cartRepoitory.GetOne(
                c=>c.ProductId ==productId && c.UserId==userId
                );
            if (item == null) return false;
            return await _cartRepoitory.DeleteAsync(item);

        }

        public async Task<bool> UpdateQuaintity(int productId, int count, string userId)
        {
            var item = await _cartRepoitory.GetOne(
               c => c.ProductId == productId && c.UserId == userId
               );
            if (item is null) return false;
            var product = await _productRepository.GetOne(p => p.Id == productId);
            if (count > product.Quantity) return false;
            item.count = count;
            return await _cartRepoitory.UpdateAsync(item);

        }
    }
    
}
