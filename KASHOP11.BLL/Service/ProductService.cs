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
                p=>p.status==EntityStatus.Active
                ,
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
        public async Task<bool> UpdateProduct(int id, ProductUpdateRequest req)
        {
            //var product = await _ProductRepository.GetOne(p=>p.Id==id);
            //if (product == null) return false;
            // req.Adapt<Product>();
            //var oldImg = product.MainImage;
            //if (req.MainImage != null) {
            //    _fileService.Delete(oldImg);
            //    product.MainImage = await _fileService.UploadAsync(req.MainImage);

            //}
            //else
            //{
            //    product.MainImage = oldImg;
            //}
            //return await _ProductRepository.UpdateAsync(product);
            //return true;

            var productDb = await _ProductRepository.GetOne(p => p.Id == id,
                new string[] {nameof(Product.Translations)});
            if (productDb == null) return false;

            var oldImg = productDb.MainImage;
            var product = req.Adapt<Product>();
            if (req.Translations != null)
            {
                foreach(var translationReq in req.Translations)
                {
                    var existing = product.Translations.FirstOrDefault(p => p.Language == translationReq.Language);
                    if (existing != null)
                    {
                        if (translationReq.Name != null)
                        {
                            existing.Name = translationReq.Name;
                        }
                        if (translationReq.Description != null)
                        {
                            existing.Description = translationReq.Description;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            product.Id = id;
            if (req.Price == null) product.Price = productDb.Price;
            if (req.Discount == null) product.Discount = productDb.Discount;
            if (req.Quantity == null) product.Quantity = productDb.Quantity;
            if (req.CategoryId == null) product.CategoryId = productDb.CategoryId;


            if (req.MainImage != null)
            {
                _fileService.Delete(oldImg);
                product.MainImage = await _fileService.UploadAsync(req.MainImage);
            }
            else
            {
                product.MainImage = oldImg;
            }

            return await _ProductRepository.UpdateAsync(product);

        }
    



    public async Task<bool> ToggleStatus(int id)
        {
            var product = await _ProductRepository.GetOne(p => p.Id == id);
            if (product == null) return false;
            product.status = product.status == EntityStatus.Active?EntityStatus.Inactive:EntityStatus.Active;
            return await _ProductRepository.UpdateAsync(product);
        }
}
}

