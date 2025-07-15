using GraduationProject_E_ParkingSystem.Models.Context;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace YourNamespace // Change to your actual namespace
{
    public class UniqueSpotNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            // Get the value being validated
            string spotNumberFromRequest = value?.ToString();

            // Access the DbContext from your service provider or create a new instance
            using (var context = new DBContext())
            {
                // Check for existing SpotNumber in the database
                var existingSpot = context.ParkingSpots // Change 'ParkingSpots' to the correct DbSet name if needed
                    .FirstOrDefault(s => s.SlotName == spotNumberFromRequest);

                if (existingSpot == null)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult("Spot Number already exists");
            }
        }
    }
}
