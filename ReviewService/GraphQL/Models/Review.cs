using System.ComponentModel.DataAnnotations;

namespace GraphQL.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Text { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ShopId { get; set; }

        public User User { get; set; }

        public Shop Shop { get; set; }
    }
}
