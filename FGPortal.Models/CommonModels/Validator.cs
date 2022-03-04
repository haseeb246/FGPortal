using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGPortal.Models
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter)]
	public class ValidatorAttribute : Attribute
	{
		public Type ValidatorType { get; private set; }

		public ValidatorAttribute(Type validatorType)
		{
			ValidatorType = validatorType;
		}
	}
}
