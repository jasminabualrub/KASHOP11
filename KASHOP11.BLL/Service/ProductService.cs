using KASHOP11.DAL.DTO.Request;
using KASHOP11.DAL.DTO.Response;
using KASHOP11.DAL.Models;
using KASHOP11.DAL.Repository;
using Mapster;
using System.Linq.Expressions;

namespace KASHOP11.BLL.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _ProductRepository;
        private readonly IFileService _fileService;
        private readonly CurrentUserService _currentUserService;

        public ProductService(
            IProductRepository productRepository,
            IFileService fileService,
            CurrentUserService currentUserService)
        {
            _ProductRepository = productRepository;
            _fileService = fileService;
            _currentUserService = currentUserService;
        }

        public async Task CreateProduct(ProductRequest request)
        {
            var product = request.Adapt<Product>();
            product.createdById = _currentUserService.UserId;

            if (request.MainImage != null)
            {
                var imagePath = await _fileService.UploadAsync(request.MainImage);
                product.MainImage = imagePath;
            }

            if (request.SubImages != null && request.SubImages.Count > 0)
            {
                product.SubImages = new List<ProductImage>();

                foreach (var image in request.SubImages)
                {
                    if (image == null) continue;

                    var imagePath = await _fileService.UploadAsync(image);

                    if (string.IsNullOrEmpty(imagePath))
                        continue;

                    product.SubImages.Add(new ProductImage
                    {
                        ImagePath = imagePath
                    });
                }
            }

            await _ProductRepository.CreateAsync(product);
        }

        public async Task<List<ProductResponse>> GetAllProductsAsync()
        {
            var products = await _ProductRepository.GetAllAsync(
                p => p.status == EntityStatus.Active,
                new string[]
                {
                    nameof(Product.Translations),
                    nameof(Product.createdBy),
                    nameof(Product.SubImages)  
                });

            return products.Adapt<List<ProductResponse>>();
        }

        public async Task<ProductResponse?> GetProduct(Expression<Func<Product, bool>> filter)
        {
            var product = await _ProductRepository.GetOne(
                filter,
                new string[]
                {
                    nameof(Product.Translations),
                    nameof(Product.createdBy),
                    nameof(Product.SubImages)   
                });

            if (product == null)
                return null;

            return product.Adapt<ProductResponse>();
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _ProductRepository.GetOne(c => c.Id == id,
                new string[] { 
                    nameof(Product.SubImages)
                });

            if (product == null)
                return false;

            _fileService.Delete(product.MainImage);
           foreach(var image in product.SubImages)
            {
                _fileService.Delete(image.ImagePath);
            }

            return await _ProductRepository.DeleteAsync(product);
        }

        //public async Task<bool> UpdateProduct(int id, ProductUpdateRequest req)
        //{
        //    var productDb = await _ProductRepository.GetOne(
        //        p => p.Id == id,
        //        new string[]
        //        {
        //            nameof(Product.Translations),
        //            nameof(Product.SubImages)
        //        });

        //    if (productDb == null)
        //        return false;

        //    var oldImg = productDb.MainImage;

        //    var product = req.Adapt<Product>();

        //    if (req.Translations != null)
        //    {
        //        foreach (var translationReq in req.Translations)
        //        {
        //            var existing = product.Translations
        //                .FirstOrDefault(p => p.Language == translationReq.Language);

        //            if (existing != null)
        //            {
        //                if (translationReq.Name != null)
        //                    existing.Name = translationReq.Name;

        //                if (translationReq.Description != null)
        //                    existing.Description = translationReq.Description;
        //            }
        //        }
        //    }

        //    product.Id = id;

        //    if (req.Price == null)
        //        product.Price = productDb.Price;

        //    if (req.Discount == null)
        //        product.Discount = productDb.Discount;

        //    if (req.Quantity == null)
        //        product.Quantity = productDb.Quantity;

        //    if (req.CategoryId == null)
        //        product.CategoryId = productDb.CategoryId;

        //    if (req.MainImage != null)
        //    {
        //        _fileService.Delete(oldImg);
        //        product.MainImage = await _fileService.UploadAsync(req.MainImage);
        //    }
        //    else
        //    {
        //        product.MainImage = oldImg;
        //    }
        //    if(req.SubImages != null)
        //    {
        //        foreach(var img in product.SubImages)
        //        {
        //            _fileService.Delete(img.ImagePath);
        //        }
        //        product.SubImages.Clear();
        //        foreach(var image in req.SubImages)
        //        {

        //            var imagePath = await _fileService.UploadAsync(image);
        //            product.SubImages.Add (new ProductImage {ImagePath= imagePath });
        //        }
        //    }

        //    return await _ProductRepository.UpdateAsync(product);
        //}
        public async Task<bool> UpdateProduct(int id, ProductUpdateRequest req)
        {
            var productDb = await _ProductRepository.GetOne(
                p => p.Id == id,
                new string[]
                {
            nameof(Product.Translations),
            nameof(Product.SubImages)
                });

            if (productDb == null)
                return false;

            // Price
            if (req.Price != null)
                productDb.Price = req.Price.Value;

            // Discount
            if (req.Discount != null)
                productDb.Discount = req.Discount.Value;

            // Quantity
            if (req.Quantity != null)
                productDb.Quantity = req.Quantity.Value;

            // CategoryId
            if (req.CategoryId != null)
                productDb.CategoryId = req.CategoryId.Value;

            // Main Image
            if (req.MainImage != null)
            {
                _fileService.Delete(productDb.MainImage);

                productDb.MainImage =
                    await _fileService.UploadAsync(req.MainImage);
            }

            // Sub Images
            if (req.SubImages != null && req.SubImages.Count > 0)
            {
                foreach (var oldImage in productDb.SubImages)
                {
                    _fileService.Delete(oldImage.ImagePath);
                }

                productDb.SubImages.Clear();

                foreach (var image in req.SubImages)
                {
                    var imagePath = await _fileService.UploadAsync(image);

                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        productDb.SubImages.Add(new ProductImage
                        {
                            ImagePath = imagePath
                        });
                    }
                }
            }

            // Translations
            if (req.Translations != null)
            {
                foreach (var transReq in req.Translations)
                {
                    var existing = productDb.Translations
                        .FirstOrDefault(t => t.Language == transReq.Language);

                    if (existing != null)
                    {
                        if (!string.IsNullOrWhiteSpace(transReq.Name))
                            existing.Name = transReq.Name;

                        if (!string.IsNullOrWhiteSpace(transReq.Description))
                            existing.Description = transReq.Description;
                    }
                }
            }
//            if(req.NewImages != null)
//            {
//                foreach(var img in req.SubImages) {
//                    var imgpath = await _fileService.UploadAsync(img);
//                    Product.SubImage.Add(new ProductImage{ImagePath=imagePath});
//}
//            }

            return await _ProductRepository.UpdateAsync(productDb);
        }
        public async Task<bool> ToggleStatus(int id)
        {
            var product = await _ProductRepository.GetOne(p => p.Id == id);

            if (product == null)
                return false;

            product.status = product.status == EntityStatus.Active
                ? EntityStatus.Inactive
                : EntityStatus.Active;

            return await _ProductRepository.UpdateAsync(product);
        }
    }
}