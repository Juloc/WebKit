using Microsoft.Extensions.DependencyInjection;
using WebKit.Web.Features;

namespace WebKit.ExampleApp.Features.Catalog;

public sealed class CatalogFeature : IWebKitFeature
{
    public void Register(IServiceCollection services) => services.AddSingleton<IProductCatalog, InMemoryProductCatalog>();
}
