using GraduationProject_E_ParkingSystem.Models.Context;
using System.ComponentModel.DataAnnotations;
using YourNamespace;

namespace GraduationProject_E_ParkingSystem.Models.ViewModel;

public class UserSpotRecordDetailsViewModel
{
	public int? UserId { get; set; }
	public int? SpotId { get; set; }
	public string SpotNumber { get; set; }
    [Required]
    public string? vehicleType { get; set; }
    [Required]
    public string? PlateNumber { get; set; }
	public DateTime StartTime { get; set; }
	public DateTime EndTime { get; set; }
	public double? Cost { get; set; }
	[Required]
	public int Duration { get; set; } // Duration in hours
    public string QRCode { get; set; } = string.Empty;
    public string? PaymentMethod { get; set; }
    public double? TotalCostPaid { get; set; }
    public double CalculateCost()
	{
		DBContext context = new DBContext();
		TimeSpan duration = EndTime - StartTime;
		double totalMinutes = duration.TotalMinutes;
		var parkingSetting = context.parkingSettings.FirstOrDefault(i => i.id == 1);

		double ratePerHour = parkingSetting.PricePerHour;
		var totalCost = (totalMinutes / 60) * ratePerHour;
		return totalCost;
	}
}
