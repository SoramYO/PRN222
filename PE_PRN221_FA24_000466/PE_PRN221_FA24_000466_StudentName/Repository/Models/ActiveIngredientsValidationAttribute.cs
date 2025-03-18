using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Repository.Models
{
	public class ActiveIngredientsValidationAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value == null)
				return new ValidationResult("Active Ingredients is required");

			var activeIngredients = value.ToString();

			// Check length - greater than 10 characters
			if (activeIngredients.Length <= 10)
				return new ValidationResult("Active Ingredients must be greater than 10 characters");

			// Check for special characters
			if (Regex.IsMatch(activeIngredients, @"[#@&\(\)]"))
				return new ValidationResult("Active Ingredients cannot contain special characters such as #, @, &, ( or )");

			// Check that each word begins with a capital letter or number
			var words = activeIngredients.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			foreach (var word in words)
			{
				if (word.Length > 0 && !(char.IsUpper(word[0]) || char.IsDigit(word[0])))
					return new ValidationResult("Each word in Active Ingredients must begin with a capital letter or a number");
			}

			return ValidationResult.Success;
		}
	}
}
