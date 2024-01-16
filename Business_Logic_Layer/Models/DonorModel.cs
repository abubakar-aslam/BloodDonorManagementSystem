using System.ComponentModel.DataAnnotations;

namespace Business_Logic_Layer.Models
{
    public class DonorModel
    {
        public int DonorId { get; set; }
        public string DonorName { get; set;}
        [Range(1,80,ErrorMessage ="Enter Age Between 1 To 80")]
        public int DonorAge { get; set;}
        public string BloodType { get; set; }
        public string ContactNumber { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", ErrorMessage ="Please Enter Valid Email ")]
        public string EmailAddress { get; set; }
        public string Address { get; set; }
    }
}
