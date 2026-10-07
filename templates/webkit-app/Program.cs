using WebKit.Web.DependencyInjection;
using WebKit.Web.Identity;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddWebKitWeb();
builder.Services.AddAuthorizationBuilder().AddPolicy("admin", policy => policy.RequireCapability("admin"));

WebApplication app = builder.Build();
app.UseExceptionHandler("/Error");
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.Run();
