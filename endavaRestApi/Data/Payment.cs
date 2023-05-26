using System.ComponentModel.DataAnnotations;
namespace endavaRestApi.Data
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ProductId { get; set; }
        public string PaymentMethod { get; set; }

        public User User { get; set; } = null!;

        public Product Product { get; set; } = null!;

        public decimal PricePaid { get; set; }
    }
}
