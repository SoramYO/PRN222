using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using System.Security.Claims;
using Repository;
using Repository.Models;

namespace PharmaceuticalManagement_StudentName.Pages.Authenticate
{
	public class LoginModel : PageModel
	{
		private readonly StoreAccountService _storeAccountService;
		public LoginModel(StoreAccountService storeAccountService)
		{
			_storeAccountService = storeAccountService;
		}
		[BindProperty]
		public StoreAccount StoreAccount { get; set; }

		public string ErrorMessage { get; set; }

		public IActionResult OnGet()
		{
			if (User?.Identity?.IsAuthenticated == true)
			{
				return RedirectToPage("/Product/Index");
			}
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (User?.Identity?.IsAuthenticated == true)
			{
				return RedirectToPage("/Product/Index");
			}

			var memberAccount = await _storeAccountService.LoginAsync(StoreAccount.EmailAddress, StoreAccount.StoreAccountPassword);
			if (memberAccount == null)
			{
				ErrorMessage = "You do not have permission to do this function!";
				ModelState.AddModelError(string.Empty, ErrorMessage);
				return Page();
			}

			// Ki?m tra quy?n (MemberRole == 1 ho?c 2)
			if ((memberAccount.Role == 1 || memberAccount.Role == 4))
			{
				ErrorMessage = "You do not have permission to do this function!";
				ModelState.AddModelError(string.Empty, ErrorMessage);
				return Page();
			}

			var claims = new List<Claim>
							{
								new Claim(ClaimTypes.NameIdentifier, memberAccount.StoreAccountId.ToString()),
								new Claim(ClaimTypes.Email, memberAccount.EmailAddress),
								new Claim(ClaimTypes.Name, memberAccount.StoreAccountDescription)
							};

			if (memberAccount.Role.HasValue)
			{
				claims.Add(new Claim(ClaimTypes.Role, memberAccount.Role.Value.ToString()));
			}

			var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var principal = new ClaimsPrincipal(identity);

			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				principal,
				new AuthenticationProperties
				{
					IsPersistent = true,
					ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
				});

			return RedirectToPage("/Product/Index");
		}
	}
}

