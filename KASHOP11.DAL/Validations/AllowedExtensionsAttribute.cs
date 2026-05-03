using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.DAL.Validations
{
    public class AllowedExtensionsAttribute:ValidationAttribute
    {
        string[] _extensions = { ".png", ".webp" };
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!_extensions.Contains(extension))
                    return new ValidationResult($"Allowed Extensions:{string.Join(",", _extensions)}");
            }
                return ValidationResult.Success;
            
        }
    }
}
