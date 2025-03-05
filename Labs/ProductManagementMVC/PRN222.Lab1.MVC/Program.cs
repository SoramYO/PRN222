using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using PRN222.Lab1.Repository;
using PRN222.Lab1.Repository.UnitOfWork;
using PRN222.Lab1.Service.Services;

namespace PRN222.Lab1.MVC
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
			builder.Services.AddScoped<ICatergoryService, CategoryService>();
			builder.Services.AddScoped<IAccountService, AccountService>();
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			// Add authentication with cookies
			builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.Cookie.HttpOnly = true;
					options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
					options.LoginPath = "/Account/Login";
					options.AccessDeniedPath = "/Account/AccessDenied";
					options.SlidingExpiration = true;
				});
			builder.Services.AddControllersWithViews();


			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Account}/{action=Login}/{id?}");

			app.Run();
		}
	}
}
