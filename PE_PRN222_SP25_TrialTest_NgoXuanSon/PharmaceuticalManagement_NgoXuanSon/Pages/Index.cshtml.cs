using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Models;
using System.Security.Claims;
using Service;

namespace PharmaceuticalManagement_NgoXuanSon.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

	private readonly StoreAccountService _storeAccountService;
	public IndexModel(StoreAccountService storeAccountService)
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

		var memberAccount = _storeAccountService.Login(StoreAccount.EmailAddress, StoreAccount.StoreAccountPassword);
		if (memberAccount == null)
		{
			ErrorMessage = "You do not have permission to do this function!";
			ModelState.AddModelError(string.Empty, ErrorMessage);
			return Page();
		}

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