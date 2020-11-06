using System.ComponentModel.DataAnnotations;

namespace SportStorage.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Please enter product name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter description")]
        public string Description { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter positive price")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Please specify a category")]
        public string Category { get; set; }
    }
}