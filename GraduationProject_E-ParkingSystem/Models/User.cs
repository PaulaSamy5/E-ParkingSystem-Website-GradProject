using Microsoft.AspNetCore.Mvc;
using Q3DotNetAssiut.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject_E_ParkingSystem.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
		[MinLength(3, ErrorMessage = "First Name should be correct")]
		[MaxLength(50, ErrorMessage = "You have limit of 50 characters")]
        //public string UserName { get; set; }
        public string FName { get; set; }
		[Required(ErrorMessage = "Last Name is required")]
		[MinLength(3, ErrorMessage = "Last Name should be correct")]
        [MaxLength(50, ErrorMessage = "You have limit of 50 characters")]
        public string LName { get; set; }
        [Required]
        public string Gender { get; set; }
        public bool IsAdmin { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "The minimum length is 8 characters.")]
        [PasswordValidation]
        public string Password { get; set; }
		[Required]
		[EmailAddress]
		//[UniqueEmail]
		public string Email { get; set; }
		//[Required]
		//[UniqueCarLicensePlate]
        //public string? CarLicensePlate { get; set; }

        public List<ParkingRecord>? ParkingRecords { get; set; }
        public List<Feedback>? feedbacks { get; set; }
    }
}
