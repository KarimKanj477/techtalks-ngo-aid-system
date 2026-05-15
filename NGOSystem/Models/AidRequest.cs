using System.ComponentModel.DataAnnotations;

namespace NGOSystem.Models
{
    public class AidRequest
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Location { get; set; }

        public string? Status { get; set; }

        public string? UserEmail { get; set; }
    }
}