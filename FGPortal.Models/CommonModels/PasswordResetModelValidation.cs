using FluentValidation;

namespace FGPortal.Models
{
	public class PasswordResetModelValidation : AbstractValidator<PasswordResetModel>
	{
		public PasswordResetModelValidation()
		{
			When((PasswordResetModel x) => x.Page == "Reset", delegate
			{
				RuleFor((PasswordResetModel x) => x.Username).NotEmpty().WithMessage("Username is required");
			});
			When((PasswordResetModel x) => x.Page == "ResetPassword", delegate
			{
				RuleFor((PasswordResetModel x) => x.PasswordResetToken).NotEmpty().WithMessage("Password Reset Token is required");
				RuleFor((PasswordResetModel x) => x.Password).NotEmpty().WithMessage("Password is required");
				RuleFor((PasswordResetModel x) => x.PasswordConfirmation).NotEmpty().WithMessage("Password confirmation is required");
				RuleFor((PasswordResetModel x) => x.PasswordConfirmation).Equal((PasswordResetModel x) => x.Password).WithMessage("Password confirmation doesn't match password");
			});
		}
	}
}
