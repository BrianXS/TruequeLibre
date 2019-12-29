using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using API.Utils;

namespace API.Resources.Incoming
{
    [MinimumAmmountOfImages]
    public class UpdateProductRequest
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        public List<PictureDto> Pictures { get; set; }
        
        public List<DetailDto> Details { get; set; }

        
        public class PictureDto
        {
            [Required]
            public string Name { get; set; }
            
            [Required]
            public string Content { get; set; }

            public string FileType => Content[0].Equals('/') ? ".jpg" : ".png";

            public override bool Equals(object? obj)
            {
                return ((PictureDto) obj).Content.Equals(Content);
            }

            public override int GetHashCode()
            {
                return (Name.GetHashCode() + Content.GetHashCode()) * 17;
            }
        }
        public class DetailDto
        {
            [Required]
            public string Name { get; set; }
            
            [Required]
            public string Description { get; set; }
        }
    }
}