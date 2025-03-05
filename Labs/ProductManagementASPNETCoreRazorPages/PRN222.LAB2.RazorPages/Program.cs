using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PRN222.LAB2.RazorPages.SignalHub;
using PRN222.LAB2.Repository.Models;
using PRN222.LAB2.Repository.UnitOfWork;
using PRN222.LAB2.Service.Services;

namespace PRN222.LAB2.RazorPages
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddDbContext<MyStoreContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			builder.Services.AddScoped<IProductService, ProductService>();
			builder.Services.AddScoped<ICategoryService, CategoryService>();
			builder.Services.AddScoped<IAccountService, AccountService>();
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			//builder.Services.AddSession(options =>
			//{
			//	options.IdleTimeout = TimeSpan.FromMinutes(20); // Set session timeout
			//	options.Cookie.HttpOnly = true; // For security
			//	options.Cookie.IsEssential = true; // Ensure session cookie is always created
			//});
			builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			.AddCookie(options =>
			{
				options.Cookie.HttpOnly = true;
				options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
				options.LoginPath = "/Login";
				options.AccessDeniedPath = "/AccessDenied";
				options.SlidingExpiration = true;
			});

			builder.Services.AddSignalR();


			builder.Services.AddRazorPages();
			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			//app.UseSession();
			app.MapHub<SignalrServer>("/signalrServer");

			app.UseAuthentication();

			app.UseAuthorization();

			app.MapRazorPages();

			app.Run();
		}
	}
}
