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
            TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
.Map(dest => dest.categoryId, src => src.Id)
.Map(dest => dest.User, src => src.createdBy != null ? src.createdBy.UserName : "Unknown")
.Map(dest => dest.Translations, src => src.Translations.Where(t => t.Language == CultureInfo
.CurrentCulture.Name).Select(t => t.Name).FirstOrDefault());


            //            TypeAdapterConfig<Product, ProductResponse>.NewConfig()

            //.Map(dest => dest.UserCreated, src => src.createdBy.UserName )
            //.Map(dest => dest.Name, src => src.Translations.Where(t => t.Language == CultureInfo
            //.CurrentCulture.Name).Select(t => t.Name).FirstOrDefault());
            TypeAdapterConfig<Product, ProductResponse>.NewConfig()

 .Map(dest => dest.UserCreated, src =>
     src.createdBy != null ? src.createdBy.UserName : "Unknown"
 )

//.Map(dest => dest.Name, src =>
//      src.Translations != null && src.Translations.Any()
//          ? src.Translations.First().Name
//          : "No Name"
//  )
.Map(dest => dest.Name, src =>
    src.Translations == null
        ? "No Name"
        : src.Translations
            .Where(t => t.Language == CultureInfo.CurrentCulture.Name)
            .Select(t => t.Name)
            .FirstOrDefault() ?? "No Name"

)
.Map(dest=>dest.MainImage,src=>$" https://localhost:7245/images/{src.MainImage}");
;






        }
    }
}
