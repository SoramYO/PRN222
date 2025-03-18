using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Service;

namespace PharmaceuticalManagement_StudentName.Pages.Product
{
	[Authorize(Roles = "2")]
	public class DeleteModel : PageModel
    {
		private readonly MedicineInformationService _medicineInformationService;

		public DeleteModel(MedicineInformationService medicineInformationService)
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

            var medicineinformation = await _medicineInformationService.GetMedicineInformationById(id);

			if (medicineinformation == null)
            {
                return NotFound();
            }
            else
            {
                MedicineInformation = medicineinformation;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicineinformation = await _medicineInformationService.GetMedicineInformationById(id);
			if (medicineinformation != null)
            {
                MedicineInformation = medicineinformation;
               _medicineInformationService.DeleteMedicineInformation(MedicineInformation);
            }

            return RedirectToPage("./Index");
        }
    }
}
