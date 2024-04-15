using API.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.Internal;
using Refit;
using Web.Models;
using static iTextSharp.text.pdf.AcroFields;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly InterfaceClient I_Client;
        public HomeController(ILogger<HomeController> logger, InterfaceClient i_Client)
        {
            _logger = logger;
            I_Client = i_Client;
        }

        public IActionResult SaveToLocalStorage(string key, string value)
        {
            HttpContext.Session.SetString(key, value); // Сохранение значения в сессии
            return Ok();
        }
        public string GetFromLocalStorage(string key)
        {
            return HttpContext.Session.GetString(key);
        }

        public async Task<IActionResult> Index()
        {
            var item = await I_Client.GetAllCourses();
            return View(item);
        }

        public async Task<IActionResult> Authors()
        {
            var item = await I_Client.GetUserAuthors();
            return View(item);
        }

        public async Task<IActionResult> Course([FromQuery] int id)
        {
            var item = await I_Client.GetCourse(id);
            return View("Test", item);
        }
        public async Task<IActionResult> CourseDelete([FromQuery] int id)
        {
            var item = await I_Client.DeleteCourse(id);
            return View("Index");
        }

        public async Task<IActionResult> CourseAdd()
        {
            ViewBag.MyList = await I_Client.GetAllCategories();

            var userId = GetFromLocalStorage("user");
            if (userId != null)
            {
                ViewData["userId"] = userId;
                return View();
            }
            else
            {
                ViewData["userId"] = 1;
                return View();
            }
        }

        public IActionResult Registration()
        {
            return View();
        }
        public IActionResult Authorization()
        {
            return View();
        }

        public async Task<IActionResult> Login([FromForm] string userLogin, string userPassword)
        {
            var users = await I_Client.GetAllUsers();
            var user = users.FirstOrDefault(u => u.UserLogin == userLogin && u.UserPassword == userPassword);

            if (user != null)
            {
                SaveToLocalStorage("user", user.UserId.ToString());
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Неверный логин или пароль";
                return View("Authorization");
            }
        }
        public async Task<IActionResult> Profile()
        {
            var userId = GetFromLocalStorage("user");
            if (userId != null)
            {
                var item = await I_Client.GetUser(Convert.ToInt32(userId));
                var image = await I_Client.GetImageByte(item.UserImage);
                ViewData["image"] = image;
                return View(item);
            }
            else
            {
                var itemZero = await I_Client.GetUser(1);
                var image = await I_Client.GetImageByte(itemZero.UserImage);
                ViewData["image"] = image;
                return View(itemZero);
            }
        }

        [HttpPost] 
        public async Task<IActionResult> CreateUser([FromForm] UserDTO user, [FromForm] IFormFile image)
        {
            await I_Client.CreateUser(user);

            return RedirectToAction("Authorization");
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromForm] CourseDTO course)
        {
            await I_Client.CreateCourse(course);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile([FromForm] UserProfileDTO user)
        {
            var userId = GetFromLocalStorage("user");
            if (userId != null)
            {
                var userData = await I_Client.GetUser(Convert.ToInt32(userId));
                user.UserId = Convert.ToInt32(userId);
                user.UserCountry = userData.UserCountry;
                user.UserImage = userData.UserImage;
                user.UserRole = userData.UserRole;
                await I_Client.UpdateUser(user);
                return RedirectToAction("Profile");
            }
            else
            {
                var userData = await I_Client.GetUser(1);
                user.UserId = 1;
                user.UserCountry = userData.UserCountry;
                user.UserImage = userData.UserImage;
                user.UserRole = userData.UserRole;
                await I_Client.UpdateUser(user);
                return RedirectToAction("Profile");
            }
        }
    }
}
