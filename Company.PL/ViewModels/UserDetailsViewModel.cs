using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels
{
    public class UserDetailsViewModel
    {
        public string Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}
