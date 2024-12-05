using Antemis.ComplexModels;
using Antemis.Database;
using Antemis.Models;
using Antemis.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Antemis.Controllers
{
    public class HotelPageController : Controller
    {
        #region Основа
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Info()
        {
            return View(HotelsRepository.CurrentHotel);
        }
		#endregion

		#region Страницы с рабочими
		[HttpGet]
        public IActionResult WorkersData()
		{
			var old = new WorkersListComplexModel();
			old.Workers = HotelsRepository.GetWorkersList();
			old.Sort = null;
			old.Works = HotelsRepository.GetJobsList();
			return View(old);
		}

        [HttpPost]
        public IActionResult WorkersData(WorkersListComplexModel wi)
        {
            if (wi.Sort != null)
            {
                var res = new WorkersListComplexModel();
                res.Workers = HotelsRepository.GetWorkersUsingSort(wi.Sort);
                res.Sort = null;
                res.Works = HotelsRepository.GetJobsList();
                return View(res);
            }
			var old = new WorkersListComplexModel();
			old.Workers = HotelsRepository.GetWorkersList();
			old.Sort = null;
			old.Works = HotelsRepository.GetJobsList();
			return View(old);
		}

        public IActionResult RemoveWorker(string inn)
        {
            HotelsRepository.DeleteWorkerByInn(inn);
            return RedirectToAction("WorkersData");
        }

        [HttpGet]
        public IActionResult UpdateWorker(string inn)
        {
            WorkerModel model = new WorkerModel();
            var person = UsersRepository.GetPersonByInn(inn);
            var worker = HotelsRepository.GetWorkerByInn(inn);
            model.INN = inn;
            model.Surname = person.Surname;
            model.Name = person.Name;
            model.Patronimic = person.Patronimic;
            model.Birth = person.Birthdate;
            model.Gender = person.Gender;
            model.HotelID = worker.Hotelid;
            model.Img = worker.Imagename;
            return View(model);
		}

		[HttpPost]
		public IActionResult UpdateWorker(WorkerModel model)
		{
			HotelsRepository.UpdateWorker(model);
			return RedirectToAction("WorkersData");
		}

		[HttpGet]
        public IActionResult AddWorker()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddWorker(WorkerModel wm)
        {
            wm.HotelID = Int32.Parse(HotelsRepository.CurrentHotel.ID);
            if (ModelState.IsValid)
            {
                if (UsersRepository.InnExists(wm.INN) == 0)
                {
                    var person = new Person()
                    {
                        Surname = wm.Surname,
                        Name = wm.Name,
                        Patronimic = wm.Patronimic,
                        Birthdate = wm.Birth,
                        Inn = wm.INN,
                        Gender = wm.Gender
                    };
                    UsersRepository.CreatePerson(person);
                }

                if (HotelsRepository.WorkerExists(wm.INN, Int32.Parse(HotelsRepository.CurrentHotel.ID), wm.Id))
                {
                    ModelState.AddModelError("Id", "Рабочий уже существует");
                    return View();
                }

                var worker = new Worker()
                {
                    Workid = wm.Id,
                    Inn = wm.INN,
                    Hotelid = wm.HotelID,
                    Imagename = wm.Img
                };

                HotelsRepository.CreateWorker(worker);
                return RedirectToAction("WorkersData");
            }
            return View();
        }

        [HttpGet]
        public IActionResult AddJob()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddJob(WorkTypeModel wtm)
        {
            if (ModelState.IsValid)
            {
                if (!HotelsRepository.IdExists(wtm.Id))
                {
                    var job = new Work()
                    {
                        Workname = wtm.Name,
                        Workid = wtm.Id
                    };

                    HotelsRepository.CreateJob(job);
                    return RedirectToAction("WorkersData");
                }
                ModelState.AddModelError("Id", "Должность с таким идентификатором уже введена");
                return View();
            }
            return View();
        }
        #endregion

        #region Страницы с комнатами
        [HttpGet]
        public IActionResult RoomsData()
        {
            var list = HotelsRepository.GetRoomsListForHotel(Int32.Parse(HotelsRepository.CurrentHotel.ID), false);
            var m = new RoomsComplexModel();
            m.Rooms = list;
            return View(m);
        }

        [HttpPost]
        public IActionResult RoomsData(RoomsComplexModel rcm)
        {
            var m = new RoomsComplexModel();
            m.Rooms = HotelsRepository.GetRoomsListUsingSort(rcm.Sort, Int32.Parse(HotelsRepository.CurrentHotel.ID), false);
            return View(m);
        }

        [HttpGet]
        public IActionResult AddRoom()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddRoom(RoomModel rm)
        {
            if (String.IsNullOrEmpty(rm.Descryption))
                rm.Descryption = "";
            if (ModelState.IsValid)
            {
                if(!HotelsRepository.RoomExists(rm.Number))
                {
                    HotelsRepository.CreateRoom(new Room() {
                        Hotelid = Int32.Parse(HotelsRepository.CurrentHotel.ID),
                        Roomnumber = rm.Number,
                        Imagename = rm.Img,
                        Isavaible = true,
                        Priceforday = rm.DaylyPrice,
                        Placesamount = rm.PlAmount,
                        Roomdescryprion = String.IsNullOrEmpty(rm.Descryption) ? "" : rm.Descryption
                    });
                }
            }
            return RedirectToAction("RoomsData");
        }

        public IActionResult RemoveRoom(int number)
        {
            HotelsRepository.DeleteRoomByNumber(number);
            return RedirectToAction("RoomsData");
        }

        public IActionResult LockChange(int number)
        {
            HotelsRepository.UpdateRoomLock(number);
            return RedirectToAction("RoomsData");
        }

        [HttpGet]
        public IActionResult UpdateRoom(int number)
        {
            var room = HotelsRepository.GetRoomByNumber(number);
            var rm = new RoomModel() {
                HotelId = room.Hotelid,
                DaylyPrice = room.Priceforday,
                Descryption = room.Roomdescryprion,
                Img = room.Imagename,
                Number = number,
                PlAmount = room.Placesamount.Value
            };
            return View(rm);
        }

        [HttpPost]
        public IActionResult UpdateRoom(RoomModel rm)
        {
            if (ModelState.IsValid)
            {
                rm.HotelId = Int32.Parse(HotelsRepository.CurrentHotel.ID);
                HotelsRepository.UpdateRoom(rm);
                return RedirectToAction("RoomsData");
            }
            return View(rm);
        }
        #endregion

        #region Страницы с бронированиями

        [HttpGet]
        public IActionResult MyReservations()
        {
            var m = HotelsRepository.GetReservationsListForCurrentHotel();
			return View(m);
        }

		#endregion
	}
}
