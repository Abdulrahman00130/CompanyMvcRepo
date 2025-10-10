using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public IList<string> Roles { get; set; }
    }
}
