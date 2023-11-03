using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Violation_p2.Models;

namespace Violation_p2.Controllers
{
    public class RegisterLoginController : Controller
    {

        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public RegisterLoginController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(User1 user)
        {
            ModelState.Remove("Imagepath");
            ModelState.Remove("ImageFile");
           
            if (ModelState.IsValid)
            {

                // Check if the email already exists in the database
                var existingUser = _context.User1s.FirstOrDefault(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email already exists.");
                    return View(user); // Return to the registration view with an error
                }

                user.RoleId = 2;

                user.Imagepath = "default-avatar.png";

                _context.User1s.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View();
        
        }

           
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
   
        public IActionResult Login(string Email, string Password)
        {
            User1 user = new User1();


            var auth = _context.User1s.Where(x => x.Email == Email &&
              x.Password == Password).SingleOrDefault();
            if (auth != null)
            {
                switch (auth.RoleId)
                {
                    case 1:
                        HttpContext.Session.SetString("AdminName", auth.Username);
                        HttpContext.Session.SetInt32("UserIdforAdmin", auth.User1Id);
                        return RedirectToAction("Index", "Admin", new { userId = auth.User1Id });
                    case 2:
                        HttpContext.Session.SetString("Username", auth.Username);
                      
                        Debug.WriteLine(auth.Imagepath);
                        HttpContext.Session.SetString("userProfileImage", auth.Imagepath);

                        // Set a breakpoint here and check the value of testValue.

                        HttpContext.Session.SetInt32("UserId", auth.User1Id); 

                        return RedirectToAction("Index", "Home");
                }


            }
            ViewBag.ERROR = "Invaild Email or Passwoed";
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            // Clear all cookies
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            return RedirectToAction("Index" ,"Home");
        }
    }
}
