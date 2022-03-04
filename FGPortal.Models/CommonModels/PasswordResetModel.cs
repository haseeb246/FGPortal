//using FluentValidation.Attributes;

namespace FGPortal.Models
{
    [Validator(typeof(PasswordResetModelValidation))]
    public class PasswordResetModel
	{
		public string PasswordResetToken { get; set; }

		public string Username { get; set; }

		public string Password { get; set; }

		public string PasswordConfirmation { get; set; }

		public string Page { get; set; }

		public bool EmailSent { get; set; }

		public bool PasswordChanged { get; set; }
	}
}
