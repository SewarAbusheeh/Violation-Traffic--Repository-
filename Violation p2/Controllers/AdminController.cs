using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Data;
using System.Net;
using System.Net.Mail;
using Violation_p2.Models;

namespace Violation_p2.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;

        private readonly IWebHostEnvironment _webHostEnviroment;

        public AdminController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }
        [Route("Admin/Index/{userId:int}")] // Ensure the route template matches the parameter
        public IActionResult Index(int UserId)
        {
            int? User1Id = HttpContext.Session.GetInt32("UserId");
            ViewBag.Name = HttpContext.Session.GetString("AdminName");
            ViewBag.VehiclesCount = _context.Vehicles.Count();
            ViewBag.ViolationsCount = _context.Violations.Count();
            ViewBag.ViolationtypesCount = _context.Violationtypes.Count();
            ViewBag.UsersCount = _context.User1s.Count();
            ViewBag.TestimonialsCount = _context.Testimonials.Count();
            ViewBag.ContactsCount = _context.Contacts.Count();
            ViewData["UserImagePath"] = HttpContext.Session.GetString("Imagepath");  // Set this according to your application logic.
            //var res = _context.User1s.Where(x=>x.User1Id == UserId).FirstOrDefault();
            var AdminInfo = _context.User1s.Where(x => x.User1Id == UserId).FirstOrDefault();
            if (AdminInfo == null)
            {
                // Handle the case when the user is not found. Maybe return an error view or redirect.
                return NotFound();
            }
            // ViewBag.AvgProductPric = _context.Products.Sum(x => x.Price) / _context.Products.Count();
            return View(AdminInfo);
        }
        //[HttpGet]
        //public IActionResult Search(DateTime? StartTime ,DateTime? EndTime )
        //{

        //}
      
        [HttpGet]
        public IActionResult Report()
        {

            return View();
        }
        //[HttpPost]
        //public IActionResult Report(int id)
        //{

        //    return View();
        //}
        [HttpPost]
        public IActionResult Search(DateTime? StartTime, DateTime? EndTime)
        {
            var ResultViolation = _context.Violations
         .Include(v => v.Violationtype)
        
         .Include(v => v.Vehicle)
         .OrderByDescending(x => x.VehicleId)
         .ToList();

            Violation V1 = new Violation();
            if (StartTime == null && EndTime == null)
            {
                return View(ResultViolation);
            }
            else if (StartTime != null && EndTime == null)

            {
                var res = StartTime.Value.Date;
                var Result = _context.Violations.Where(x => x.Violationdate >= res);
                return View(Result);
            }
            else if (StartTime == null && EndTime != null)
            {
                var res = EndTime.Value.Date;
                var Result = _context.Violations.Where(x => x.Violationdate <= res);
                return View(Result);
            }
            else
            {
                var res = StartTime.Value.Date;
                var res1 = EndTime.Value.Date;
                var Result = _context.Violations.Where(x => x.Violationdate >= res && x.Violationdate <= res1);
                return View(Result);
            }
        }

       
        public IActionResult MonthlyReport(int year, int month)
        {
            // Check the year and month values first to ensure they're within the valid range
            if (year < 1 || year > 9999 || month < 1 || month > 12)
            {
                // Return an error view or error response
                // You can also log the error details
                return BadRequest("Year or month is out of range.");
            }

            try
            {
                DateTime start = new DateTime(year, month, 1);
                DateTime end = start.AddMonths(1).AddDays(-1);

                var count = _context.Violations
                    .Include(v => v.Vehicle)
                        .ThenInclude(v => v.User)
                    .Include(v => v.Violationtype) // Including ViolationType
                    .Where(v => v.Violationdate >= start && v.Violationdate <= end)
                    .ToList();

                return View(count); // send the count to the view
            }
            catch (ArgumentOutOfRangeException ex)
            {
                // Handle the case where the resulting date is out of range
                // Log the details of the exception
                // Return an error view or error response
                return BadRequest("The provided date range is invalid.");
            }
        }


        public IActionResult AnnualReport(int year)
        {
            if (year < 1 || year > 9999)
            {
                // Handle the invalid year value (e.g., return an error view or redirect).
                return View();
            }
            DateTime start = new DateTime(year, 1, 1);
            DateTime end = new DateTime(year, 12, 31);

            var count = _context.Violations
      .Include(v => v.Vehicle)
          .ThenInclude(v => v.User)
      .Include(v => v.Violationtype) // Including ViolationType
      .Where(v => v.Violationdate >= start && v.Violationdate <= end)
      .ToList();

            return View(count);
        }

        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.User1s == null)
        //    {
        //        return NotFound();
        //    }

        //    var user1 = await _context.User1s.FindAsync(id);
        //    if (user1 == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user1);
        //}

        //// POST: User1/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("User1Id,Username,Password,Email,ImageFile")] User1 user1)
        //{
        //    if (id != user1.User1Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        if (user1.ImageFile != null)
        //        {
        //            string wwwRootPath = _webHostEnviroment.WebRootPath;
        //            string fileName = Guid.NewGuid().ToString() + user1.ImageFile.FileName;



        //            string path = Path.Combine(wwwRootPath + "/ImagesTodisplay/" + fileName);



        //            using (var fileStream = new FileStream(path, FileMode.Create))
        //            {
        //                await user1.ImageFile.CopyToAsync(fileStream);
        //            }



        //            user1.Imagepath = fileName;
        //        }
        //        try
        //        {
        //            _context.Update(user1);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!User1Exists(user1.User1Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    //ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", user1.RoleId);
        //    return View(user1);
        //}
        private bool User1Exists(int Id)
        {
            return (_context.User1s?.Any(e => e.User1Id == Id)).GetValueOrDefault();
        }
        public IActionResult SendLicenseExpiryNotifications()
        {

           
            // 1. Fetch the vehicles with licenses expiring soon.
            var vehiclesWithExpiringLicenses = _context.Vehicles
                .Include(v => v.User)
                .Where(v => v.Licenseexpirydate.Value.Date == DateTime.UtcNow.AddDays(1).Date)
                .ToList();

            // 2. For each of those vehicles, fetch the corresponding user details and send an email.
            // 2. Send Emails
            foreach (var vehicle in vehiclesWithExpiringLicenses)
            {
                var user = vehicle.User;

                // 3. Generate and send the notification email for that user and their vehicle.
                string body = $"Name: {user.Username}<br>" +
                              $"Email: <b>{user.Email}</b><br>" +
                              $"Subject: License Notification<br>" +
                              $"Message: <b>Vehicle <h1>{vehicle.Model}</h1> + <h1> {vehicle.Color}</h1> <h1>{vehicle.Type} </h1> with license plate  <h1>{vehicle.Licenseplate} </h1> is getting expired tomorrow</b><br>";

                SendEmail(user.Email, "License Expiry Notice", body);
            }


            // Redirect to home or dashboard after sending notifications.
            return View(vehiclesWithExpiringLicenses);
        }

        // 3. Email Sending Utility

        private void SendEmail(string to, string subject, string body)
        {
            var mail = new MailMessage();

            // Credentials for sending the email
            var logInfo = new NetworkCredential("sewarabusheeh@gmail.com", "lkkwzsvegzkffbxz");

            // From (sender's email) and To (recipient's email) details
            mail.From = new MailAddress("sewarabusheeh@gmail.com");  // Sender's email
            mail.To.Add(new MailAddress(to));  // Recipient's email

            // Email subject
            mail.Subject = subject;

            // Email body
            mail.Body = body;
            mail.IsBodyHtml = true;

            // Setting up the SMTP client
            using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.EnableSsl = true;  // Secure connection
                smtpClient.Credentials = logInfo;

                // Sending the email
                smtpClient.Send(mail);
            }
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.User1s == null)
            {
                return NotFound();
            }

            var user = await _context.User1s.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("User1Id,Username,Password,Email,ImageFile")] User1 user1)
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
                    user1.RoleId = 1;
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
                return RedirectToAction(nameof(Index) , "User1Id");
            }

            return View(user1);
        }


        [HttpPost]
        public async Task<IActionResult> SearchUser(string query)
        {



            var users = await _context.User1s
      .Where(u => u.Username.ToUpper().Contains(query.ToUpper())
               || u.Username.ToUpper().StartsWith(query.ToUpper())
               || u.Username.ToUpper().EndsWith(query.ToUpper()))
      .ToListAsync();


            if (users == null || !users.Any())
            {
                return NotFound();
            }

            //return View("Index" ,"User1",user);
            return View(users);
        }
    }
}
