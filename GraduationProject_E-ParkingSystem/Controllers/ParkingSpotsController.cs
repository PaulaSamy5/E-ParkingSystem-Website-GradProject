using GraduationProject_E_ParkingSystem.Models;
using GraduationProject_E_ParkingSystem.Models.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject_E_ParkingSystem.Controllers
{
	public class ParkingSpotsController : Controller
	{
		DBContext context = new DBContext();
		public IActionResult Add()
		{
			ParkingSetting parkingSetting = context.parkingSettings.Where(i => i.id == 1).FirstOrDefault();

			if (parkingSetting != null)
			{
				TempData["PricePerHour"] = parkingSetting.PricePerHour; // Store PricePerHour as a double
			}
			return View("AddParkingSpots");
		}
		public IActionResult UpdateThePrice(double price)
		{
			ParkingSetting parkingSetting = context.parkingSettings.Where(i => i.id == 1).FirstOrDefault();
			if (parkingSetting != null)
			{
				parkingSetting.PricePerHour = price; // Update the price
				context.SaveChanges();
				TempData["PricePerHour"] = parkingSetting.PricePerHour.ToString();
			}
			return RedirectToAction("Add", "ParkingSpots");
		}
		public IActionResult SaveSpot(ParkingSpots parkingSpot)
		{
			ParkingSetting parkingSetting = context.parkingSettings.Where(i => i.id == 1).FirstOrDefault();
			if (parkingSpot != null)
			{
				if (ModelState.IsValid)
				{
					context.ParkingSpots.Add(parkingSpot);
					context.SaveChanges();
					TempData["PricePerHour"] = parkingSetting.PricePerHour.ToString();
					return View("AddParkingSpots", new ParkingSpots());
				}
				else
				{
					TempData["PricePerHour"] = parkingSetting.PricePerHour.ToString();

					return View("AddParkingSpots", parkingSpot);

				}
			}
			TempData["PricePerHour"] = parkingSetting.PricePerHour.ToString();
			return View("AddParkingSpots", new ParkingSpots());
		}
		public IActionResult CheckAndUpdateBookingStatus()
		{
			try
			{
                //Console.WriteLine("ttt");
				// Get the current time
				var currentTime = DateTime.Now;
                //			var expiredBookings = context.parkingRecords
                //.Include(b => b.ParkingSpot) // Ensure ParkingSpot is loaded
                //.Where(b => b.EndTime <= currentTime && b.ParkingSpot.Isbooked)
                //.ToList();
                var expiredRecordsById = context.parkingRecords
                        .Where(pr => pr.EndTime <= DateTime.Now)
                        .Select(pr => pr.ParkingSpotId)
                        .Distinct()
                        .ToList();
				//Console.WriteLine(expiredBookings.FirstOrDefault().EndTime);
				Console.WriteLine(expiredRecordsById.FirstOrDefault());

				// Loop through each expired booking and update the spot status
				//foreach (var booking in expiredRecordsById)
				//{
				//	// Set the Isbooked flag to false
				//	var spot = context.ParkingSpots.FirstOrDefault(s => s.Id == booking);
				//	if (spot != null)
				//	{
				//			spot.Isbooked = false;
				//	}
				//}

				var latestRecords = context.parkingRecords
        .GroupBy(pr => pr.ParkingSpotId)
        .Select(g => new
        {
            ParkingSpotId = g.Key,
            MaxEndTime = g.Max(pr => pr.EndTime)
        })
        .ToList();

				Console.WriteLine(DateTime.Now);
				var expiredSpots = latestRecords
                    .Where(r => r.MaxEndTime <= DateTime.Now)
                    .Select(r => r.ParkingSpotId)
                    .ToList();

                if (expiredSpots.Any())
				{
					var spotsToUpdate = context.ParkingSpots
						.Where(ps => expiredSpots.Contains(ps.Id))
						.ToList();

					foreach (var spot in spotsToUpdate)
					{
						spot.Isbooked = false;

					}
					// Save the changes
					context.SaveChanges();
					return Json(new { success = true });
				}
                return Json(new { success = false});
            }
            catch (Exception ex)
			{
				// Log the exception if necessary and return an error response
				return Json(new { success = false, message = ex.Message });
			}
		}

	}
}
