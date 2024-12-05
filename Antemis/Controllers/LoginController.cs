using Antemis.Models;
using Antemis.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Antemis.Controllers
{
	public class LoginController : Controller
	{
		public IActionResult Path()
		{
			return View();
		}

		[HttpGet]
		public IActionResult Hotel()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Hotel(HotelModel hm)
		{
			if (String.IsNullOrEmpty(hm.HotelINN) || String.IsNullOrEmpty(hm.Password))
				return View(hm);
			if (HotelsRepository.Autentificate(hm.HotelINN, hm.Password))
			{
				return RedirectToAction("Index", "HotelPage");
			}
			ModelState.AddModelError("HotelINN", "Введён неверный логин или пароль");
			return View();
		}

		[HttpGet]
		public IActionResult UserForm()
		{
			return View();
		}

		[HttpPost]
		public IActionResult UserForm(UserModel um)
		{
			if (String.IsNullOrEmpty(um.Login) || String.IsNullOrEmpty(um.Password))
				return View(um);
			if (UsersRepository.AutentificateUser(um.Login, um.Password))
			{
				return RedirectToAction("Index", "UserPage");
			}
			ModelState.AddModelError("Login", "Введён неверный логин или пароль");
			return View(um);

		}
	}
}
