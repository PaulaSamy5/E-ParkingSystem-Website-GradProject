using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject_E_ParkingSystem.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        [Required]
        [MinLength(10, ErrorMessage = "The Feedback is too small...")]
        public string? Message { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
