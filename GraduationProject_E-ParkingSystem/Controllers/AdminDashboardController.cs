using GraduationProject_E_ParkingSystem.Models;
using GraduationProject_E_ParkingSystem.Models.Context;
using GraduationProject_E_ParkingSystem.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;

namespace GraduationProject_E_ParkingSystem.Controllers
{
	public class AdminDashboardController : Controller
	{
		DBContext context = new DBContext();
		//public IActionResult Details()
		//{

		//	List<ParkingSpots> parkingSpots = context.ParkingSpots.ToList();
		//	List<ParkingRecord> parkingRecords = context.parkingRecords.ToList();
		//	ParkingSetting parkingSetting = context.parkingSettings.FirstOrDefault(c => c.id == 1);
		//	// Create a dynamic object to pass user and parking spot details to the view
		//	dynamic Data = new ExpandoObject();
		//	Data.Spots = parkingSpots;
		//	Data.records = parkingRecords;
		//	//price  
		//	/////////////////////////////////
		//	Data.price = parkingSetting;
		//	return View("Details");
		//}
		public IActionResult HomePage()
		{
			// Check if the user is already logged in (by checking if the session contains a valid user ID)
			if (HttpContext.Session.GetInt32("Id") == null)
			{
				// Redirect to the login page if the user is not logged in
				return RedirectToAction("SignIn", "User");
			}

			// Get the user details from the session
			var userId = HttpContext.Session.GetInt32("Id");
			var checkLogin = context.Users.FirstOrDefault(x => x.Id == userId);

			if (checkLogin != null)
			{
				// Retrieve parking spots and parking records
				List<ParkingSpots> parkingSpots = context.ParkingSpots.ToList();
				List<ParkingRecord> parkingRecords = context.parkingRecords.ToList();
				List<User> users = context.Users.ToList();
				List<Payment> payments = context.payments.ToList();
				ViewData["UserName"] = checkLogin.FName;
				var totalcost = parkingRecords.Sum(p => p.Cost);
				// Prepare the data for the view
				dynamic Data = new ExpandoObject();
				Data.Spots = parkingSpots;
				Data.UserDetails = checkLogin;
				Data.records = parkingRecords;
				Data.Users = users;
				Data.Payments = totalcost;
				if (checkLogin.IsAdmin)
				{
					ViewData["ActivePage"] = "HomePage";
					return View("Adminstrator", Data);
				}
				// Return the HomePage view with the data
				return View("HomePage", Data);
			}

			// If for some reason the session is invalid, redirect to the login page
			return RedirectToAction("SignIn", "User");
		}
		public IActionResult List()
		{
			var Admin = HttpContext.Session.GetInt32("Id");
			//dynamic Data = new ExpandoObject();
			//Data.User = context.Users.FirstOrDefault(i => i.Id == Admin);
			//Data.Slots = context.ParkingSpots.ToList();
			var user = context.Users.FirstOrDefault(i => i.Id == Admin);

			if (user.IsAdmin)
			{
				SlotsViewModel slots = new SlotsViewModel();
				slots.parkingSpots = context.ParkingSpots.ToList();
				ViewData[index: "ActivePage"] = "List";
				return View("OurSlots", slots);
			}
			else
			{
				return NotFound();
			}
		}

		public IActionResult DeleteSlot(int id)
		{
			ParkingSpots slot = context.ParkingSpots.FirstOrDefault(i => i.Id == id);
			List<ParkingRecord> parkingRecords = context.parkingRecords.ToList();

			if (slot == null)
			{
				return NotFound();
			}
			//bool hasRecords = context.parkingRecords.Any(r => r.ParkingSpotId == id);
			//if (hasRecords)
			//{
			//	List<ParkingRecord> parkingRecords1 = context.parkingRecords.Where(r => r.ParkingSpotId == id).ToList();
			//	context.parkingRecords.RemoveRange(parkingRecords1);
			//	context.SaveChanges();
			//}
			context.ParkingSpots.Remove(slot);
			context.SaveChanges();
			//return RedirectToAction("List");
			return NoContent();
			// Check if there are any parking records associated with the slot
		}
		[HttpPost]
		public IActionResult WindowForUpdateTheSlot([FromBody] ParkingSpots model)
		{
			if (model == null)
				return BadRequest(new { success = false, message = "Invalid Data" });

			ParkingSpots slot = context.ParkingSpots.FirstOrDefault(i => i.Id == model.Id);
			if (slot == null)
				return NotFound(new { success = false, message = "Slot not found" });

			bool isNameTaken = context.ParkingSpots.Any(s => s.SlotName == model.SlotName && s.Id != model.Id);
			if (isNameTaken)
			{
				return BadRequest(new { success = false, message = "This slot name is already taken." });
			}

			slot.SlotName = model.SlotName;
			context.SaveChanges();

			return Json(new { success = true, message = "Slot updated successfully" });
		}

		public JsonResult CheckSlotName(string? name, int id)
		{
			name = name?.Trim(); // إزالة المسافات لو فيه اسم مدخل
			if (string.IsNullOrEmpty(name))
			{
				return Json(new { exists = false }); // لو الاسم فاضي، نرجع false مباشرةً
			}

			string myslotname = context.ParkingSpots.FirstOrDefault(i => i.Id == id)?.SlotName ?? "";

			bool exists = id != 0
				? context.ParkingSpots.Any(s => s.SlotName == name && name != myslotname)
				: context.ParkingSpots.Any(s => s.SlotName == name);

			return Json(new { exists });
		}

		[HttpPost]
		public IActionResult AddNewSlot([FromBody] SlotsViewModel slot)
		{
			if (slot != null && slot.parkingspot != null)
			{
				if (ModelState.IsValid)
				{
					ParkingSpots NewSlot = new ParkingSpots
					{
						SlotName = slot.parkingspot.SlotName,
					};
					context.ParkingSpots.Add(NewSlot);
					context.SaveChanges();

					return Json(new { success = true, slotName = NewSlot.SlotName });
				}
			}
			return BadRequest(new { success = false, message = "Invalid data" });
		}
		[HttpGet]
		public IActionResult GetUsersByGender()
		{
			try
			{
				var users = context.Users.ToList();
				int maleCount = users.Count(u => u.Gender == "Male");
				int femaleCount = users.Count(u => u.Gender == "Female");

				return Json(new { male = maleCount, female = femaleCount });
			}
			catch
			{
				return Json(new { male = 0, female = 0 });
			}
		}

		public IActionResult Profits(string? timePeriod = "Day")
		{
			// تحديد الفترة الزمنية بناءً على الـ timePeriod
			List<ParkingRecord> records = context.parkingRecords.ToList();

			var groupedData = new List<ParkingProfitViewModel>();

			if (timePeriod == "Day")
			{
				// تجميع البيانات حسب اليوم
				groupedData = records.GroupBy(r => r.StartTime.Date)
									.Select(g => new ParkingProfitViewModel
									{
										Date = g.Key.ToString("yyyy-MM-dd"), // تحويل التاريخ إلى نص
										Profit = g.Sum(r => r.Cost)
									})
									.ToList();
			}
			else if (timePeriod == "Month")
			{
				// تجميع البيانات حسب الشهر والسنة
				groupedData = records.GroupBy(r => new { r.StartTime.Year, r.StartTime.Month })
									.Select(g => new ParkingProfitViewModel
									{
										Date = $"{g.Key.Year}-{g.Key.Month:D2}", // تنسيق السنة والشهر (مثل 2025-04)
										Profit = g.Sum(r => r.Cost)
									})
									.ToList();
			}
			else if (timePeriod == "Year")
			{
				// تجميع البيانات حسب السنة
				groupedData = records.GroupBy(r => r.StartTime.Year)
									.Select(g => new ParkingProfitViewModel
									{
										Date = g.Key.ToString(), // السنة كنص
										Profit = g.Sum(r => r.Cost)
									})
									.ToList();
			}
			return Json(groupedData);
		}
		public IActionResult ParkingSettingForPriceAndFine()
		{
			var Admin = HttpContext.Session.GetInt32("Id");

			var user = context.Users.Where(i => i.Id == Admin).FirstOrDefault();
			if (user.IsAdmin)
			{
				List<ParkingSetting> settings = context.parkingSettings.ToList();
				ViewData[index: "ActivePage"] = "ParkingSetting";
				return View("ParkingSettings", settings);
			}
			return NotFound();
		}

		public IActionResult UpdatePrice(double price)
		{
			ParkingSetting priceSetting = context.parkingSettings.Where(i => i.id == 1).FirstOrDefault();
			priceSetting.PricePerHour = price;
			context.SaveChanges();
			TempData["SuccessMessage"] = "Price updated successfully!";
			return RedirectToAction("ParkingSettingForPriceAndFine");
		}
		public IActionResult UpdateFine(double fine)
		{
			ParkingSetting priceSetting = context.parkingSettings.Where(i => i.id == 2).FirstOrDefault();
			priceSetting.PricePerHour = fine;
			context.SaveChanges();
			TempData["FineSuccessMessage"] = $"Fine updated successfully!";
			return RedirectToAction("ParkingSettingForPriceAndFine");
		}


		public IActionResult AllRecords(DateTime? from, DateTime? to, string? search)
		{
			var Admin = HttpContext.Session.GetInt32("Id");
			var user = context.Users.FirstOrDefault(i => i.Id == Admin);

			if (user != null && user.IsAdmin)
			{
				var records = context.parkingRecords
					.Include(pr => pr.User)
					.Include(pr => pr.ParkingSpot)
					//.Include(pr => pr.payment)
					.OrderByDescending(pr => pr.Id)
					.AsQueryable();

				// بحث بالـ PlateNumber
				if (!string.IsNullOrEmpty(search))
				{
					string cleanedSearch = search.Trim();
					records = records.Where(r => r.PlateNumber.Contains(cleanedSearch));
					ViewBag.Search = search;
				}

				// تحديد أقل تاريخ موجود في البيانات كـ from لو مش متحدد
				if (!from.HasValue || from > DateTime.Now)
				{
					from = context.parkingRecords.Any()
						? context.parkingRecords.Min(r => r.StartTime)
						: DateTime.Now.AddYears(-1); // افتراضي لو الجدول فاضي
				}

				// فلترة بالسجل من تاريخ from فقط
				records = records.Where(r => r.StartTime >= from.Value);

				// فلترة بالتاريخ to لو متحدد فقط
				if (to.HasValue)
				{
					records = records.Where(r => r.EndTime <= to.Value);
				}

				// تمرير التواريخ للـ View
				ViewBag.From = from?.ToString("yyyy-MM-ddTHH:mm");
				ViewBag.To = to?.ToString("yyyy-MM-ddTHH:mm");

				ViewData["ActivePage"] = "Records";

				return View("BookingRecords", records.ToList());
			}

			return NotFound();
		}

		public IActionResult UserDetails(int id, string fromPage = "Records")
		{
			var AdminId = HttpContext.Session.GetInt32("Id");
			if (AdminId == null)
				return RedirectToAction("Login", "Auth"); // لو مفيش session

			var adminUser = context.Users.FirstOrDefault(i => i.Id == AdminId.Value);
			if (adminUser == null || !adminUser.IsAdmin)
				return Unauthorized(); // لو المستخدم مش أدمن

			if (id == 0)
				return BadRequest(); // id مش صالح

			var userWithBookings = context.Users
			.Where(i => i.Id == id)  // التأكد من الـ Id
			.Include(u => u.ParkingRecords)
				.ThenInclude(p => p.ParkingSpot)
			.FirstOrDefault();

			if (userWithBookings != null)
			{
				// ترتيب السجلات حسب Id من الأحدث للأقدم
				userWithBookings.ParkingRecords = userWithBookings.ParkingRecords
					.OrderByDescending(r => r.Id)
					.ToList();
			}
			if (userWithBookings == null)
				return NotFound(); // المستخدم المطلوب مش موجود

			// هنا بقى نحدد الـ ActivePage بناءً على مكان الطلب
			if (fromPage == "Feedbacks")
				ViewData["ActivePage"] = "Feedbacks";
			else if (fromPage == "Users")
				ViewData["ActivePage"] = "User";
			else
				ViewData["ActivePage"] = "Records";

			return View("UserDetailsForAdmin", userWithBookings);
		}

		public IActionResult ShowFeedbacks()
		{
			var Admin = HttpContext.Session.GetInt32("Id");
			var user = context.Users.FirstOrDefault(i => i.Id == Admin);

			if (user != null && user.IsAdmin)
			{
				// أول حاجة نعمل Dictionary يحتوي عدد الحجوزات لكل UserId
				var userBookingCounts = context.parkingRecords
				.Where(p => p.UserId != null) // نتأكد إن مفيش null
				.GroupBy(p => p.UserId)
				.Select(g => new { UserId = g.Key.Value, Count = g.Count() }) // نستخدم .Value بثقة دلوقتي
				.ToDictionary(g => g.UserId, g => g.Count);

				List<Feedback> feedbacks = context.feedbacks
				.Include(f => f.User)
				.ThenInclude(p => p.ParkingRecords)
				.ToList()
				.OrderByDescending(f => userBookingCounts.ContainsKey(f.UserId) ? userBookingCounts[f.UserId] : 0) // ترتيب حسب عدد الحجوزات
				.ThenByDescending(f => f.Id) // ترتيب حسب الـ Id (من الأحدث إلى الأقدم)
				.ToList();


				// بعدين نجيب الفيدباكات ونرتبهم حسب عدد حجوزات اليوزر في ParkingRecords
				//var feedbacks = context.feedbacks
				//.Include(f => f.User)
				//.ThenInclude(p => p.ParkingRecords)
				//.ToList() // لازم ToList() هنا علشان نشتغل في الذاكرة
				//.GroupBy(f => f.UserId) // تجمع حسب كل مستخدم
				//.SelectMany(g => g
				//	.OrderByDescending(f => f.Id) // ترتب من الأحدث
				//	.Take(3)) // تاخد آخر 3 لكل مستخدم
				//.OrderByDescending(f => userBookingCounts.ContainsKey(f.UserId) ? userBookingCounts[f.UserId] : 0)
				//.ThenByDescending(f => f.Id)
				//.ToList();

				ViewData["ActivePage"] = "Feedbacks";
				return View("FeedbacksForAdmin", feedbacks);
			}
			return NotFound();
		}


		public IActionResult DeleteFeedback(int id)
		{
			// التحقق من ID الـ Admin من الـ Session
			var Admin = HttpContext.Session.GetInt32("Id");
			var user = context.Users.FirstOrDefault(i => i.Id == Admin);

			if (user != null && user.IsAdmin)
			{
				// البحث عن الفيدباك باستخدام الـ id
				var feedback = context.feedbacks.FirstOrDefault(f => f.Id == id);

				if (feedback != null)
				{
					// حذف الفيدباك
					context.feedbacks.Remove(feedback);
					// حفظ التغييرات في الـ DB
					context.SaveChanges();

					return Ok(); // لو العملية تمت بنجاح
				}
				else
				{
					return NotFound(); // لو مش لاقي الفيدباك
				}
			}

			return Unauthorized(); // لو المستخدم مش Admin
		}
		public IActionResult Users(string search, int? minBookings)
		{
			var Admin = HttpContext.Session.GetInt32("Id");
			var user = context.Users.FirstOrDefault(i => i.Id == Admin);

			if (user != null && user.IsAdmin)
			{
				List<User> users;

				string originalSearch = search;

				// Normalize the search string for comparison only
				if (!string.IsNullOrEmpty(search))
				{
					search = System.Text.RegularExpressions.Regex.Replace(search, @"\s+", " ").Trim().ToLower();
				}

				if (!string.IsNullOrEmpty(search) && minBookings.HasValue)
				{
					// Search by full name or email with minimum bookings
					users = context.Users
						.Include(p => p.ParkingRecords)
						.Where(i =>
							(i.FName + " " + i.LName).ToLower().Contains(search) ||
							i.Email.ToLower().Contains(search)
						)
						.Where(i => i.ParkingRecords.Count() >= minBookings.Value)
						.OrderByDescending(s => s.ParkingRecords.Count())
						.ToList();
				}
				else if (!string.IsNullOrEmpty(search))
				{
					// Search by full name or email only
					users = context.Users
						.Include(p => p.ParkingRecords)
						.Where(i =>
							(i.FName + " " + i.LName).ToLower().Contains(search) ||
							i.Email.ToLower().Contains(search)
						)
						.OrderByDescending(d => d.Id)
						.ToList();
				}
				else if (minBookings.HasValue)
				{
					// Filter by minimum bookings only
					users = context.Users
						.Include(p => p.ParkingRecords)
						.Where(i => i.ParkingRecords.Count() >= minBookings.Value)
						.OrderByDescending(s => s.ParkingRecords.Count())
						.ToList();
				}
				else
				{
					// Return all users
					users = context.Users
						.Include(p => p.ParkingRecords)
						.OrderByDescending(d => d.Id)
						.ToList();
				}

				// Pass original search value to the view
				ViewBag.Search = originalSearch;
				ViewBag.MinBookings = minBookings;
				ViewData["ActivePage"] = "User";
				return View("OurUsers", users);
			}

			return NotFound();
		}








	}
}