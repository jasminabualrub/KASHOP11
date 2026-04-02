using KASHOP11.DAL.DTO.Request;
using KASHOP11.DAL.DTO.Response;
using KASHOP11.DAL.Models;
using KASHOP11.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.BLL.Service
{
    public class ProductService:IProductService
    {
        private readonly IProductRepository _ProductRepository;
        private readonly IFileService _fileService;
        public ProductService(IProductRepository productRepository,IFileService fileService)
        {
            _ProductRepository = productRepository;
            _fileService = fileService;
        }
        public async Task CreateProduct(ProductRequest request)
        {
            var product = request.Adapt<Product>();
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
                    nameof(Product.createdBy)
                }
                );
            return products.Adapt<List<ProductResponse>>();
        }
    }
}
