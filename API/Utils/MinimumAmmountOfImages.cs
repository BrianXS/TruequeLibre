using System.ComponentModel.DataAnnotations;
using System.Linq;
using API.Resources.Incoming;

namespace API.Utils
{
    public class MinimumAmmountOfImages : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var request = (UpdateProductRequest) validationContext.ObjectInstance;

            if (!request.Pictures.Any())
            {
                return new ValidationResult("You must add at least an image to your product", 
                    new []{ nameof(UpdateProductRequest)});
            }
            
            return ValidationResult.Success;
        }
    }
}