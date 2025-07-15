using GraduationProject_E_ParkingSystem.Models;
using GraduationProject_E_ParkingSystem.Models.Context;
using GraduationProject_E_ParkingSystem.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System.Configuration;
using System.Diagnostics;
using System.Dynamic;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;

namespace GraduationProject_E_ParkingSystem.Controllers
{
    public class UserController : Controller
    {
        DBContext context = new DBContext();
        public IActionResult SignIn()
        {
            return View("SignIn");
        }
        public IActionResult SignUp()
        {
            return View("SignUp");
        }

        public IActionResult SaveUser(User user)
        {


            /* Added 
			   with removing it comment emailunique valideation in User	
			*/
            if (context.Users.Any(u => u.Email == user.Email))
            {
                ModelState.AddModelError("Email", "Email already exist");
            }

            //var emailPattern = @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$";
            //if (!Regex.IsMatch(user.Email ?? "", emailPattern, RegexOptions.IgnoreCase))
            //{
            //	ModelState.AddModelError("Email", "Invalid email format. Please enter a valid email address.");
            //}

            if (ModelState.IsValid)
            {
                context.Users.Add(user);
                context.SaveChanges();
                TempData["SuccessMessage"] = "Done! Now You are one of our family!";
                return RedirectToAction("SignUp");
            }

            return View("SignUp", user);
        }
        [HttpGet]
        public IActionResult CheckEmailExists(string email)
        {
            bool exists = context.Users.Any(u => u.Email == email);
            return Json(!exists); // نرجع true إذا الإيميل **غير موجود** عشان validation ينجح
        }

        //public IActionResult SaveLogin(User user)
        //{
        //    if (string.IsNullOrEmpty(user?.UserName) && string.IsNullOrEmpty(user?.Password))
        //    {
        //        TempData["WrongMessage"] = "Enter Your Name and your password please..!";
        //        return View("SignIn");
        //    }
        //    if (string.IsNullOrEmpty(user?.UserName))
        //    {
        //        TempData["WrongMessage"] = "Enter Your Name please";
        //        return View("SignIn");
        //    }

        //    if (string.IsNullOrEmpty(user?.Password))
        //    {
        //        TempData["WrongMessage"] = "Enter Your Password please";
        //        return View("SignIn", user);
        //    }


        //    var checkLogin = context.Users
        //        .Where(x => x.UserName.Equals(user.UserName) && x.Password.Equals(user.Password))
        //        .FirstOrDefault(); // or use .SingleOrDefault() depending on your requirements
        //    if (checkLogin != null)
        //    {
        //        //HttpContext.Session.SetInt32("UserId", checkLogin.Id); // Assuming Id is an integer
        //        HttpContext.Session.SetString("UserName", checkLogin.UserName);
        //        HttpContext.Session.SetInt32("Id", checkLogin.Id);
        //        /*Set the password*/
        //        HttpContext.Session.SetString("Password", checkLogin.Password);
        //        List<ParkingSpot> parkingSpots = context.ParkingSpots.ToList();
        //        dynamic Data = new ExpandoObject();
        //        Data.Spots = parkingSpots;
        //        Data.UserDetails = checkLogin;

        //        //return View("MainPage", Data);
        //        return View("M", Data);
        //    }
        //    TempData["WrongMessage"] = "Wrong username or password.";

        //    return View("SignIn", user);
        //}


        public IActionResult SaveLogin(User user)
        {
            // Check if both Email and Password are empty
            if (string.IsNullOrEmpty(user?.Email) && string.IsNullOrEmpty(user?.Password))
            {
                TempData["WrongMessage"] = "Enter your email and your password, please.";
                return View("SignIn");
            }
            // Check if Email is empty
            if (string.IsNullOrEmpty(user?.Email))
            {
                TempData["WrongMessage"] = "Enter your email, please";
                return View("SignIn");
            }
            // Check if Password is empty
            if (string.IsNullOrEmpty(user?.Password))
            {
                TempData["WrongMessage"] = "Enter your password, please";
                return View("SignIn", user);
            }

            // Check if a user with the provided Email and Password exists in the database
            //var checkLogin = context.Users
            //	.Where(x => x.Email.Equals(user.Email) && x.Password.Equals(user.Password))
            //	.FirstOrDefault(); // or use .SingleOrDefault() depending on your requirements

            //da 34an mokarnet el uppper w lower cases
            var checkLogin = context.Users
    .Where(x => x.Email == user.Email) // optional filter to reduce result set
    .AsEnumerable() // brings results into memory
    .FirstOrDefault(x =>
	  x.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase) &&
		x.Password.Equals(user.Password, StringComparison.Ordinal));



            if (checkLogin != null)
            {
                // Store user details in the session
                //HttpContext.Session.SetInt32("UserId", checkLogin.Id); // Assuming Id is an integer
                HttpContext.Session.SetString("UserName", checkLogin.FName);
                HttpContext.Session.SetInt32("Id", checkLogin.Id);
                /*Set the password*/
                HttpContext.Session.SetString("Password", checkLogin.Password);
                HttpContext.Session.SetString("IsAdmin", checkLogin.IsAdmin.ToString());
                // Retrieve parking spots from the database
                ViewData["UserName"] = checkLogin.FName;

                List<ParkingSpots> parkingSpots = context.ParkingSpots.ToList();
                List<ParkingRecord> parkingRecords = context.parkingRecords.ToList();
                ParkingSetting parkingSetting = context.parkingSettings.FirstOrDefault(c => c.id == 1);
                // Create a dynamic object to pass user and parking spot details to the view
                dynamic Data = new ExpandoObject();
                Data.Spots = parkingSpots;
                Data.UserDetails = checkLogin;
                Data.records = parkingRecords;



                //price  
                /////////////////////////////////
                Data.price = parkingSetting;
                ////////////////////////////////
                if (checkLogin.IsAdmin)
                {
                    ViewData["ActivePage"] = "HomePage";
                    return RedirectToAction("HomePage", "AdminDashboard");
                    //return View("Adminstrator", Data);
                }
                // Return the main page view with the data
                //return View("MainPage", Data);
                ViewData[index: "ActivePage"] = "HomePage";
                return View("HomePage", Data);
            }

            // If login fails, show an error message
            TempData["WrongMessage"] = "Wrong email or password.";
            return View("SignIn", user);
        }
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
                ViewData["UserName"] = checkLogin.FName;

                // Prepare the data for the view
                dynamic Data = new ExpandoObject();
                Data.Spots = parkingSpots;
                Data.UserDetails = checkLogin;
                Data.records = parkingRecords;
                if (checkLogin.IsAdmin)
                {
                    ViewData["ActivePage"] = "HomePage";
                    //return View("Adminstrator", Data);
                    return RedirectToAction("HomePage", "AdminDashboard");

                }
                ViewData[index: "ActivePage"] = "HomePage";

                // Return the HomePage view with the data
                return View("HomePage", Data);
            }

            // If for some reason the session is invalid, redirect to the login page
            return RedirectToAction("SignIn", "User");
        }

        //public IActionResult SaveLogin(User user)
        //{
        //	if (string.IsNullOrEmpty(user?.Email) && string.IsNullOrEmpty(user?.Password))
        //	{
        //		TempData["WrongMessage"] = "Enter your email and password, please!";
        //		return View("SignIn");
        //	}

        //	if (string.IsNullOrEmpty(user?.Email))
        //	{
        //		TempData["WrongMessage"] = "Enter your email, please!";
        //		return View("SignIn");
        //	}

        //	if (string.IsNullOrEmpty(user?.Password))
        //	{
        //		TempData["WrongMessage"] = "Enter your password, please!";
        //		return View("SignIn", user);
        //	}

        //	var checkLogin = context.Users
        //		.FirstOrDefault(x => x.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase) &&
        //							 x.Password.Equals(user.Password));

        //	if (checkLogin != null)
        //	{
        //		HttpContext.Session.SetString("UserName", checkLogin.UserName);
        //		HttpContext.Session.SetInt32("Id", checkLogin.Id);
        //		HttpContext.Session.SetString("Password", checkLogin.Password);

        //		var parkingSpots = context.ParkingSpots.ToList();
        //		var parkingRecords = context.parkingRecords.ToList();

        //		dynamic Data = new ExpandoObject();
        //		Data.Spots = parkingSpots;
        //		Data.UserDetails = checkLogin;
        //		Data.records = parkingRecords;

        //		return View("MainPage", Data);
        //	}

        //	TempData["WrongMessage"] = "Wrong email or password.";
        //	return View("SignIn", user);
        //}

        public IActionResult LogOut()
        {
            // Clear the session
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
        public IActionResult MainPage()
        {
            // Retrieve user details from the session
            var userName = HttpContext.Session.GetString("UserName");
            var userId = HttpContext.Session.GetInt32("Id");

            if (userName == null || userId == null)
            {
                TempData["WrongMessage"] = "Please login again.";
                return RedirectToAction("SignIn");
            }

            // Fetch the user details from the database using the session Id
            var checkLogin = context.Users.FirstOrDefault(x => x.Id == userId);

            if (checkLogin == null)
            {
                TempData["WrongMessage"] = "User not found. Please login again.";
                return RedirectToAction("SignIn");
            }
            ViewData["UserName"] = checkLogin.FName;
            // Fetch all ParkingSpots
            //List<ParkingSpots> parkingSpots = context.ParkingSpots.Where(p => p.SlotName.Contains("C")).ToList();
            List<ParkingSpots> parkingSpots = context.ParkingSpots
            .Where(p => p.SlotName.Contains("C"))
            .ToList()
            .OrderBy(p =>
            {
                var numberPart = new string(p.SlotName.SkipWhile(c => !char.IsDigit(c)).ToArray());
                return int.TryParse(numberPart, out int num) ? num : int.MaxValue;
            })
            .ToList();

            List<ParkingRecord> parkingRecords = context.parkingRecords.ToList();
            ParkingSetting parkingSetting = context.parkingSettings.FirstOrDefault(c => c.id == 1);
            //ParkingRecord record = new ParkingRecord();
            // Prepare the data to pass to the view
            dynamic Data = new ExpandoObject();
            Data.Spots = parkingSpots;
            Data.UserDetails = checkLogin;
            Data.records = parkingRecords;
            Data.price = parkingSetting;
            //Data.record = record;
            if (checkLogin.IsAdmin)
            {
                return RedirectToAction("HomePage", "AdminDashboard");
                //return View("Adminstrator", Data);
            }
            ViewData[index: "ActivePage"] = "CarSlots";

            // Return the MainPage view with data
            return View("MainPage", Data);
        }


        public IActionResult MainPageBus()
        {
            // Retrieve user details from the session
            var userName = HttpContext.Session.GetString("UserName");
            var userId = HttpContext.Session.GetInt32("Id");

            if (userName == null || userId == null)
            {
                TempData["WrongMessage"] = "Please login again.";
                return RedirectToAction("SignIn");
            }

            // Fetch the user details from the database using the session Id
            var checkLogin = context.Users.FirstOrDefault(x => x.Id == userId);

            if (checkLogin == null)
            {
                TempData["WrongMessage"] = "User not found. Please login again.";
                return RedirectToAction("SignIn");
            }
            ViewData["UserName"] = checkLogin.FName;
            // Fetch all ParkingSpots
            //List<ParkingSpots> parkingSpots = context.ParkingSpots.Where(p => p.SlotName.Contains("B")).ToList();
            List<ParkingSpots> parkingSpots = context.ParkingSpots
            .Where(p => p.SlotName.Contains("B"))
            .ToList()
            .OrderBy(p =>
            {
                var numberPart = new string(p.SlotName.SkipWhile(c => !char.IsDigit(c)).ToArray());
                return int.TryParse(numberPart, out int num) ? num : int.MaxValue;
            })
            .ToList();

            List<ParkingRecord> parkingRecords = context.parkingRecords.ToList();
            ParkingSetting parkingSetting = context.parkingSettings.FirstOrDefault(c => c.id == 1);

            // Prepare the data to pass to the view
            dynamic Data = new ExpandoObject();
            Data.Spots = parkingSpots;
            Data.UserDetails = checkLogin;
            Data.records = parkingRecords;
            Data.price = parkingSetting;
            if (checkLogin.IsAdmin)
            {
                return View("Adminstrator", Data);
            }
            ViewData[index: "ActivePage"] = "BusSlots";

            // Return the MainPage view with data
            return View("MainPageBus", Data);
        }

        //public IActionResult MainPage()
        //{
        //	var userId = HttpContext.Session.GetInt32("Id");

        //	if (userId == null)
        //	{
        //		TempData["WrongMessage"] = "Please log in again.";
        //		return RedirectToAction("SignIn");
        //	}

        //	var checkLogin = context.Users.FirstOrDefault(x => x.Id == userId);

        //	if (checkLogin == null)
        //	{
        //		TempData["WrongMessage"] = "User not found. Please log in again.";
        //		return RedirectToAction("SignIn");
        //	}

        //	var parkingSpots = context.ParkingSpots.ToList();
        //	var parkingRecords = context.parkingRecords.ToList();

        //	dynamic Data = new ExpandoObject();
        //	Data.Spots = parkingSpots;
        //	Data.UserDetails = checkLogin;
        //	Data.records = parkingRecords;

        //	return View("MainPage", Data);
        //}
        [HttpPost]
        public IActionResult EnterDetails(int UserId, int SpotId)
        {
            var user = context.Users.FirstOrDefault(u => u.Id == UserId);
            var spot = context.ParkingSpots.FirstOrDefault(s => s.Id == SpotId);

            if (user == null || spot == null)
            {
                // Handle cases where user or spot is not found
                return NotFound();
            }

            //Authontication  تعديلة عشان الـ 
            if (spot.Isbooked == true || spot.IsAvailable == false)
            {
                return NotFound();
            }
            //DateTime now = DateTime.Now;
            //TimeSpan start = new TimeSpan(16, 0, 0); // 4:00 PM
            //TimeSpan end = new TimeSpan(16, 12, 0);   // 5:00 PM

            //if (now.TimeOfDay >= start && now.TimeOfDay < end)
            //{
            //	return NotFound();
            //}


            UserSpotRecordDetailsViewModel rec = new UserSpotRecordDetailsViewModel();
            rec.UserId = UserId;
            rec.SpotId = SpotId;
            rec.SpotNumber = spot.SlotName;  // Corrected to assign the SpotNumber property
            rec.StartTime = DateTime.Now.AddMinutes(30); // Set the current date/time plus 30 minutes
            rec.EndTime = rec.StartTime.AddHours(1); // Set a default end time, for example, 1 hour later
            if (spot.SlotName.Contains("B"))
            {
                rec.vehicleType = "Bus";
                ViewData[index: "ActivePage"] = "BusSlots";

            }
            else
            {
                rec.vehicleType = "Car";
                ViewData[index: "ActivePage"] = "CarSlots";

            }

            return View("EnterDetails2", rec);  // Pass the rec object to the view
        }

        [HttpPost]
        public IActionResult BookTheSlot(UserSpotRecordDetailsViewModel parkingRecord, int spotid, int userid)
        {



            // Set SpotId and UserId if they are passed from the view
            parkingRecord.SpotId = spotid;
            parkingRecord.UserId = userid;

            var spot = context.ParkingSpots.FirstOrDefault(s => s.Id == parkingRecord.SpotId);

            if (spot.Isbooked == true || spot.IsAvailable == false)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                // Set the start time to the current time
                parkingRecord.StartTime = DateTime.Now.AddMinutes(30);

                // Calculate the leaving time based on the duration // changed to minutes
                parkingRecord.EndTime = parkingRecord.StartTime.AddHours(parkingRecord.Duration);

                // Calculate the cost
                parkingRecord.Cost = parkingRecord.CalculateCost();

                // Activation تعديل عشان الـ Ac
                if (parkingRecord.vehicleType == "Bus") ViewData[index: "ActivePage"] = "BusSlots";
                else ViewData[index: "ActivePage"] = "CarSlots";



                return View("BookingConfirmation", parkingRecord);
            }

            // If the model is invalid, ensure that SpotNumber and other details are preserved in the view model
            var user = context.Users.FirstOrDefault(u => u.Id == parkingRecord.UserId); // Get the user data

            if (spot != null)
            {
                // Ensure that the SpotNumber is retained when the model state is invalid
                parkingRecord.SpotNumber = spot.SlotName;
            }

            // Return the same view with the existing parking record data, including Spot and User info
            return View("EnterDetails2", parkingRecord);  // Pass the rec object to the view
        }
        public IActionResult ConfirmBooking(UserSpotRecordDetailsViewModel record, int spotid, int userid)
        {
            var spot = context.ParkingSpots.FirstOrDefault(s => s.Id == record.SpotId);

            if (spot.Isbooked == true || spot.IsAvailable == false)
            {
                return NotFound();
            }

            return View("ConfirmBooking", record);
        }
        //public IActionResult Done(UserSpotRecordDetailsViewModel record, int spotid, int userid)
        //{
        //	// Check if the model is valid
        //	if (!ModelState.IsValid)
        //	{
        //		return View("ConfirmBooking", record); // Return an error view if the model is invalid
        //	}

        //	try
        //	{
        //		// Fetch the spot and user from the context (using the DbContext)
        //		var spot = context.ParkingSpots.FirstOrDefault(s => s.Id == spotid);
        //		if (spot == null)
        //		{
        //			return NotFound(); // Return 404 if the spot doesn't exist
        //		}

        //		var user = context.Users.FirstOrDefault(u => u.Id == userid);
        //		if (user == null)
        //		{
        //			return NotFound(); // Return 404 if the user doesn't exist
        //		}

        //		// Create the booking object using the data from the ViewModel
        //		var booking = new ParkingRecord
        //		{
        //			ParkingSpotId = spotid,
        //			UserId = userid,
        //			StartTime = record.StartTime,
        //			EndTime = record.EndTime,
        //			Duration = record.Duration,
        //			Cost = record.Cost,
        //			vehicleType = record.vehicleType,
        //			PlateNumber = record.PlateNumber,
        //		};

        //		// Add the booking to the context
        //		context.parkingRecords.Add(booking);

        //		// Mark the spot as unavailable (assuming you have an IsAvailable property)
        //		spot.Isbooked = true;
        //		context.SaveChanges();
        //		return View("Done", record);
        //	}
        //	catch (Exception ex)
        //	{
        //		// Log the error and return an error page (if necessary)
        //		// You can use your logger here to log the exception
        //		return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //	}
        //}






        // الكانت شغالة بس لما نعمل ريفرش تسجل ريكورد تانى 


        //public IActionResult Done(UserSpotRecordDetailsViewModel record, int spotid, int userid)
        //{
        //	// Check if the model is valid
        //	if (!ModelState.IsValid)
        //	{
        //		return View("ConfirmBooking", record); // Return an error view if the model is invalid
        //	}

        //	try
        //	{

        //		// Fetch the spot and user from the context (using the DbContext)
        //		var spot = context.ParkingSpots.FirstOrDefault(s => s.Id == spotid);
        //		if (spot == null)
        //		{
        //			return NotFound(); // Return 404 if the spot doesn't exist
        //		}

        //		var user = context.Users.FirstOrDefault(u => u.Id == userid);
        //		if (user == null)
        //		{
        //			return NotFound(); // Return 404 if the user doesn't exist
        //		}
        //		var payment = new Payment
        //		{
        //			PaymentMethod = record.PaymentMethod,
        //			TotalCostPaid = record.Cost,
        //		};
        //		context.payments.Add(payment);
        //		context.SaveChanges();
        //		// Create the booking object using the data from the ViewModel
        //		var booking = new ParkingRecord
        //		{
        //			ParkingSpotId = spotid,
        //			UserId = userid,
        //			PaymentId = payment.Id,
        //			StartTime = record.StartTime,
        //			EndTime = record.EndTime,
        //			Duration = record.Duration,
        //			Cost = record.Cost,
        //			vehicleType = record.vehicleType,
        //			PlateNumber = record.PlateNumber,
        //		};

        //              //Console.WriteLine(booking.Id);
        //		// Generate a unique QR code using the booking details
        //		string uniqueText = $"{spotid}-{userid}-{Guid.NewGuid()}"; // Unique string for the QR code
        //		QRCodeGenerator qrGenerator = new QRCodeGenerator();
        //		QRCodeData qrCodeData = qrGenerator.CreateQrCode(uniqueText, QRCodeGenerator.ECCLevel.Q);

        //		using (var qrCode = new Base64QRCode(qrCodeData))
        //		{
        //			string base64Image = qrCode.GetGraphic(20); // Generate a PNG image and get it as Base64

        //			// Store the Base64 string in the QRCode property (as a string, as per your class definition)
        //			booking.QRCode = base64Image;
        //			record.QRCode = base64Image;

        //              }

        //		// Add the booking to the context
        //		context.parkingRecords.Add(booking);
        //              Console.WriteLine(booking.Id);
        //		// Mark the spot as unavailable (assuming you have an IsAvailable property)
        //		spot.Isbooked = true;
        //		context.SaveChanges();
        //		Console.WriteLine(booking.Id);
        //		//context.SaveChanges();
        //		// Return the "Done" view with the booking details
        //		return View("Done", record);
        //	}
        //	catch (Exception ex)
        //	{
        //		// Log the error and return an error page (if necessary)
        //		// You can use your logger here to log the exception
        //		return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //	}
        //}


        // شغالة ولما تعمل ريفرش بتتشتغل

        public IActionResult Done(UserSpotRecordDetailsViewModel record, int spotid, int userid)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                return View("ConfirmBooking", record); // Return an error view if the model is invalid
            }

            try
            {
                // Fetch the spot and user from the database
                var spot = context.ParkingSpots.FirstOrDefault(s => s.Id == spotid);
                if (spot == null)
                {
                    return NotFound(); // Return 404 if the spot doesn't exist
                }

                var user = context.Users.FirstOrDefault(u => u.Id == userid);
                if (user == null)
                {
                    return NotFound(); // Return 404 if the user doesn't exist
                }

                // Create and save the payment
                var payment = new Payment
                {
                    PaymentMethod = record.PaymentMethod,
                    TotalCostPaid = record.Cost,
                };
                context.payments.Add(payment);
                context.SaveChanges();

                // Create and save the booking
                var booking = new ParkingRecord
                {
                    ParkingSpotId = spotid,
                    UserId = userid,
                    PaymentId = payment.Id,
                    StartTime = record.StartTime,
                    EndTime = record.EndTime,
                    Duration = record.Duration,
                    Cost = record.Cost,
                    vehicleType = record.vehicleType,
                    PlateNumber = record.PlateNumber,
                };

                // Generate a unique QR code
                string uniqueText = $"{spotid}-{userid}-{Guid.NewGuid()}";
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(uniqueText, QRCodeGenerator.ECCLevel.Q);

                using (var qrCode = new Base64QRCode(qrCodeData))
                {
                    string base64Image = qrCode.GetGraphic(20);
                    booking.QRCode = base64Image;
                }

                // Add booking to the context and update spot availability
                context.parkingRecords.Add(booking);
                spot.Isbooked = true;
                context.SaveChanges();

                // Redirect to a new action to prevent re-submission on refresh
                return RedirectToAction("BookingSuccess", new { bookingId = booking.Id });
            }
            catch (Exception ex)
            {
                // Log the error and return an error page
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        public IActionResult BookingSuccess(int bookingId)
        {
            var booking = context.parkingRecords
                                 .Include(b => b.payment) // Ensure Payment is loaded
                                 .FirstOrDefault(b => b.Id == bookingId);

            if (booking == null)
            {
                return NotFound(); // Handle the case where the booking doesn't exist
            }

            var viewModel = new UserSpotRecordDetailsViewModel
            {
                QRCode = booking.QRCode,
                Cost = booking.Cost,
                PaymentMethod = booking.payment?.PaymentMethod ?? "Unknown", // Avoid null reference
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                Duration = booking.Duration,
                vehicleType = booking.vehicleType,
                PlateNumber = booking.PlateNumber
            };
            //if (viewModel.vehicleType == "Bus") ViewData[index: "ActivePage"] = "BusSlots";
            //else ViewData[index: "ActivePage"] = "CarSlots";
            return View("Done", viewModel);
        }


        public IActionResult MarkArrived(int SpotId)
        {
            // Retrieve the parking spot from the database using SpotId
            var spot = context.ParkingSpots.FirstOrDefault(s => s.Id == SpotId);

            // Check if the parking spot exists
            if (spot == null)
            {
                TempData["WrongMessage"] = "Parking spot not found.";
                return RedirectToAction("MainPage");
            }

            // Change the IsAvailable property to false
            spot.IsAvailable = false;
            TempData["Arrived"] = true;
            // Save the changes to the database
            context.SaveChanges();
            if (spot.SlotName.Contains("B"))
            {
                ViewData[index: "ActivePage"] = "BusSlots";
                return RedirectToAction("MainPageBus");
            }

            // Optionally, display a success message or redirect to another page
            //TempData["SuccessMessage"] = "You have arrived at your parking spot.";
            ViewData[index: "ActivePage"] = "CarSlots";
            return RedirectToAction("MainPage");
        }
        public IActionResult MarkLeaving(/*int SpotId, */int recordid)
        {
            // Find the parking spot by SpotId
            var record = context.parkingRecords.FirstOrDefault(p => p.Id == recordid);
            var parkingSpot = context.ParkingSpots.FirstOrDefault(p => p.Id == record.ParkingSpotId);
            if (parkingSpot == null)
            {
                return NotFound("Parking spot not found.");
            }
            //if (record.EndTime < DateTime.Now /*&& parkingSpot.IsAvailable == true*/)
            //{
            //             TempData["ShowPopup"] = true; // ✅ Use TempData instead of ViewData
            //             if (parkingSpot.SlotName.Contains("Z") || parkingSpot.SlotName.Contains("Y") || parkingSpot.SlotName.Contains("X"))
            //             {
            //                 return RedirectToAction("MainPageBus");
            //             }
            //             return RedirectToAction("MainPage");
            //	//return View("PayFine");
            //}
            if (record.EndTime/*.AddMinutes(5)*/ < DateTime.Now)
            {
                // حساب التأخير
                TimeSpan delay = DateTime.Now - record.EndTime;

                // حساب الغرامة بناءً على عدد الدقائق
                double fineAmount = 0;

                if (delay > TimeSpan.Zero)
                {
                    ParkingSetting fineRatePerHour = context.parkingSettings.Where(i => i.id == 2).FirstOrDefault();

                    // حساب عدد الساعات بناءً على عدد الدقائق
                    double hoursDelayed = delay.TotalMinutes / 60; // التحويل إلى ساعات

                    // إذا كانت الساعة غير كاملة، سنحسب الغرامة بناءً على عدد الدقائق
                    fineAmount = hoursDelayed * fineRatePerHour.PricePerHour;

                }
                int roundedFineAmount = (int)Math.Ceiling(fineAmount);

                // عرض الغرامة المحسوبة
                TempData["FineAmount"] = roundedFineAmount.ToString();



                TempData["recordId"] = recordid;


                TempData["ShowPopup"] = true; // ✅ Use TempData instead of ViewData
                if (parkingSpot.SlotName.Contains("B"))
                {
                    ViewData[index: "ActivePage"] = "BusSlots";
                    return RedirectToAction("MainPageBus");
                }
                ViewData[index: "ActivePage"] = "CarSlots";
                return RedirectToAction("MainPage");
            }
            else
            {
                TempData["Leaving"] = true;
            }

            TempData["ShowPopup"] = false; // ✅ Persist this value across redirect
                                           // Mark the spot as leaving (set isBooked to false and isAvailable to true)
                                           // record.endtime = current time
            parkingSpot.Isbooked = false;
            parkingSpot.IsAvailable = true;
            record.IsFinished = true;




            record.EndTime = DateTime.Now;

            // Save the changes to the database
            context.SaveChanges();

            if (parkingSpot.SlotName.Contains("B"))
            {
                ViewData[index: "ActivePage"] = "BusSlots";
                return RedirectToAction("MainPageBus");
            }
            // Optionally, return a message or redirect the user to another page
            ViewData[index: "ActivePage"] = "CarSlots";
            return RedirectToAction("MainPage"); // Or any other view you want to show after updating
        }
        public IActionResult FineLeaving(int recordid, double fineamount)
        {
            var record = context.parkingRecords.FirstOrDefault(p => p.Id == recordid);
            var parkingSpot = context.ParkingSpots.FirstOrDefault(p => p.Id == record.ParkingSpotId);
            if (parkingSpot == null)
            {
                return NotFound("Parking spot not found.");
            }
            record.Cost += fineamount;
            parkingSpot.IsAvailable = true;
            parkingSpot.Isbooked = false;
            record.EndTime = DateTime.Now;
            record.IsFinished = true;
            context.SaveChanges();
            TempData["ShowPopup"] = false;
            TempData["Leaving"] = true;

            if (parkingSpot.SlotName.Contains("B"))
            {
                ViewData[index: "ActivePage"] = "BusSlots";
                return RedirectToAction("MainPageBus");
            }
            ViewData[index: "ActivePage"] = "CarSlots";
            // Optionally, return a message or redirect the user to another page
            return RedirectToAction("MainPage");
        }


        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetInt32("Id");
            //User user = context.Users.FirstOrDefault(u => u.Id == userId);
            User user = context.Users
                .Include(u => u.ParkingRecords)
                    .ThenInclude(pr => pr.ParkingSpot)
                .FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                user.ParkingRecords = user.ParkingRecords
                    .OrderByDescending(r => r.Id)
                    .ToList();
            }

            if (user == null)
            {
                return NotFound();
            }
            return View("Profile", user);
        }
        [HttpGet]
        public JsonResult CheckEmailExistsForUpdate(string email, int currentUserId)
        {
            var emailExists = context.Users
                .Any(u => u.Email == email && u.Id != currentUserId); // استثناء المستخدم الحالي

            return Json(new { exists = emailExists });
        }
        public IActionResult UpdateProfile()
        {
            var userId = HttpContext.Session.GetInt32("Id");

            User user = context.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }
            return View("UpdateProfile", user);
        }

        //public IActionResult UniqueEmail(string email, int id)
        //{
        //	if (id == 0)
        //	{
        //		return Json(false);
        //	}
        //	// Check if there is any course with the same name, except the one being edited (if updating)
        //	User isDuplicate = context.Users
        //.FirstOrDefault(c => c.Email.Trim().ToLower() == email.Trim().ToLower() && (c.Id != id));
        //	if (isDuplicate != null)
        //	{
        //		return Json(false); // Duplicate name found, not unique
        //	}
        //	return Json(true); // Name is unique
        //}
        public IActionResult SaveUpdateProfile(User user, int id)
        {
            User user1 = context.Users.FirstOrDefault(u => u.Id == id);

            if (user1 == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                user1.FName = user.FName;
                user1.LName = user.LName;
                user1.Email = user.Email;
                user1.Gender = user.Gender;
                user1.Password = user.Password;
                context.SaveChanges(); // Save changes to the database

                user1 = context.Users.Include(s => s.ParkingRecords).ThenInclude(p => p.ParkingSpot).FirstOrDefault(u => u.Id == id);
                user1.ParkingRecords = user1.ParkingRecords
    .OrderByDescending(p => p.Id)
    .ToList();

                return View("Profile", user1);
            }
            return View("UpdateProfile", user);
        }
        public IActionResult Feedback()
        {
            var userId = HttpContext.Session.GetInt32("Id");

            User user = context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }
            Feedback feedback = new Feedback();
            feedback.UserId = user.Id;
            ViewData[index: "ActivePage"] = "Feedback";

            return View("Feedback", feedback);
        }
        public IActionResult Savefeedback(Feedback feedback)
        {
            ViewData[index: "ActivePage"] = "Feedback";
            var userId = HttpContext.Session.GetInt32("Id");

            User user = context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                context.feedbacks.Add(feedback);
                context.SaveChanges();
                TempData["Success"] = "The feedback added Successfully";
                return RedirectToAction("Feedback");
            }
            return View("Feedback", feedback);
        }
        public IActionResult CarPlateValidation(string plateNumber)
        {
            // التحقق من إذا كان PlateNumber فارغ أو لا يتوافق مع الشروط
            if (string.IsNullOrWhiteSpace(plateNumber))
            {
                return Json(new { success = false, message = "Please enter your plate number" });
            }


            // الاستعلام الأول: الحصول على ParkingSpotId مع أكبر EndTime لكل سجل
            var latestRecords = context.parkingRecords
                .Where(pr => pr.ParkingSpotId != null) // استبعاد السجلات التي تحتوي على ParkingSpotId = null
                .GroupBy(pr => pr.ParkingSpotId) // تجميع السجلات حسب ParkingSpotId
                .Select(g => new
                {
                    ParkingSpotId = g.Key,
                    MaxEndTime = g.Max(pr => pr.EndTime) // الحصول على أكبر EndTime لكل مجموعة
                }).ToList();

            // الاستعلام الثاني: التحقق مما إذا كانت الـ ParkingSpot محجوزة أو غير متاحة
            // Loop through latestRecords to print out the ParkingSpotId and IsAvailable before the main query


            var isCarBooked = latestRecords
           .Any(latestRecord =>
               context.parkingRecords
                   .Any(pr => pr.ParkingSpotId == latestRecord.ParkingSpotId
                       && pr.EndTime == latestRecord.MaxEndTime
                       && (pr.ParkingSpot.Isbooked || !pr.ParkingSpot.IsAvailable)  // Check IsAvailable is false correctly
                       && pr.PlateNumber == plateNumber)
           );
            //Console.WriteLine(isCarBooked?.ParkingSpotId);



            if (isCarBooked)
            {
                return Json(new { success = false, message = "This car is already booked for a parking spot." });
            }



            // إضافة Console Log لتأكيد النتيجة النهائية

            return Json(new { success = true });
        }

        public IActionResult BestFeatures()
        {
            var userId = HttpContext.Session.GetInt32("Id");
            if (userId != 0)
            {
                User user = context.Users.Where(i => i.Id == userId).FirstOrDefault();
                ViewData[index: "ActivePage"] = "BestFeatures";
                return View("OurBestFetures", user);
            }
            return NotFound();
        }
        public IActionResult CancelBooking(int recordId)
        {
            var record = context.parkingRecords.FirstOrDefault(p => p.Id == recordId);
            var parkingSpot = context.ParkingSpots.FirstOrDefault(p => p.Id == record.ParkingSpotId);
            if (parkingSpot == null)
            {
                return NotFound("Parking spot not found.");
            }
            //TempData["RefundAmount"] = (record.Cost*0.8).ToString();
            record.Cost = record.Cost*0.2;
            record.EndTime = DateTime.Now;
            record.IsFinished = true;
            parkingSpot.IsAvailable = true;
            parkingSpot.Isbooked = false;
            context.SaveChanges();
            TempData["Refund"] = "True";
            if (parkingSpot.SlotName.Contains("B"))
            {
                ViewData[index: "ActivePage"] = "BusSlots";
                return RedirectToAction("MainPageBus");
            }
            ViewData[index: "ActivePage"] = "CarSlots";
            // Optionally, return a message or redirect the user to another page
            return RedirectToAction("MainPage");

        }
        public IActionResult UpdateDuration(int recordId, int duration, double amount)
        {
            var record = context.parkingRecords.FirstOrDefault(p => p.Id == recordId);
            var parkingSpot = context.ParkingSpots.FirstOrDefault(p => p.Id == record.ParkingSpotId);
            if (parkingSpot == null)
            {
                return NotFound("Parking spot not found.");
            }
            record.Duration += duration;
            record.EndTime = record.EndTime.AddHours(duration);
            record.Cost += amount;
            context.SaveChanges();
            TempData["ConfirmUpdateRecord"] = true;
            TempData["AddedDuration"] = duration.ToString();
            TempData["DeductedBalance"] = amount.ToString();
            if (parkingSpot.SlotName.Contains("B"))
            {
                ViewData[index: "ActivePage"] = "BusSlots";
                return RedirectToAction("MainPageBus");
            }
            ViewData[index: "ActivePage"] = "CarSlots";
            // Optionally, return a message or redirect the user to another page
            return RedirectToAction("MainPage");
        }
    }
}