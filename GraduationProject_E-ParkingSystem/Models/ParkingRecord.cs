using GraduationProject_E_ParkingSystem.Models.Context;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject_E_ParkingSystem.Models
{
    public class ParkingRecord
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public int? UserId {  get; set; }
        [ForeignKey("ParkingSpot")]
        public int? ParkingSpotId {  get; set; }

        [ForeignKey("Payment")]
        public int? PaymentId { get; set; }
        public DateTime StartTime { get; set; }
        [Required]
        public int Duration {  get; set; }
        public DateTime EndTime { get; set; }
        [Required]
        public string? vehicleType {  get; set; }
        public string QRCode { get; set; } = string.Empty;
        [Required]
        public string? PlateNumber {  get; set; }
        public User User { get; set; }
        public ParkingSpots ParkingSpot { get; set; }
        public Payment payment { get; set; }

        //------------------------------------
        //public Payment PaymentMethod { get; set; }
        //------------------------------------
        public double? Cost { get; set; } = 0;
        public bool IsFinished { get; set; }
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
}
