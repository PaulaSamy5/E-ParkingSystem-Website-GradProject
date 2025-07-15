using System.ComponentModel.DataAnnotations;
using YourNamespace;

namespace GraduationProject_E_ParkingSystem.Models;

public class ParkingSpots
{
    public int Id { get; set; }
    [Required]
    [UniqueSpotNumber]
    public string SlotName { get; set; }
    public bool Isbooked {  get; set; }
    public bool IsAvailable { get; set; } = true;

    // One parking spot can have many parking records over time
    public List<ParkingRecord>? ParkingRecords { get; set; }
}
