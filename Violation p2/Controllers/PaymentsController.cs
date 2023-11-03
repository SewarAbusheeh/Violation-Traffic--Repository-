using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;
using System;
using System.Net;
using System.Net.Mail;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using Violation_p2.Models;
using PdfSharpCore;
using System.Runtime.InteropServices;


using PuppeteerSharp;
using PuppeteerSharp.Media;
using PdfSharpCore.Pdf;

namespace Violation_p2.Controllers
{


    public class PaymentsController : Controller
    {
        private readonly ModelContext _context;
        private readonly string apiKey = "rzp_test_T6dRH8Gc0T6IWs";
        private readonly string apiSecret = "zUIeACGbdBqnsmwO9ZRMfjdz";

        private readonly IConverter _converter;
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibrary(string dllToLoad);

        public PaymentsController(ModelContext context , IConverter converter)
        {
            _context = context;
            _converter = converter;
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "libwkhtmltox.dll");
            LoadLibrary(path);
        }
        [HttpGet]
        public IActionResult InitiatePayment(int Id)
        {
            var violation = _context.Violations.Find(Id);
            if (violation == null)
            {
                return NotFound();
            }
            if (violation.Ispaid == true)
            {
                ViewBag.Message = "Your violation has been Paied";
            }

            var amount = (int)(violation.Fineamount.Value * 100); // Assuming your Fineamount is in currency's main unit, convert it to smallest unit e.g., rupees to paisa
            var currency = "INR"; // Assuming INR, change accordingly

            var model = new PaymentViewModel
            {
                ViolationId = Id,
                Amount = amount,
                Currency = currency
            };

            return View(model);
        }

    

        public IActionResult PaymentSuccess()
        {
            return View();
        }




      



        public byte[] GeneratePDF(string htmlContent)
        {
            var document = new PdfDocument();
            PdfGenerator.AddPdfPages(document, htmlContent, PageSize.A4);
            byte[] response;
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                response = ms.ToArray();
            }
            return response;
        }

        private async Task SendEmail(string toEmail, string subject, string htmlMessage, byte[] pdfAttachment)
        {
            using (var message = new MailMessage())
            {
                message.From = new MailAddress("sewarabusheeh@gmail.com");
                message.To.Add(new MailAddress(toEmail));
                message.Subject = subject;
                message.Body = htmlMessage;
                message.IsBodyHtml = true;

                // Attach the generated PDF
                using (var attachmentStream = new MemoryStream(pdfAttachment))
                {
                    var attachment = new Attachment(attachmentStream, "Invoice.pdf", "application/pdf");
                    message.Attachments.Add(attachment);

                    using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtpClient.Credentials = new NetworkCredential("sewarabusheeh@gmail.com", "lkkwzsvegzkffbxz ");
                        smtpClient.EnableSsl = true;

                        try
                        {
                            await smtpClient.SendMailAsync(message);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message); // or log the error to your logging system
                        }

                    }
                }
            }
        }

        public async Task<IActionResult> CompletePayment(string razorpayPaymentId, int violationId)
        {
            // ... (your existing code)
            var violation = await _context.Violations.FindAsync(violationId);
            if (violation == null)
            {
                return NotFound();
            }

            violation.RazorpayPaymentId = razorpayPaymentId;
            violation.Ispaid = true;

            _context.Update(violation);
            await _context.SaveChangesAsync();
            // Fetch email and send
            var res = await _context.Violations
                              .Include(v => v.Vehicle)       // Include related Vehicle entity
                              .ThenInclude(v => v.User)     // Then include the User related to that Vehicle
                              .FirstOrDefaultAsync(v => v.ViolationId == violationId);

            if (res == null)
            {
                return NotFound();
            }

            var userEmail = violation.Vehicle.User.Email;
            var info1 = violation.Vehicle.Type;
            var info2 = violation.Vehicle.Color;
            var info3 = violation.Vehicle.Model;
            var subject = "Payment Confirmation";
            var htmlContentForEmail = "<h1>Thank you for your payment.</h1> <h3>Your payment ID is:</h3> " + razorpayPaymentId + "<h3> Vehicle Type:</h3> " + info1 + "<h3> Vehicle Color:</h3> " + info2 + "<h3> Vehicle Model:</h3> " + info3;

            // Generate the PDF from HTML content
            byte[] pdfData = GeneratePDF(htmlContentForEmail);

            // Send email with PDF attachment
            await SendEmail(userEmail, subject, htmlContentForEmail, pdfData);

            // Redirect to the payment success page
            return RedirectToAction("PaymentSuccess");
        }


    }
}