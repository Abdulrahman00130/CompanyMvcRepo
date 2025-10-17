using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels
{
    public class UserEditViewModel
    {
        public string Id { get; set; }
        [RegularExpression("[a-zA-z]{1,15}")]
        public string FName { get; set; }

        [RegularExpression("[a-zA-z]{1,15}")]
        public string LName { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
    }
}
