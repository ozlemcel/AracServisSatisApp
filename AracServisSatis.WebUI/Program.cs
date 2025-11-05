using AracServisSatis.Areas.Writer.Models;
using AracServisSatis.Data;
using AracServisSatis.Entities.Concrete;
using AracServisSatis.Service.Abstract;
using AracServisSatis.Service.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient(typeof(IService<>), typeof(Service<>));
builder.Services.AddTransient<ICarService,CarService>();

builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<DatabaseContext>()
    .AddDefaultTokenProviders().AddErrorDescriber<TurkishIdentityErrorDescriber>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Admin/Account/Login";
    options.AccessDeniedPath = "/Admin/Account/AccessDenied";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();


app.MapStaticAssets();

app.MapControllerRoute(
           name: "admin",
           pattern: "{area:exists}/{controller=Main}/{action=Index}/{id?}"
         );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
