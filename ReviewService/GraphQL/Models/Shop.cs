using System.ComponentModel.DataAnnotations;

namespace GraphQL.Models
{
    public class Shop
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
