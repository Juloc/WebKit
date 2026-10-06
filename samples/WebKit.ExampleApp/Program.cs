using Microsoft.AspNetCore.Authentication.Cookies;
using WebKit.ExampleApp.Features.Catalog;
using WebKit.Web.DependencyInjection;
using WebKit.Web.Features;
using WebKit.Web.Identity;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddWebKitWeb();
builder.Services.AddWebKitFeatures(new CatalogFeature());
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Forbidden";
});
builder.Services.AddAuthorizationBuilder().AddPolicy("admin", policy => policy.RequireCapability("admin"));

WebApplication app = builder.Build();
app.UseExceptionHandler("/Error");
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
