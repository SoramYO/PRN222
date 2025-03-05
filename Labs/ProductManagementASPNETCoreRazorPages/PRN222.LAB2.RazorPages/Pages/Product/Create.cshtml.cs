using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using PRN222.LAB2.RazorPages.SignalHub;
using PRN222.LAB2.Repository.Models;
using PRN222.LAB2.Service.Services;

namespace PRN222.LAB2.RazorPages.Pages.Product
{
    public class CreateModel : PageModel
    {
		private readonly IProductService _productService;
		private readonly ICategoryService _categoryService;
        private readonly IHubContext<SignalrServer> _hubContext;
		public CreateModel(IProductService productService, ICategoryService categoryService, IHubContext<SignalrServer> hubContext)
        {
			_productService = productService;
            _categoryService = categoryService;
			_hubContext = hubContext;
		}

        public async Task<IActionResult> OnGetAsync()
        {
        ViewData["CategoryId"] = new SelectList(await _categoryService.GetCategoriesAsync(), "CategoryId", "CategoryName");
            return Page();
        }

        [BindProperty]
        public PRN222.LAB2.Repository.Models.Product Product { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

			await _productService.AddProductAsync(Product);
            await _hubContext.Clients.All.SendAsync("LoadAllItems");

			return RedirectToPage("./Index");
        }
    }
}
