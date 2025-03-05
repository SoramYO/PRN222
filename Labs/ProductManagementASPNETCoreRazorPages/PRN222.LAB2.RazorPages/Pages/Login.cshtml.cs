using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222.LAB2.Repository.Models;
using PRN222.LAB2.Service.Services;
using System.Security.Claims;

namespace ProductManagementRazorPages.Pages
{
	public class LoginModel : PageModel
	{
		private readonly IAccountService _accountService;

		public LoginModel(IAccountService accountService)
		{
			_accountService = accountService;
		}

		[BindProperty]
		public AccountMember AccountMember { get; set; }

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

			var memberAccount = await _accountService.GetAccountByEmailAsync(AccountMember.EmailAddress);
			if (memberAccount == null || memberAccount.MemberPassword != AccountMember.MemberPassword)
			{
				ErrorMessage = "You do not have permission to do this function!";
				ModelState.AddModelError(string.Empty, ErrorMessage);
				return Page();
			}

			// Ki?m tra quy?n (MemberRole == 1 ho?c 2)
			if (!(memberAccount.MemberRole == 1 || memberAccount.MemberRole == 2))
			{
				ErrorMessage = "You do not have permission to do this function!";
				ModelState.AddModelError(string.Empty, ErrorMessage);
				return Page();
			}

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, memberAccount.MemerId),
				new Claim(ClaimTypes.Name, memberAccount.FullName),
				new Claim(ClaimTypes.Email, memberAccount.EmailAddress)
			};

			if (memberAccount.MemberRole.HasValue)
			{
				claims.Add(new Claim(ClaimTypes.Role, memberAccount.MemberRole.Value.ToString()));
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
