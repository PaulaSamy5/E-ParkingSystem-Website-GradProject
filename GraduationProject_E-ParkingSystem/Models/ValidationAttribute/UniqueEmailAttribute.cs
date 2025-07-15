using GraduationProject_E_ParkingSystem.Models.Context;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Q3DotNetAssiut.Models
{
	public class UniqueEmailAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
		{
			// Get the value being validated
			string emailFromRequest = value?.ToString();

			// Access the DbContext from your service provider or create a new instance
			using (var context = new DBContext())
			{
				// Check for existing Email in the database
				var existingUser = context.Users // Change 'Users' to the correct DbSet name if needed
					.FirstOrDefault(u => u.Email == emailFromRequest);

				if (existingUser == null)
				{
					return ValidationResult.Success;
				}
				return new ValidationResult("Email already exists!!! :(");
			}
		}
	}
}
