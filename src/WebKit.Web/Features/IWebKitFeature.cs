using Microsoft.Extensions.DependencyInjection;

namespace WebKit.Web.Features;

/// <summary>
/// The explicit registration boundary for an application feature.
/// </summary>
public interface IWebKitFeature
{
    void Register(IServiceCollection services);
}

public static class WebKitFeatureRegistration
{
    public static IServiceCollection AddWebKitFeatures(this IServiceCollection services, params IWebKitFeature[] features)
    {
        foreach (IWebKitFeature feature in features)
        {
            feature.Register(services);
        }

        return services;
    }
}
