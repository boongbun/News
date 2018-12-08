using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using News.BaseCore.Resources;
namespace News.Models
{
    public class AccountModel : IValidatableObject
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Remember { get; set; }
        public long LoginOptionID { get; set; }
        public bool IsRemoteLogOn { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(UserName))
                yield return new ValidationResult(GlobalResource.UserNameRequire, new[] { "UserName" });
            if (string.IsNullOrEmpty(Password))
                yield return new ValidationResult(GlobalResource.PasswordRequire, new[] { "Password" });
        }

    }
}