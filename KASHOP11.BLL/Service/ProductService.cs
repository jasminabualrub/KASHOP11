using KASHOP11.DAL.DTO.Request;
using KASHOP11.DAL.DTO.Response;
using KASHOP11.DAL.Models;
using KASHOP11.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.BLL.Service
{
    public class ProductService:IProductService
    {
        private readonly IProductRepository _ProductRepository;
        private readonly IFileService _fileService;
        private readonly CurrentUserService _currentUserService;
        public ProductService(IProductRepository productRepository,IFileService fileService, CurrentUserService currentUserService)
        {
            _ProductRepository = productRepository;
            _fileService = fileService;
            _currentUserService = currentUserService;
        }
        public async Task CreateProduct(ProductRequest request)
        {
            var product = request.Adapt<Product>();
            product.createdById = _currentUserService.UserId;
            if (request.MainImage!=null)
            {
                var imagePath = await _fileService.UploadAsync(request.MainImage);
                product.MainImage = imagePath;
                
            }
            await _ProductRepository.CreateAsync(product);
        }
        public async Task<List<ProductResponse>> GetAllProductsAsync()
        {
            var products = await _ProductRepository.GetAllAsync(

                new string[]
                {
                    nameof(Product.Translations),
                    nameof(Product.createdBy),
                     
                }
                );
            return products.Adapt<List<ProductResponse>>();
        }


        public async Task<ProductResponse?> GetProduct(Expression<Func<Product,bool>>filter) 
        {
            var product = await _ProductRepository.GetOne(filter, new string[]
                {
                    nameof(Product.Translations),
                    nameof(Product.createdBy),

                });
                if(product ==null)return null;
            return product.Adapt<ProductResponse>();
        }
        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _ProductRepository.GetOne(c=>c.Id==id);
            if (product == null) return false;
            _fileService.Delete(product.MainImage);
            return await _ProductRepository.DeleteAsync(product);

        }

    }
    
}
