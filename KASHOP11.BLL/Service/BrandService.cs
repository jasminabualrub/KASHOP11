using KASHOP11.DAL.DTO.Request;
using KASHOP11.DAL.DTO.Response;
using KASHOP11.DAL.Models;
using KASHOP11.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KASHOP11.BLL.Service
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IFileService _fileService;
        private readonly CurrentUserService _currentUserService;

        public BrandService(IBrandRepository brandRepository, IFileService fileService, CurrentUserService currentUserService)
        {
            _brandRepository = brandRepository;
            _fileService = fileService;
            _currentUserService = currentUserService;
        }

       
        public async Task CreateBrand(BrandRequest request)
        {
            var brand = request.Adapt<Brand>();
           

            if (request.Logo != null)
            {
                var logoPath = await _fileService.UploadAsync(request.Logo);
                brand.Logo = logoPath;
            }

            await _brandRepository.CreateAsync(brand);
        }

   
        public async Task<List<BrandResponse>> GetAllBrandsAsync()
        {
            var brands = await _brandRepository.GetAllAsync(
                b => true, 
                new string[]
                {
                    nameof(Brand.Translations)
                   
                }
            );

            return brands.Adapt<List<BrandResponse>>();
        }

       
        public async Task<BrandResponse?> GetBrand(Expression<Func<Brand, bool>> filter)
        {
            var brand = await _brandRepository.GetOne(filter, new string[]
            {
                nameof(Brand.Translations)
            });

            if (brand == null) return null;

            return brand.Adapt<BrandResponse>();
        }

      
        public async Task<bool> DeleteBrand(int id)
        {
            var brand = await _brandRepository.GetOne(b => b.Id == id, new string[]
            {
                nameof(Brand.Translations)
            });

            if (brand == null) return false;

            
            if (!string.IsNullOrEmpty(brand.Logo))
            {
                _fileService.Delete(brand.Logo);
            }

            return await _brandRepository.DeleteAsync(brand);
        }
    }
}