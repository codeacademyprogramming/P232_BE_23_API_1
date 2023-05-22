using System.ComponentModel.DataAnnotations;

namespace AdminUI.ViewModels
{
    public class ProductCreateViewModel
    {
        [Required]
        [MaxLength(30)]
        [MinLength(2)]
        public string Name { get; set; }
        [Required]
        public decimal CostPrice { get; set; }
        [Required]
        public decimal SalePrice { get; set; }
        [Required]
        public decimal DiscountPercent { get; set; }
        [Required]
        public int BrandId { get; set; }
    }
}
