using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace endavaRestApi.Data
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public string ProductBrand { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        [Required]
        public decimal Price { get; set; }
        public string ProductSize { get; set; }
        public string ProductDescription { get; set; }
        public decimal Weight { get; set; }
        [Required]
        public int ProductQuantity { get; set; }
        public string Color { get; set; }

        public decimal TotalPrice { get; set; }
        [Required]
        public Guid ProductGuidId { get; set; }

    }
}
