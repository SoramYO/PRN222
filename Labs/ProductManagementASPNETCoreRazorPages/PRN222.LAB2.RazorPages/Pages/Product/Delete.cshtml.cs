using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PRN222.LAB2.RazorPages.SignalHub;
using PRN222.LAB2.Repository.Models;
using PRN222.LAB2.Service.Services;

namespace PRN222.LAB2.RazorPages.Pages.Product
{
	[Authorize]
	public class DeleteModel : PageModel
    {
		private readonly IProductService _productService;
		private readonly IHubContext<SignalrServer> _hubContext;

		public DeleteModel(IProductService productService, IHubContext<SignalrServer> hubContext)
        {
			_productService = productService;
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

            var product = await _productService.GetProductByIdAsync(id.Value);

			if (product == null)
            {
                return NotFound();
            }
            else
            {
                Product = product;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductByIdAsync(id.Value);
			if (product != null)
            {
                Product = product;
				await _productService.DeleteProductAsync(product);
				await _hubContext.Clients.All.SendAsync("LoadAllItems");
			}

            return RedirectToPage("./Index");
        }
    }
}
