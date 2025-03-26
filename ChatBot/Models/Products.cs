using System.ComponentModel.DataAnnotations;

namespace ChatBot.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Category { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public string? GeneratedDescription { get; set; }
    }
}
