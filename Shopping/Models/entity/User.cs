using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace Shopping.Models.entity
{
    public class User : BaseModel
    {
        public User()
        {
            ProductLists = new HashSet<ProductList>();
        }
        [MaxLength(50)] public required string Email { get; set; }

        [MaxLength(16)] public required byte[] SaltValue { get; set; }

        [MaxLength(50)] public required string Name { get; set; }
        [MaxLength(50)] public required string LastName { get; set; }

        [MaxLength(50)] public required string PasswordHash { get; set; }
        [MaxLength(50)] public string Role { get; set; } = "User";
        public ICollection<ProductList> ProductLists { get; set; }
    }
}
