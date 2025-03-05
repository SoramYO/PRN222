using BOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repos;

namespace PharmaceuticalManagement_NgoXuanSon.Pages.PharmaceuticalManagementPage
{
	public class IndexModel : PageModel
	{

		[BindProperty(SupportsGet = true)]
		public string SearchTerm { get; set; } = default!;

		public int PageIndex { get; set; } = 1;

		public int TotalPages { get; set; }
		public int PageSize { get; set; } = 3;

		private readonly IMedicineRepo _medicineRepo;

		public IndexModel(IMedicineRepo medicineRepo)
		{
			_medicineRepo = medicineRepo;
		}

		public IList<MedicineInformation> MedicineInformation { get; set; } = default!;

		public async Task OnGetAsync(int pageIndex = 1)
		{
			var result = await _medicineRepo.GetList(SearchTerm, pageIndex, 3);
			PageIndex = result.PageIndex;
			TotalPages = result.TotalPages;
			MedicineInformation = result.MedicineInformation;
		}
	}
}
