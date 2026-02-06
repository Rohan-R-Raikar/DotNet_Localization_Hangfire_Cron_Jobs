using System.ComponentModel.DataAnnotations;

namespace LocalizationProject.Models
{
    public class PaymentByLinkModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ShopAccountId { get; set; }

        [Required]
        public int MerchantAccountId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(1.00, double.MaxValue, ErrorMessage = "Amount must be at least 1.00")]
        [DataType(DataType.Currency)]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [StringLength(25, ErrorMessage = "Reference Number cannot exceed 25 characters")]
        [Display(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [Display(Name = "URLExpiry")]
        public string URLExpiry { get; set; }

        [Display(Name = "ExpiryType")]
        public string ExpiryType { get; set; }
    }
}
