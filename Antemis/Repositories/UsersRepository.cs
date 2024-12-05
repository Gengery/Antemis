using Antemis.Database;
using Antemis.Models;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace Antemis.Repositories
{
	public static class UsersRepository
	{
		public static UserModel? CurrentUser { get; set; }
		public static string? CurrentUsersInn { get; set; }


		public static void ClearAllUsers()
		{
			using (PostgresContext db = new PostgresContext())
			{
				db.Users.ExecuteDelete();
				db.SaveChanges();
			}
		}
		public static void SetCurrentUserByInn(string inn)
		{
			if (CurrentUser == null)
				using (PostgresContext db = new PostgresContext())
				{
					User u = db.Users.Where(x => x.Inn == inn).Single();
					Person p = db.Persons.Where(x => x.Inn == u.Inn).Single();

					UserModel um = new UserModel()
					{
						Surname = p.Surname,
						Name = p.Name,
						Patronimic = p.Patronimic,
						Gender = p.Gender,
						INN = p.Inn,
						BirthDate = p.Birthdate,
						Img = u.Imagename,
						Login = u.Userlogin,
						Password = u.Userpassword,
						ID = u.Userid.ToString("D12")
					};
					CurrentUser = um;
				}
		}

		public static int InnExists(string inn) //если нет пользователя - 0, если есть только личность - 1, если есть пользователь - 2
		{
			using (PostgresContext db = new PostgresContext())
			{
				if (db.Users.Where(x => x.Inn == inn).Count() > 0)
					return 2;
				else if (db.Persons.Where(x => x.Inn == inn).Count() > 0)
					return 1;
				else
					return 0;
			}
		}

		public static bool LoginExists(string login)
		{
			using (PostgresContext db = new PostgresContext())
			{
				if (db.Users.Where(x => x.Userlogin == login).Count() > 0)
					return true;
				return false;
			}
		}

		public static User? GetUserByInn(string inn)
		{
			using (PostgresContext db = new PostgresContext())
			{
				return db.Users.Where(x => x.Inn == inn).Single();
			}
		}

		public static bool AutentificateUser(string login, string password)
		{
			using (PostgresContext db = new PostgresContext())
			{
				var users = db.Users.Where(x => x.Userlogin == login);
				if (users.Count() == 0)
					return false;
				var user = users.Single();
				if (user.Userpassword == password)
				{
					CurrentUsersInn = user.Inn;
					SetCurrentUserByInn(user.Inn);
					return true;
				}
				return false;
			}
		}

		public static Person? GetPersonByInn(string inn)
		{
			using (PostgresContext db = new PostgresContext())
			{
				return db.Persons.Where(x => x.Inn == inn).Single();
			}
		}

		public static void CreatePerson(Person person)
		{
			using (PostgresContext db = new PostgresContext())
			{
				db.Persons.Add(person);
				db.SaveChanges();
			}
		}

		public static void CreateUser(User user)
		{
			using (PostgresContext db = new PostgresContext())
			{
				db.Users.Add(user);
				db.SaveChanges();
			}
		}

		public static void CreateReservation(Customer reservation)
		{
			using (var db = new PostgresContext())
			{
				db.Customers.Add(reservation);
				db.SaveChanges();
			}
		}
		
		public static bool RoomIsFreeForDates(int hid, int num, DateOnly fDate, DateOnly sDate)
		{
			using (var db = new PostgresContext())
			{
				return db.Customers
					.Where(x => x.Hotelid == hid && x.Roomnumber == num)
					.Where(x => x.Arrivaldate <= sDate && x.Leavingdate >= fDate)
					.Count() == 0;
			}
		}

		public static List<UResModel> GetCurrentUserReservations()
		{
			using (var db = new PostgresContext())
			{
				return db.Customers
					.Where(x => x.Customerinn == CurrentUser.INN)
					.Select(x => new UResModel()
					{
						ADate = x.Arrivaldate,
						LDate = x.Leavingdate,
						HotelName = db.Hotels.Single(y => y.Hotelid == x.Hotelid).Hotelname,
						Price = db.Rooms.Single(y => y.Hotelid == x.Hotelid && y.Roomnumber == x.Roomnumber).Priceforday * (x.Leavingdate.DayNumber - x.Arrivaldate.DayNumber),
						RoomNumber = x.Roomnumber
					})
					.ToList();
			}
		}
	}
}
