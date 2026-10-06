using Microsoft.Extensions.DependencyInjection;
using WebKit.Web.Features;

namespace WebKitFeature;

public sealed class WebKitFeatureFeature : IWebKitFeature
{
    public void Register(IServiceCollection services)
    {
        // Register feature services here.
    }
}
