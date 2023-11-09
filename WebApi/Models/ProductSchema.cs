using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class ProductSchema
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? SupplierArticleNumber { get; set; }
        public string? Description { get; set; } 
    }
}
