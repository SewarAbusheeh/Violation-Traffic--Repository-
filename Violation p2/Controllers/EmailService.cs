//using Violation_p2.Models;

//namespace Violation_p2.Controllers
//{
//    public class EmailService : IHostedService, IDisposable
//    {
//        private Timer _timer;
       
//        private readonly ModelContext _context;
     

  
//        public EmailService(ModelContext context)
//        {
//            _context = context;
//        }

//        public Task StartAsync(CancellationToken cancellationToken)
//        {
//            _timer = new Timer(DoWork, null, TimeSpan.Zero,
//                TimeSpan.FromMinutes(5)); // For example, run every 5 minutes

//            return Task.CompletedTask;
//        }

//        private void DoWork(object state)
//        {
//            // Place your logic to fetch the email based on violation and send it here.
//            // Note: Make sure to handle exceptions as this runs in the background.
//            var user =  _context.Violations
//                      .Where(v => v.Id == violationId)
//                      .Select(v => v.User)
//                      .FirstOrDefaultAsync();

//            if (user == null)
//            {
//                return NotFound();
//            }

//            string payerEmail = user.Email;
//        }

//        public Task StopAsync(CancellationToken cancellationToken)
//        {
//            _timer?.Change(Timeout.Infinite, 0);
//            return Task.CompletedTask;
//        }

//        public void Dispose()
//        {
//            _timer?.Dispose();
//        }
//    }

//}
