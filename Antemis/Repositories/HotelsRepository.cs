using Antemis.Database;
using Antemis.Models;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Antemis.Repositories
{
    public static class HotelsRepository
    {
        public static HotelModel? CurrentHotel;

		#region Установка значения и аутентификация
		public static void SetCurrentHotelByInn(string inn)
        {
            using (PostgresContext db = new PostgresContext())
            {
                Hotel hotel = db.Hotels.Single(x => x.Hotelinn == inn);
                Person director = db.Persons.Single(x => x.Inn == hotel.Hoteldirectorinn);
                Person owner = db.Persons.Single(x => x.Inn == hotel.Hotelownerinn);

                CurrentHotel = new HotelModel()
                {
                    HotelINN = hotel.Hotelinn,
                    HotelNam = hotel.Hotelname,
                    Password = hotel.Hotelpassword,
                    DirectorINN = hotel.Hoteldirectorinn,
                    DSurname = director.Surname,
                    DName = director.Name,
                    DPatronimic = director.Patronimic,
                    DBirthDate = (DateOnly)director.Birthdate,
                    DGender = director.Gender,
                    OwnerINN = hotel.Hotelownerinn,
                    OSurname = owner.Surname,
                    OName = owner.Name,
                    OPatronimic = owner.Patronimic,
                    OBirthDate = (DateOnly)owner.Birthdate,
                    OGender = owner.Gender,
                    ID = hotel.Hotelid.ToString("D12")
                };
            }
        }

        public static bool Autentificate(string hinn, string password)
        {
            using (PostgresContext db = new PostgresContext())
            {
                if (HotelsRepository.InnExists(hinn))
                    if (db.Hotels.Single(x => x.Hotelinn == hinn).Hotelpassword == password)
                    {
                        SetCurrentHotelByInn(hinn);
                        return true;
                    }
                return false;
            }
        }
		#endregion

		#region Проверка на существование
		public static bool InnExists(string inn)
        {
            using (var db = new PostgresContext())
            {
                if (db.Hotels.Where(x => x.Hotelinn == inn).Count() > 0)
                    return true;
                return false;
            }
        }

        public static bool IdExists(int id)
        {
            using (var db = new PostgresContext())
            {
                if (db.Works.Where(x => x.Workid == id).Count() > 0)
                    return true;
                return false;
            }
		}

		public static bool WorkerExists(string inn, int hotelId, int workId)
		{
			using (var db = new PostgresContext())
			{
				if (db.Workers.Where(x => x.Inn == inn && x.Hotelid == hotelId && x.Workid == workId).Count() > 0)
					return true;
				return false;
			}
		}

        public static bool RoomExists(int number)
        {
            using (var db = new PostgresContext())
            {
                if (db.Rooms.Where(x => Int32.Parse(CurrentHotel.ID) == x.Hotelid && x.Roomnumber == number).Count() > 0)
                    return true;
                return false;
            }
        }
		#endregion

		#region Добавление данных
		public static void CreateJob(Work job)
        {
            using (var db = new PostgresContext())
            {
                db.Works.Add(job);
                db.SaveChanges();
            }
		}

		public static void CreateHotel(Hotel hotel)
		{
			using (var db = new PostgresContext())
			{
				db.Add(hotel);
				db.SaveChanges();
			}
		}

		public static void CreateWorker(Worker worker)
        {
            using (var db = new PostgresContext())
            {
                db.Workers.Add(worker);
                db.SaveChanges();
            }
        }

        public static void CreateRoom(Room room)
        {
            using (var db = new PostgresContext())
            {
                db.Rooms.Add(room);
                db.SaveChanges();
            }
        }
		#endregion

		#region Получение данных
		public static List<Work> GetJobsList()
        {
            using (var db = new PostgresContext())
            {
                return db.Works.ToList();
            }
        }

        public static List<WorkerMainInfoModel> GetWorkersList()
        {
            using (var db = new PostgresContext())
            {
                return db.Workers
                    .Where(x => x.Hotelid == Int32.Parse(CurrentHotel.ID))
                    .Select(x => new WorkerMainInfoModel(db.Persons.Single(y => y.Inn == x.Inn), 
                        db.Works.Single(y => y.Workid == x.Workid).Workname,
                        x.Imagename))
                    .ToList();
            }
        }

        public static List<HotelMainInfoModel> GetHotelsList()
        {
            using (var db = new PostgresContext())
            {
                return db.Hotels.Select(x => new HotelMainInfoModel()
                {
                    HotelId = x.Hotelid,
                    HotelName = x.Hotelname,
                    RoomsAmount = db.Rooms.Where(y => y.Hotelid == x.Hotelid && y.Isavaible.Value).Count(),
                    DirectorFIO = db.Persons.Single(y => y.Inn == x.Hoteldirectorinn).Surname + ' '
                    + db.Persons.Single(y => y.Inn == x.Hoteldirectorinn).Name + ' '
                    + db.Persons.Single(y => y.Inn == x.Hoteldirectorinn).Patronimic,
					OwnerFIO = db.Persons.Single(y => y.Inn == x.Hotelownerinn).Surname + ' '
					+ db.Persons.Single(y => y.Inn == x.Hotelownerinn).Name + ' '
					+ db.Persons.Single(y => y.Inn == x.Hotelownerinn).Patronimic
				}).ToList();
            }
        }

        public static List<WorkerMainInfoModel> GetWorkersUsingSort(WorkerSortComponents sort)
        {
            using (var db = new PostgresContext())
            {
                var start = db.Workers
                    .Where(x => x.Hotelid == Int32.Parse(CurrentHotel.ID))
                    .Select(x => new WorkerMainInfoModel(db.Persons.Single(y => y.Inn == x.Inn),
                        db.Works.Single(y => y.Workid == x.Workid).Workname,
                        x.Imagename)).ToList();
                var filtered = sort.JobFilter == "None" ? start : start.Where(x => x.JobName == sort.JobFilter).ToList();
                List<WorkerMainInfoModel> sorted = null;
                if (sort.SortName != "None")
                {
                    if (sort.SortName == "fio")
                    {
						if (sort.SortOrder == 'u')
							sorted = filtered.OrderBy(x => x.Surname + x.Name + x.Patronimic).ToList();
						else
							sorted = filtered.OrderByDescending(x => x.Surname + x.Name + x.Patronimic).ToList();
					}
                    else if (sort.SortName == "inn")
                    {
						if (sort.SortOrder == 'u')
							sorted = filtered.OrderBy(x => x.INN).ToList();
						else
							sorted = filtered.OrderByDescending(x => x.INN).ToList();
					}
                    else if (sort.SortName == "job")
                    {
						if (sort.SortOrder == 'u')
							sorted = filtered.OrderBy(x => x.JobName).ToList();
						else
							sorted = filtered.OrderByDescending(x => x.JobName).ToList();
					}
                }
                else
                    sorted = filtered.ToList();
				return sorted;
			}
		}

		public static List<Room> GetRoomsListUsingSort(RoomsSortComponents sort, int hid, bool avaibilityMatter)
		{
			using (var db = new PostgresContext())
			{
				var query = db.Rooms.Where(x => x.Hotelid == hid);
                if (avaibilityMatter)
                    query = query.Where(x => x.Isavaible.Value);
				if (sort.LowerPriceFilter != null && sort.UpperPriceFilter != null)
					query = query.Where(x => x.Priceforday < sort.UpperPriceFilter && x.Priceforday > sort.LowerPriceFilter);
				if (sort.RoomsAmountFilter != -1)
					query = query.Where(x => x.Placesamount == sort.RoomsAmountFilter);
                if (sort.ADate != null && sort.LDate != null && sort.ADate <= sort.LDate)
                    query = query.Where(x => x.Customers.Where(y => y.Arrivaldate <= sort.LDate && y.Leavingdate >= sort.ADate).Count() == 0);
				if (sort.Case == "num")
				{
					if (sort.IsDescending)
						return query.OrderByDescending(x => x.Roomnumber).ToList();
					return query.OrderBy(x => x.Roomnumber).ToList();
				}
				else if (sort.Case == "price")
				{
					if (sort.IsDescending)
						return query.OrderByDescending(x => x.Priceforday).ToList();
					return query.OrderBy(x => x.Priceforday).ToList();
				}
				else if (sort.Case == "amo")
				{
					if (sort.IsDescending)
						return query.OrderByDescending(x => x.Placesamount).ToList();
					return query.OrderBy(x => x.Placesamount).ToList();
				}
				return query.ToList();
			}
		}

		public static Worker? GetWorkerByInn(string inn)
		{
			using (var db = new PostgresContext())
			{
				return db.Workers.Single(x => x.Inn == inn);
			}
		}

        public static List<Room> GetRoomsListForHotel(int id, bool aMatter)
        {
            using (var db = new PostgresContext())
            {
                if (!aMatter)
                    return db.Rooms.Where(x => x.Hotelid == id).ToList();
                else
					return db.Rooms.Where(x => x.Hotelid == id && x.Isavaible.Value).ToList();
			}
        }

        public static Room GetRoomByNumber(int number)
        {
            using (var db = new PostgresContext())
            {
                int hid = Int32.Parse(CurrentHotel.ID);
                return db.Rooms.Single(x => x.Hotelid == hid && x.Roomnumber == number);
            }
        }

        public static List<HResModel> GetReservationsListForCurrentHotel()
        {
            using (var db = new PostgresContext())
            {
                int hid = Int32.Parse(CurrentHotel.ID);
				return db.Customers
                    .Where(x => x.Hotelid == hid)
                    .Select(x => new HResModel()
                    {
                        ADate = x.Arrivaldate,
                        LDate = x.Leavingdate,
                        CustINN = x.Customerinn,
                        RoomNumber = x.Roomnumber,
                        Img = db.Users.Single(y => y.Inn == x.Customerinn).Imagename,
                        Surname = db.Persons.Single(y => y.Inn == x.Customerinn).Surname,
						Name = db.Persons.Single(y => y.Inn == x.Customerinn).Name,
						Patronimic = db.Persons.Single(y => y.Inn == x.Customerinn).Patronimic
					})
                    .ToList();
            }
        }
		#endregion

		#region Удаление данных
		public static void DeleteWorkerByInn(string inn)
        {
            using (var db = new PostgresContext())
            {
                db.Workers.Where(x => x.Inn == inn).ExecuteDelete();
                db.SaveChanges();
            }
        }

        public static void DeleteRoomByNumber(int number)
        {
            using (var db = new PostgresContext())
            {
                int hid = Int32.Parse(CurrentHotel.ID);
				db.Rooms.Where(x => x.Hotelid == hid && x.Roomnumber == number).ExecuteDelete();
                db.SaveChanges();
            }
        }
		#endregion

		#region Обновление данных
		public static void UpdateWorker(WorkerModel worker)
        {
			using (var db = new PostgresContext())
			{
				Person person = db.Persons.Single(x => x.Inn == worker.INN);
                Worker worker1 = db.Workers.Single(x => x.Inn == worker.INN);
                person.Name = worker.Name;
                person.Surname = worker.Surname;
                person.Patronimic = worker.Patronimic;
                person.Gender = worker.Gender;
                person.Birthdate = worker.Birth;
                worker1.Workid = worker.Id;
                worker1.Imagename = worker.Img;
                worker1.Hotelid = Int32.Parse(CurrentHotel.ID);
                db.SaveChanges();
			}
		}

        public static void UpdateRoom(RoomModel room)
        {
            using (var db = new PostgresContext())
            {
				Room r = db.Rooms.Single(x => x.Hotelid == room.HotelId && x.Roomnumber == room.Number);
				r.Hotelid = room.HotelId;
				r.Roomdescryprion = room.Descryption;
				r.Roomnumber = room.Number;
				r.Isavaible = true;
				r.Imagename = room.Img;
				r.Placesamount = room.PlAmount;
				r.Priceforday = room.DaylyPrice;
				db.SaveChanges();
            }
        }

        public static void UpdateRoomLock(int number)
        {
            using (var db = new PostgresContext())
            {
				int hid = Int32.Parse(CurrentHotel.ID);
                var m = db.Rooms.Single(x => x.Hotelid == hid && x.Roomnumber == number);
                m.Isavaible = !m.Isavaible;
                db.SaveChanges();
            }
        }
		#endregion
	}
}
