using Antemis.AdditionalFunctions;
using Antemis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Antemis.Repositories;
using Antemis.Database;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.Mapping;
using System.Globalization;
using Humanizer;

namespace Antemis.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Path()
        {
            return View();
        }

        [HttpGet]
        public IActionResult InnForm() //запросить ИНН пользователя
        {
            return View();
        }

        [HttpPost]
        public IActionResult InnForm(UserModel um) //запросить ИНН пользователя
        {
            if (um.INN != null && um.INN.Length == 12 && FHelper.OfDigits(um.INN))
            {
                int hasInn = UsersRepository.InnExists(um.INN);

                if (hasInn == 2)
                {
                    ModelState.AddModelError("INN", "ИНН уже использован");
                    return View();
                }
                else
                    return RedirectToAction("UserForm", new { um.INN });
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult UserForm(string INN) //форма регистрации пользователя
        {
            int hasInn = UsersRepository.InnExists(INN);
            if (hasInn == 0)
            {
                UserModel um = new UserModel()
                {
                    INN = INN,
                    Surname = "",
                    Name = "",
                    Patronimic = "",
                    Gender = 'm',
                    BirthDate = DateOnly.FromDateTime(DateTime.Now)
                };
                return View(um);
            }
            else
            {
                var person = UsersRepository.GetPersonByInn(INN);
                UserModel um = new UserModel();
                um.INN = INN;
                um.HasPerson = true;
                um.Surname = person.Surname;
                um.Name = person.Name;
                um.Patronimic = person.Patronimic;
                um.BirthDate = person.Birthdate;
                um.Gender = person.Gender;
                return View(um);
            }
        }

        [HttpPost]
        public IActionResult UserForm(UserModel um) //форма регистрации пользователя
        {
            if (ModelState.IsValid)
            {
                if (UsersRepository.LoginExists(um.Login))
                {
                    ModelState.AddModelError("Login", "Логин уже использован");
                    return View(um);
                }
                else if (UsersRepository.InnExists(um.INN) == 1)
                {
                    User user = new User();
                    user.Inn = um.INN;
                    user.Userlogin = um.Login;
                    user.Userpassword = um.Password;
					user.Imagename = um.Img;
					UsersRepository.CreateUser(user);
                }
                else
                {
                    Person person = new Person();
                    person.Name = um.Name;
                    person.Surname = um.Surname;
                    person.Patronimic = um.Patronimic;
                    person.Inn = um.INN;
                    person.Birthdate = um.BirthDate;
                    person.Gender = um.Gender;
                    User user = new User();
                    user.Inn = um.INN;
                    user.Userlogin = um.Login;
                    user.Userpassword = um.Password;
                    user.Imagename = um.Img;
                    UsersRepository.CreatePerson(person);
                    UsersRepository.CreateUser(user);
                }
                UsersRepository.SetCurrentUserByInn(um.INN);
                UsersRepository.CurrentUsersInn = um.INN;
                return RedirectToActionPermanent("Index", "UserPage");
            }
            return View(um);
        }

        [HttpGet]
        public IActionResult HotelForm() //форма регистрации отеля
        {
            return View();
        }

        [HttpPost]
        public IActionResult HotelForm(HotelModel hm) //форма регистрации отеля
        {
            if (ModelState.IsValid)
            {
                if (!HotelsRepository.InnExists(hm.HotelINN))
                {
                    if (UsersRepository.InnExists(hm.DirectorINN) == 0)
                    {
                        Person dir = new Person()
                        {
                            Surname = hm.DSurname,
                            Name = hm.DName,
                            Patronimic = hm.DPatronimic,
                            Birthdate = hm.DBirthDate,
                            Gender = hm.DGender,
                            Inn = hm.DirectorINN
                        };
                        UsersRepository.CreatePerson(dir);
                    }

                    if (UsersRepository.InnExists(hm.OwnerINN) == 0)
                    {
                        Person own = new Person()
                        {
                            Surname = hm.OSurname,
                            Name = hm.OName,
                            Patronimic = hm.OPatronimic,
                            Birthdate = hm.OBirthDate,
                            Gender = hm.OGender,
                            Inn = hm.OwnerINN
                        };
                        UsersRepository.CreatePerson(own);
                    }

                    Hotel hotel = new Hotel()
                    {
                        Hoteldirectorinn = hm.DirectorINN,
                        Hotelownerinn = hm.OwnerINN,
                        Hotelinn = hm.HotelINN,
                        Hotelimage = (new Random(DateTime.Now.Millisecond).Next() % 5).ToString(),
                        Hotelpassword = hm.Password,
                        Hotelname = hm.HotelNam
                    };

                    HotelsRepository.CreateHotel(hotel);
                    HotelsRepository.SetCurrentHotelByInn(hm.HotelINN);
                    return RedirectToAction("Index", "HotelPage");
                }
            }
            return View(hm);
        }
    }
}
