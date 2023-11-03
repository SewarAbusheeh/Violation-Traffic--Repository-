namespace Violation_p2.Models
{
    public class CustomViewModel
    {
        public IEnumerable<Slider> DataFromTable1 { get; set; }
        public IEnumerable<Vehicle> DataFromTable2 { get; set; }
        public IEnumerable<Aboutu> DataFromTable3 { get; set; }
        public IEnumerable<Blog> DataFromTable4 { get; set; }
        public IEnumerable<Testimonial> DataFromTable5 { get; set; }
        public IEnumerable<User1> DataFromTable6 { get; set; }
        public IEnumerable<Violation> DataFromTable7 { get; set; }
        public  int CountCars { get; set; }
        public int CountViolations { get; set; }
        public int CountUsers { get; set; }
       public  string username { get; set; }

        public string UserProfileImage { get; set; }



    }
}
