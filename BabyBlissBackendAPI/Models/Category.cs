namespace BabyBlissBackendAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? CategoryName { get; set; }
        public virtual ICollection<Product> _Produts { get; set; }
    }
}
