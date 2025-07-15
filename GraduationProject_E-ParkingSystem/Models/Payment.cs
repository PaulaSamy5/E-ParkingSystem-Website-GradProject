namespace GraduationProject_E_ParkingSystem.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string? PaymentMethod { get; set; }
        public double? TotalCostPaid { get; set; }
        public ParkingRecord ParkingRecord { get; set; }
    }
}
