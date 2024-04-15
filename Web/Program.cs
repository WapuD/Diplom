using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Refit;
using API.Data;
using API.Models;
using Web.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRefitClient<InterfaceClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7237/api"));

builder.Services
    .AddDbContext<CourseContext>(Options =>
    Options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

builder.Services
    .AddControllersWithViews()
    .AddRazorRuntimeCompilation();

builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();
