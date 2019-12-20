using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class PictureInfo
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileLocation { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
        
        [NotMapped] 
        public string FilePath => $@"{FileLocation}/{FileName}";
    }
}