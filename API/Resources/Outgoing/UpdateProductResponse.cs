using System.Collections.Generic;

namespace API.Resources.Outgoing
{
    public class UpdateProductResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public List<PictureDto> Pictures { get; set; }
        public List<DetailDto> Details { get; set; }

        public class PictureDto
        {
            public string Name { get; set; }
            public string Content { get; set; }
        }
        
        public class DetailDto
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}