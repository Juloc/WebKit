using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebKit.Core.Time;
using WebKit.Web.Configuration;
using WebKit.Web.Identity;
using WebKit.Web.Notifications;

namespace WebKit.Web.DependencyInjection;

public static class WebKitServiceCollectionExtensions
{
    public static IServiceCollection AddWebKitWeb(this IServiceCollection services, Action<WebKitWebOptions>? configure = null)
    {
        if (configure is not null)
        {
            services.Configure(configure);
        }
        else
        {
            services.AddOptions<WebKitWebOptions>();
        }

        services.AddHttpContextAccessor();
        services.AddProblemDetails();
        services.TryAddSingleton<IClock, SystemClock>();
        services.TryAddSingleton<ICapabilityEvaluator, DefaultCapabilityEvaluator>();
        services.TryAddScoped<IUserContext, HttpUserContext>();
        services.TryAddScoped<IFlashMessageStore, TempDataFlashMessageStore>();
        services.AddSingleton<IAuthorizationHandler, CapabilityAuthorizationHandler>();
        return services;
    }
}
