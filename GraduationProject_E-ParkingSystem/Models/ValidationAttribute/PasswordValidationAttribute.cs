using System.ComponentModel.DataAnnotations;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class PasswordValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var password = value as string;

        if (string.IsNullOrEmpty(password))
        {
            return new ValidationResult("Password is required.");
        }

        bool hasUpperCase = password.Any(char.IsUpper);
        bool hasLowerCase = password.Any(char.IsLower);
        bool hasDigit = password.Any(char.IsDigit);
        bool hasSymbol = password.Any(ch => !char.IsLetterOrDigit(ch));

		//if (!hasUpperCase)
		//    return new ValidationResult("Password must contain at least one uppercase letter.");
		//if (!hasLowerCase)
		//    return new ValidationResult("Password must contain at least one lowercase letter.");
		//if (!hasDigit)
		//    return new ValidationResult("Password must contain at least one digit.");
		//if (!hasSymbol)
		//    return new ValidationResult("Password must contain at least one special symbol.");
		// بدل من الوصف الطويل نستخدم رموز
		var errors = new List<string>();


		if (!hasUpperCase)
			errors.Add("uppercase");
		if (!hasLowerCase)
			errors.Add("lowercase");
		if (!hasDigit)
			errors.Add("digit");
		if (!hasSymbol)
			errors.Add("symbol");


		if (errors.Any())
		{
			string message = "Password missing: " + string.Join(", ", errors) + ".";
			return new ValidationResult(message);
		}


		return ValidationResult.Success;
    }
}
