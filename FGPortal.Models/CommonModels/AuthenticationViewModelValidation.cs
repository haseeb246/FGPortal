using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGPortal.Models
{
	public class AuthenticationViewModelValidation : AbstractValidator<AuthenticationViewModel>
	{
		public AuthenticationViewModelValidation()
		{
			When((AuthenticationViewModel x) => x.Page != "Restore", delegate
			{
				RuleFor((AuthenticationViewModel x) => x.Password).NotEmpty().WithMessage("Password is required");
			});
			When((AuthenticationViewModel x) => x.Page == "Signup", delegate
			{
				RuleFor((AuthenticationViewModel x) => x.Name).NotEmpty().WithMessage("Full name is required");
			});
			When((AuthenticationViewModel x) => x.Page == "Signin" || x.Page == "Signup" || x.Page == "Restore", delegate
			{
				RuleFor((AuthenticationViewModel x) => x.Username).NotEmpty().WithMessage("Username is required");
			});
			When((AuthenticationViewModel x) => x.Page == "ChangePassword" || x.Page == "Signup", delegate
			{
				RuleFor((AuthenticationViewModel x) => x.PasswordConfirmation).NotEmpty().WithMessage("Password confirmation is required");
				RuleFor((AuthenticationViewModel x) => x.PasswordConfirmation).Equal((AuthenticationViewModel x) => x.Password).WithMessage("Password confirmation doesn't match password");
			});
		}
	}
}
