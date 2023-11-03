using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing;
using System.Net.Mail;
using System.Net;
using Violation_p2.Models;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using NuGet.Packaging.Licenses;

namespace Violation_p2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public HomeController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;

        }
    
        [HttpPost]
        public IActionResult Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return View(new List<Vehicle>()); // or however you handle an empty search
            }

            var results = _context.Vehicles
       .Include(v => v.User)  // Ensure User data is fetched too
       .Where(v =>
           v.Model.ToUpper().Contains(query.ToUpper()) ||
           v.Licenseplate.ToUpper().Contains(query.ToUpper()) ||
           v.Type.ToUpper().Contains(query.ToUpper()) ||
           v.User.Username.ToUpper().Contains(query.ToUpper()))  // Assumes User is a navigational property and Username is a field
       .ToList();

            if (results.IsNullOrEmpty())
            {
                ViewBag.error = "No Result Found";
            }
            return View(results);
        }

        [Authorize]
        private bool CarExists(int VehicleId)
        {
            return (_context.Vehicles?.Any(e => e.VehicleId == VehicleId)).GetValueOrDefault();
        }
        private bool LicenseplateExists(string Licenseplate)
        {
            return (_context.Vehicles?.Any(e => e.Licenseplate == Licenseplate)).GetValueOrDefault();
        }
        //   [Authorize]
        [HttpPost]
        public IActionResult AddUserCar([Bind("UserId", "VehicleId", "ImageFile", "Licenseplate", "Model", "Licenseexpirydate", "Type", "Color")] Vehicle vehicle1)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");

            if (!UserId.HasValue)
            {
                ViewBag.Error = "You Have To Login First!";
                return RedirectToAction("Login", "RegisterLogin");
            }

            if (CarExists(vehicle1.VehicleId) || LicenseplateExists(vehicle1.Licenseplate))
            {
                ViewBag.Error1 = "Your Car already exists!";
                //return RedirectToAction("Index", " <script>     alert('Your alert message here.');   </script>");// Return to the same view showing the error.
                return Content("Your Car already exists!");


            }

            // If car doesn't exist
            Vehicle vehicle = new Vehicle
            {
                User1Id = UserId.Value,
                Licenseplate = vehicle1.Licenseplate,
                Model = vehicle1.Model,
                Type = vehicle1.Type,
                Color = vehicle1.Color,
                Licenseexpirydate = vehicle1.Licenseexpirydate,
            };

            // Handle image upload
            if (vehicle1.ImageFile != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + vehicle1.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/ImagesTodisplay/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    vehicle1.ImageFile.CopyTo(fileStream);
                }
                vehicle.Imagepath = fileName;
            }

            _context.Add(vehicle);
            _context.SaveChanges();

            ViewBag.Message = "Your vehicle has been added.";
            return RedirectToAction("Index");
        }
       
      
        public IActionResult Index()
        {
            string username = HttpContext.Session.GetString("Username");
            //HttpContext.Session.SetString("userProfileImage", "someImage.jpg");
            //var test = HttpContext.Session.GetString("userProfileImage");
            var dataForTable1 = _context.Sliders.OrderByDescending(x => x.SliderId).Take(1);
            var dataForTable2 = _context.Vehicles
                                     .Where(vehicle => vehicle.Violations.Any()).OrderByDescending(z => z.VehicleId)
                                     .ToList();

            var dataForTable3 = _context.Aboutus.OrderByDescending(z => z.AboutuId).ToList();
            var dataForTable4 = _context.Blogs.OrderByDescending(z => z.BlogId).ToList();
            var dataForTable5 = _context.Testimonials.Where(x => x.Isapproved == true).OrderByDescending(z=>z.TestimonialId);
            var dataForTable7 = _context.Violations.ToList();
            var dataForTable8 = _context.User1s.Count();
            var CountCars = _context.Vehicles.Count();
            var CountViolations = dataForTable7.Count;
            var CountUsers = dataForTable8;
            var customViewModel = new CustomViewModel
            {
                DataFromTable1 = dataForTable1,
                DataFromTable2 = dataForTable2,
                DataFromTable3 = dataForTable3,
                DataFromTable4 = dataForTable4,
                DataFromTable5 = dataForTable5,
                CountCars = CountCars,
                CountViolations = CountViolations,
                CountUsers = CountUsers,
                username = username,
                //UserProfileImage= test
            };
            //if (vehicle1 == null)
            //{
            //    return RedirectToAction("AddUserCar");
            //}

            return View(customViewModel);
        }
        [HttpGet]
        public IActionResult Testimonial()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Testimonial(string Email, string UserName, Testimonial testimonial)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (ModelState.IsValid)
            {

                if (UserId != null)
                {

                    var existingUser = _context.User1s.FirstOrDefault(u => u.User1Id == UserId);
                    if (existingUser == null)
                    {
                        // Handle user not found case, for example:
                        ViewBag.err = "User not found.";
                        return View();
                    }

                    Testimonial testimonial1 = new Testimonial
                    {
                        Content = testimonial.Content,
                        Subject = testimonial.Subject,
                        Isapproved = false,
                        User = existingUser
                    };
                    _context.Testimonials.Add(testimonial1);
                    _context.SaveChanges();
                    ViewBag.Message = "Your message has been sent.";
                    return View();
                }
                return RedirectToAction("Login", "RegisterLogin");
            }
            ViewBag.err = "Please Fill The Complete Inforamtion ";
            return View();
        }
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact([Bind("Name,Subject,Message,Content,Email")] Contact contact)
        {

            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (ModelState.IsValid)
            {
                //        // obj mail 
                //        // كلاس يمثل رساله  




                var mail = new MailMessage();
                //        //  معطيات تسجيل الدخول بغرض ارسال الرسائل من خلاله يت تحويل الرسائل  
                //        // نتورك كردنشل ينتظر الايميل والباسورد
                //        //   الايميل الذي سوف نرسل من خلاله اللي رح اقوم بالارسال من خلاله 
                var Loginfo = new NetworkCredential("sewarabusheeh@gmail.com", "lkkwzsvegzkffbxz ");


                //        // مين هو المرسل 
                mail.From = new MailAddress(contact.Email);
                //        // الوجهه 
                mail.To.Add(new MailAddress("sewarabusheeh@gmail.com"));

                mail.Subject = contact.Subject;
                mail.IsBodyHtml = true;
                string body = "Name :" + contact.Name + "<br>" +
                               "Email: <br> <b> " + contact.Email + "<br>" +
                              "Subject:" + contact.Subject + "<br>" +
                               "Message: <b>" + contact.Message + "<br>";
                mail.Body = body;
                //     smtpClient بدي ارسل ايميل ب دوت نت عن طريق   واحدد الهوست 
                //        //the port of gmail is 587
                var smtpClient = new SmtpClient("smtp.gmail.com", 587);
                //        // الجميل يسمح بالوضع الامن   ؟؟ البيانات محميه عند تحويل  من البراوسر الى سيرفر
                //        // smtp deal with ssl
                smtpClient.EnableSsl = true;
                //        //معطيات الارسال 

                smtpClient.Credentials = Loginfo;
                smtpClient.Send(mail);
                contact.Contactdate = DateTime.Now;
                _context.Contacts.Add(contact);
                _context.SaveChanges();
                ViewBag.Message = "Your message has been sent.";
                return View();

            }

            ViewBag.err = "Please Fill The Complete Inforamtion ";
            return View();

        }

        public IActionResult SendNoti_Expirylicense()
        {
            DateTime currentDate = DateTime.Now.Date;
            Vehicle vehicle = new Vehicle();
            //vehicle.Licenseexpirydate= currentDate.AddYears(1);
            //DateTime expectedExpiryDate =_context.Vehicles.
            //   DateTime justTheDate = vehicle.Licenseexpirydate.Date

            bool isExpiringSoon = vehicle.Licenseexpirydate >= currentDate &&
                                   vehicle.Licenseexpirydate <= currentDate.AddDays(7);
            if (isExpiringSoon == true)
            {
                var mail = new MailMessage();
                var Loginfo = new NetworkCredential("sewarabusheeh@gmail.com", "lkkwzsvegzkffbxz ");


                //        // مين هو المرسل 
                mail.From = new MailAddress(vehicle.User.Email);
                //        // الوجهه 
                mail.To.Add(new MailAddress("sewarabusheeh@gmail.com"));

                mail.Subject = "LiencesNotifcation";
                mail.IsBodyHtml = true;
                string body = "Name :" + vehicle.User.Username + "<br>" +
                               "Email: <br> <b> " + vehicle.User.Email + "<br>" +
                              "Subject:" + "LiencesNotifcation" + "<br>" +
                               "Message: <b>" + vehicle.Model + vehicle.Color + vehicle.Type + vehicle.Licenseplate + "is gitteng Expierd " + "<br>";
                mail.Body = body;
                //     smtpClient بدي ارسل ايميل ب دوت نت عن طريق   واحدد الهوست 
                //        //the port of gmail is 587
                var smtpClient = new SmtpClient("smtp.gmail.com", 587);
                //        // الجميل يسمح بالوضع الامن   ؟؟ البيانات محميه عند تحويل  من البراوسر الى سيرفر
                //        // smtp deal with ssl
                smtpClient.EnableSsl = true;
                //        //معطيات الارسال 

                smtpClient.Credentials = Loginfo;
                smtpClient.Send(mail);



            }
            return NoContent();
        }
        public IActionResult ProfileUser(int? Id)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (Id == null)
            {
                return NotFound();
            }
            Vehicle vehicle = new Vehicle();
            //var check = _context.Vehicles.Where(x => x.User1Id == Id);
            var userWithVehicles = _context.User1s
                                   .Include(u => u.Vehicles)
                                   .FirstOrDefault(u => u.User1Id == Id.Value);
            if (userWithVehicles == null)
            {
                return View();
            }

            return View(userWithVehicles);
            //if (check.Any())
            //{

            //    return View(check.ToList());
            //}
            //return View();
        }
        public IActionResult DetailsCarUser(int? Id)
        {



            //int? UserId = HttpContext.Session.GetInt32("UserId");
            if (Id == null)
            {
                return NotFound();
            }
            Vehicle vehicle = new Vehicle();
            //var vehicleWithViolations = _context.Vehicles
            //                  .Include(v => v.Violations)
            //                  .FirstOrDefault(v => v.VehicleId == Id.Value);
            var vehicleDetails = _context.Vehicles
                         .Include(v => v.Violations)
                             .ThenInclude(violation => violation.Violationtype)
                         .FirstOrDefault(v => v.VehicleId == Id.Value);
            if (vehicleDetails == null)
            {
                return View();
            }

            return View(vehicleDetails);
            //if (check.Any())
            //{

            //    return View(check.ToList());
            //}
            //return View();
        }
        public IActionResult ProfileAdmin(int? Id)
        {
            // int? UserId = HttpContext.Session.GetInt32("UserId");
            if (Id == null)
            {
                return NotFound();
            }
            // Vehicle vehicle = new Vehicle();
            var check = _context.User1s.Where(x => x.User1Id == Id);
            if (check.Any())
            {
                return View(check);
            }
            return View();
        }
        public IActionResult Confirm_Testimonial(int id)
        {
            var res = _context.Testimonials.FirstOrDefault(x => x.TestimonialId == id);
            if (res == null)
            {
                return NotFound();
            }
            res.Isapproved = true;
            _context.SaveChanges();
            return RedirectToAction("Index", "testimonials");
        }



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.User1s == null)
            {
                return NotFound();
            }

            var user1 = await _context.User1s.FindAsync(id);
            if (user1 == null)
            {
                return NotFound();
            }

            return View(user1);
        }

        // POST: User1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("User1Id,Username,ImageFile,Password,Email")] User1 user1)
        {
            if (id != user1.User1Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {

                if (user1.ImageFile != null)
                {
                    string wwwRootPath = _webHostEnviroment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + user1.ImageFile.FileName;



                    string path = Path.Combine(wwwRootPath + "/ImagesTodisplay/" + fileName);



                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await user1.ImageFile.CopyToAsync(fileStream);
                    }



                    user1.Imagepath = fileName;
                }
                try
                {
                    user1.RoleId = 2;
                    _context.Update(user1);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!User1Exists(user1.User1Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Error = "Fill all the Requerment";
            return View();
        }
        private bool User1Exists(int id)
        {
            return (_context.User1s?.Any(e => e.User1Id == id)).GetValueOrDefault();
        }
    }
}