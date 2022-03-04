using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using FluentValidation.Attributes;

namespace FGPortal.Models
{
    [Validator(typeof(AuthenticationViewModelValidation))]
    public class AuthenticationViewModel
	{
		public string Name { get; set; }

		public string Username { get; set; }

		public string Password { get; set; }

		public string PasswordConfirmation { get; set; }

		public string RedirectTo { get; set; }

		public string Page { get; set; }
	}
}
