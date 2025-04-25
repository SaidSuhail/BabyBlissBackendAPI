using System.ComponentModel.DataAnnotations;

namespace BabyBlissBackendAPI.Dto
{
    public class ProductViewDto
    {
        public int ProductId { get; set; }

        [Required]
        public string? ProductName { get; set; }

        [Required]
        public string? ProductDescription { get; set; }

        [Required]
        public decimal? ProductPrice { get; set; }
        [Required]
        public decimal? OfferPrize { get; set; }

        [Required]
        public decimal? Rating { get; set; }

        [Required]
        [Url]
        public string? ImageUrl { get; set; }

        [Required]
        public int? StockId { get; set; }
    }
}
