using KASHOP11.DAL.Data;
using KASHOP11.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.DAL.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Product>?> DecreaseQuaintityAsync(List<OrderItem> orderItems)
        {
            var productIds = orderItems.Select(i=>i.ProductId).ToList();
            var products = await GetAllAsync(p=>productIds.Contains(p.Id));
            foreach(var product in products)
            {
                var item = orderItems.FirstOrDefault(p=>p.ProductId==product.Id);
                product.Quantity -= item.Quantity;
            }
          
            await UpdateRangeAsync(products);
            return products.Where(p => p.Quantity < 5).ToList();

            
        }
    }
}
