using System.ComponentModel.DataAnnotations;

namespace GraphQL.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
