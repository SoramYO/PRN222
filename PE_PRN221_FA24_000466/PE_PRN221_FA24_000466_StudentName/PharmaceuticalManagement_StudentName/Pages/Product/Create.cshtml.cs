using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Models;
using Service;

namespace PharmaceuticalManagement_StudentName.Pages.Product
{
	[Authorize(Roles = "2")]
	public class CreateModel : PageModel
	{
		private readonly MedicineInformationService _medicineInformationService;

		public CreateModel(MedicineInformationService medicineInformationService)
		{
			_medicineInformationService = medicineInformationService;
		}

		public async Task<IActionResult> OnGet()
		{
			ViewData["ManufacturerId"] = new SelectList(await _medicineInformationService.GetManufacturer(), "ManufacturerId", "ManufacturerName");
			return Page();
		}

		[BindProperty]
		public MedicineInformation MedicineInformation { get; set; } = default!;

		// For more information, see https://aka.ms/RazorPagesCRUD.
		public async Task<IActionResult> OnPostAsync()
		{
			if (!ValidateActiveIngredients(MedicineInformation.ActiveIngredients, out string errorMessage))
			{
				ModelState.AddModelError("MedicineInformation.ActiveIngredients", errorMessage);
			}
			if (!ModelState.IsValid)
			{
				ViewData["ManufacturerId"] = new SelectList(await _medicineInformationService.GetManufacturer(), "ManufacturerId", "ManufacturerName");
				return Page();
			}

			_medicineInformationService.AddMedicineInformation(MedicineInformation);

			return RedirectToPage("./Index");
		}

		private bool ValidateActiveIngredients(string activeIngredients, out string errorMessage)
		{
			errorMessage = string.Empty;

			if (string.IsNullOrEmpty(activeIngredients))
			{
				errorMessage = "Active Ingredients is required";
				return false;
			}

			// Check length - greater than 10 characters
			if (activeIngredients.Length <= 10)
			{
				errorMessage = "Active Ingredients must be greater than 10 characters";
				return false;
			}

			// Check for special characters
			if (Regex.IsMatch(activeIngredients, @"[#@&\(\)]"))
			{
				errorMessage = "Active Ingredients cannot contain special characters such as #, @, &, ( or )";
				return false;
			}

			// Check that each word begins with a capital letter or number
			var words = activeIngredients.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			foreach (var word in words)
			{
				if (word.Length > 0 && !(char.IsUpper(word[0]) || char.IsDigit(word[0])))
				{
					errorMessage = "Each word in Active Ingredients must begin with a capital letter or a number";
					return false;
				}
			}

			return true;
		}
	}
}
