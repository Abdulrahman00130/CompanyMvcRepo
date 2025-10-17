using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
