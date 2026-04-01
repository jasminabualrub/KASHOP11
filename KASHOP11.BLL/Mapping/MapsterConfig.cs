using KASHOP11.DAL.DTO.Response;
using KASHOP11.DAL.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.BLL.Mapping
{
   public static class MapsterConfig
    {
        public static void MapsterConfigRegister() {
            TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
                .Map(dest => dest.categoryId, source => source.Id)
              //  .Map(dest=>dest.User,source=>source.createdBy.UserName)
                 .Map(dest => dest.Name, source => source.Translations.Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                 .Select(t=>t.Name).FirstOrDefault());
            // 



        }
    }
}
