using GraduationProject_E_ParkingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GraduationProject_E_ParkingSystem.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

	}
}
