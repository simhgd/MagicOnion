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

                var magicOnionSwaggerOptions = sp.GetService<IOptions<SwaggerOptions>>();
                var magicOnionServiceDefinitionDIWrapper = sp.GetService<MagicOnionServiceDefinitionDIWrapper>();

                if (magicOnionSwaggerOptions == null || magicOnionSwaggerOptions.Value == null)
                {
                    throw new ArgumentException(nameof(SwaggerOptions));
                }

                if (magicOnionServiceDefinitionDIWrapper == null || magicOnionServiceDefinitionDIWrapper.MagicOnionServiceDefinition == null)
                {
                    throw new ArgumentException(nameof(MagicOnionServiceDefinitionDIWrapper));
                }

                app.UseMagicOnionSwagger(magicOnionServiceDefinitionDIWrapper.MagicOnionServiceDefinition.MethodHandlers, magicOnionSwaggerOptions.Value);
            }

            return _app;
        }
    }
}
