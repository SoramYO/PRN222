using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Models;
using Service;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddRazorPages();

		builder.Services.AddDbContext<Sp25PharmaceuticalDbContext>(options =>
			options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));
		builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
		builder.Services.AddScoped<StoreAccountService>();
		builder.Services.AddScoped<MedicineInfomationService>();

		builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			.AddCookie(options =>
			{
				options.Cookie.HttpOnly = true;
				options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
				options.LoginPath = "/Login";
				options.AccessDeniedPath = "/Home";
				options.SlidingExpiration = true;
			});
		builder.Services.AddSignalR();
		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Error");
			app.UseHsts();
		}


		app.UseHttpsRedirection();
		app.UseStaticFiles();
		app.UseRouting();
		app.UseAuthentication();
		app.UseAuthorization();
		app.MapRazorPages();
		app.Run();
	}
}
