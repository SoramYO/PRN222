using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Service;

namespace PharmaceuticalManagement_StudentName.Pages.Product
{
	[Authorize(Roles = "2")]
	public class EditModel : PageModel
    {
		private readonly MedicineInformationService _medicineInformationService;

		public EditModel(MedicineInformationService medicineInformationService)
        {
			_medicineInformationService = medicineInformationService;
        }

        [BindProperty]
        public MedicineInformation MedicineInformation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicineinformation =  await _medicineInformationService.GetMedicineInformationById(id);

			if (medicineinformation == null)
            {
                return NotFound();
            }
            MedicineInformation = medicineinformation;
           ViewData["ManufacturerId"] = new SelectList(await _medicineInformationService.GetManufacturer(), "ManufacturerId", "ManufacturerName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
			if (!ValidateActiveIngredients(MedicineInformation.ActiveIngredients, out string errorMessage))
			{
				ModelState.AddModelError("MedicineInformation.ActiveIngredients", errorMessage);
			}
			if (!ModelState.IsValid)
            {
                return Page();
            }

			try
            {
                _medicineInformationService.UpdateMedicineInformation(MedicineInformation);
			}
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicineInformationExists(MedicineInformation.MedicineId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MedicineInformationExists(string id)
        {
            return _medicineInformationService.GetMedicineInformationById(id) != null;

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
