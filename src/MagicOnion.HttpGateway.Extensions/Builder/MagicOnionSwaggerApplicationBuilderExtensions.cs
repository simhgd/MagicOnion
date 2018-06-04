using MagicOnion.HttpGateway.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace MagicOnion.HttpGateway
{
    public static class MagicOnionSwaggerApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMagicOnionSwagger(this IApplicationBuilder app)
        {
            var _app = app ?? throw new ArgumentNullException(nameof(app));

            using (var appScope = _app.ApplicationServices.CreateScope())
            {
                var sp = appScope.ServiceProvider;

                var magicOnionServiceDefinitionDIWrapper = sp.GetService<MagicOnionServiceDefinitionDIWrapper>();
                if (magicOnionServiceDefinitionDIWrapper?.MagicOnionServiceDefinition == null)
                {
                    throw new InvalidOperationException("Unable to find the required services. Please add all the required services by calling 'IServiceCollection.AddMagicOnionServer' inside the call to 'ConfigureServices(...)' in the application startup code.");
                }

                var magicOnionSwaggerOptions = sp.GetService<IOptions<SwaggerOptions>>();
                if (magicOnionSwaggerOptions?.Value == null)
                {
                    throw new InvalidOperationException(nameof(SwaggerOptions));
                }

                

                app.UseMagicOnionSwagger(magicOnionServiceDefinitionDIWrapper.MagicOnionServiceDefinition.MethodHandlers, magicOnionSwaggerOptions.Value);
            }

            return _app;
        }
    }
}
