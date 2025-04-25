using System.ComponentModel.DataAnnotations;

namespace BabyBlissBackendAPI.Dto
{
    public class WishListDto
    {
        [Required]
        public int? ProductId { get; set; }
        [Required]
        public int? UserId { get; set; }
    }
}
