using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PRN222.LAB2.RazorPages.SignalHub;
using PRN222.LAB2.Repository.Models;
using PRN222.LAB2.Service.Services;

namespace PRN222.LAB2.RazorPages.Pages.Product
{
	[Authorize]
	public class EditModel : PageModel
    {
		private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
		private readonly IHubContext<SignalrServer> _hubContext;
		public EditModel(IProductService productService, ICategoryService categoryService, IHubContext<SignalrServer> hubContext)
        {
			_productService = productService;
			_categoryService = categoryService;
            _hubContext = hubContext;

		}

        [BindProperty]
        public PRN222.LAB2.Repository.Models.Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
			Console.WriteLine("concac",id);

            var product =  await _productService.GetProductByIdAsync(id.Value);
			if (product == null)
            {
                return NotFound();
            }
            Product = product;
           ViewData["CategoryId"] = new SelectList(await _categoryService.GetCategoriesAsync(), "CategoryId", "CategoryName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

			try
            {
                await _productService.UpdateProductAsync(Product);
				await _hubContext.Clients.All.SendAsync("LoadAllItems");
			}
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.ProductId))
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

        private bool ProductExists(int id)
        {
            return _productService.GetProductByIdAsync(id) != null;
		}
    }
}
