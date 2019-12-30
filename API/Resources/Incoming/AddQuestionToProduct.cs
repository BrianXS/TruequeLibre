using System.ComponentModel.DataAnnotations;

namespace API.Resources.Incoming
{
    public class AddQuestionToProduct
    {
        [Required]
        public string Body { get; set; }
    }
}