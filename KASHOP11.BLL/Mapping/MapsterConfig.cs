using KASHOP11.DAL.DTO.Request;
using KASHOP11.DAL.DTO.Response;
using KASHOP11.DAL.Models;
using Mapster;
using Microsoft.Azure.Amqp.Framing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.BLL.Mapping
{
   public static class MapsterConfig
    {
        public static void MapsterConfigRegister() {
            //            TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
            //.Map(dest => dest.categoryId, src => src.Id)
            //.Map(dest => dest.User, src => src.createdBy != null ? src.createdBy.UserName : "Unknown")
            //.Map(dest => dest.Translations, src => src.Translations.Where(t => t.Language == CultureInfo
            //.CurrentCulture.Name).Select(t => t.Name).FirstOrDefault());

            TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
              .Map(destniation => destniation.categoryId, source => source.Id)
              .Map(destniation => destniation.User, source => source.createdBy.UserName)
              .Map(dest => dest.Name, source => source.Translations
              .Where(t => t.Language == CultureInfo.CurrentCulture.Name).Select(t => t.Name).FirstOrDefault());

            TypeAdapterConfig<Product, ProductResponse>.NewConfig()

             .Map(dest => dest.UserCreated, src =>
                  src.createdBy != null ? src.createdBy.UserName : "Unknown")


             .Map(dest => dest.Name, src =>
               src.Translations == null
                             ? "No Name"
                         : src.Translations
                  .Where(t => t.Language == CultureInfo.CurrentCulture.Name)
            .Select(t => t.Name)
            .FirstOrDefault() ?? "No Name"

)
.Map(dest=>dest.MainImage,src=>$" https://localhost:7245/images/{src.MainImage}")
;
            TypeAdapterConfig<ProductUpdateRequest, Product>.NewConfig().IgnoreNullValues(true);


            TypeAdapterConfig<Brand, BrandResponse>.NewConfig()
                 .Map(dest => dest.Id, src => src.Id)
                 .Map(dest => dest.Logo, src => $"https://localhost:7245/images/{src.Logo}")
                 .Map(dest => dest.Name, src => src.Translations
                     .Where(t => t.Language == CultureInfo.CurrentCulture.Name)
                     .Select(t => t.Name)
                     .FirstOrDefault() ?? "No Name");

           
            TypeAdapterConfig<BrandRequest, Brand>.NewConfig()
                .Map(dest => dest.Translations,
                     src => src.Translations.Select(t => new BrandTranslation
                     {
                         Name = t.Name,
                         Language = t.Language
                     }).ToList());






        }
    }
}
