using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PRN222.Lab1.Repository.Models;
using PRN222.Lab1.Service.Services;
using System.Security.Claims;

namespace PRN222.Lab1.MVC.Controllers
{
	public class AccountController : Controller
	{
		private readonly IAccountService _accountService; // Inject your account service

		public AccountController(IAccountService accountService)
		{
			_accountService = accountService;
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(AccountMember model)
		{
			ModelState.Remove("FullName"); // Remove validation for FullName
			ModelState.Remove("MemberRole");
			ModelState.Remove("MemerId");
			if (ModelState.IsValid)
			{
				Console.WriteLine(model.EmailAddress);
				var user = _accountService.GetAccountByEmail(model.EmailAddress);

				if (user != null && user.MemberPassword == model.MemberPassword)
				{
					var claims = new List<Claim>
									{
										new Claim(ClaimTypes.NameIdentifier, user.MemerId),
										new Claim(ClaimTypes.Name, user.FullName),
										new Claim(ClaimTypes.Email, user.EmailAddress)
									};

					// Add role if available
					if (user.MemberRole.HasValue)
					{
						claims.Add(new Claim(ClaimTypes.Role, user.MemberRole.Value.ToString()));
					}

					var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
					var principal = new ClaimsPrincipal(identity);

					// Sign in with the principal
					await HttpContext.SignInAsync(
						CookieAuthenticationDefaults.AuthenticationScheme,
						principal,
						new AuthenticationProperties
						{
							IsPersistent = true, // Remember me functionality
							ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
						});

					return RedirectToAction("Index", "Products");
				}
				else
				{
					ModelState.AddModelError("", "Invalid username or password.");
				}
			}

			return View(model);
		}

		public IActionResult Logout()
		{
			HttpContext.Session.Clear(); // Clear session data
			return RedirectToAction("Login");
		}
	}

}
