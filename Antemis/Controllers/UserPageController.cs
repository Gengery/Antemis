using Antemis.ComplexModels;
using Antemis.Database;
using Antemis.Models;
using Antemis.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Antemis.Controllers
{
	public class UserPageController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public IActionResult MyPage()
		{
			return View(UsersRepository.CurrentUser);
		}

		[HttpGet]
		public IActionResult HotelSelection()
		{
			var m = new HotelComplexModel();
			m.Hotels = HotelsRepository.GetHotelsList();
			return View(m);
		}

		[HttpPost]
		public IActionResult HotelSelection(HotelComplexModel hcm)
		{
			return RedirectToAction("Index");
		}

		[HttpGet]
		public IActionResult RoomSelection(int hid)
		{
			var m = new RoomsComplexModel();
			m.Rooms = HotelsRepository.GetRoomsListForHotel(hid, true);
			m.Hid = hid;
			return View(m);
		}

		[HttpPost]
		public IActionResult RoomSelection(RoomsComplexModel rcm, int hid)
		{
			var m = new RoomsComplexModel();
			m.Rooms = HotelsRepository.GetRoomsListUsingSort(rcm.Sort, hid, true);
			m.Hid = hid;
			return View(m);
		}

		[HttpGet]
		public IActionResult MakeReservation(int hid, int num)
		{
			var res = new ReservationModel();
			res.HotelID = hid;
			res.RoomNumber = num;
			res.INN = UsersRepository.CurrentUser.INN;
			return View(res);
		}

		[HttpPost]
		public IActionResult MakeReservation(ReservationModel rm)
		{
			if (ModelState.IsValid)
			{
				if (rm.ArrivalDate <= rm.LeavingDate && rm.ArrivalDate >= DateOnly.FromDateTime(DateTime.Now))
				{
					if (UsersRepository.RoomIsFreeForDates(rm.HotelID, rm.RoomNumber, rm.ArrivalDate, rm.LeavingDate))
					{
						var res = new Customer();
						res.Hotelid = rm.HotelID;
						res.Roomnumber = rm.RoomNumber;
						res.Customerinn = UsersRepository.CurrentUser.INN;
						res.Arrivaldate = rm.ArrivalDate;
						res.Leavingdate = rm.LeavingDate;
						UsersRepository.CreateReservation(res);
						return RedirectToAction("Index");
					}
					else
						ModelState.AddModelError("LeavingDate", "Бронь на эти даты уже существует, воспользуйтесь фильтром");
				}
				else
				{
					ModelState.AddModelError("LeavingDate", "Некорректный промежуток дат");
				}
			}
			return View(rm);
		}

		[HttpGet]
		public IActionResult MyReservations()
		{
			return View(UsersRepository.GetCurrentUserReservations());
		}
	}
}
