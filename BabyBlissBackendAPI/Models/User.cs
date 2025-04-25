using System.ComponentModel.DataAnnotations;

namespace BabyBlissBackendAPI.Models
{
    public class User
    {

        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public bool IsBlocked { get; set; }
        public UserRole Role { get; set; } = UserRole.User;// Change from string to UserRole enum
        public enum UserRole
        {
            User = 1,  // Regular user
            Admin = 2  // Admin user
        }
        public virtual Cart _Cart { get; set; }
        public virtual ICollection<WishList> _WishLists { get; set; }
        public virtual ICollection<Order> _Orders { get; set; }
        public virtual ICollection<UserAddress> _UserAddresses { get; set; }

    }
}
