using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Resources.Incoming
{
    public class AddProductRequest
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        //[MinLength(1)]
        public List<PictureDto> Pictures { get; set; }
        
        public List<DetailDto> Details { get; set; }

        public class DetailDto
        {
            [Required]
            public string Name { get; set; }
            
            [Required]
            public string Description { get; set; }
        }
        
        public class PictureDto
        {
            [Required]
            public string Name { get; set; }
            
            [Required]
            public string Content { get; set; }

            public string FileType => Content[0].Equals('/') ? ".jpg" : ".png";
        }
    }
}