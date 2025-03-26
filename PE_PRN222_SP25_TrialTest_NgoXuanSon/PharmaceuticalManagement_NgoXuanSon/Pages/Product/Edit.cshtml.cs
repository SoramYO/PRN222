using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Service;

namespace PharmaceuticalManagement_NgoXuanSon.Pages.Product
{
	[Authorize(Roles = "2")]
	public class EditModel : PageModel
    {
		private readonly MedicineInfomationService _medicineInformationService;

		public EditModel(MedicineInfomationService context)
        {
			_medicineInformationService = context;
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
    }
}
