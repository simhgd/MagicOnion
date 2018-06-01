using MagicOnion.HttpGateway.Swagger;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MagicOnion.HttpGateway
{
    public static class MagicOnionSwaggerBuilderExtensions
    {
        public static MagicOnionSwaggerBuilder AddMagicOnionSwaggerOptions(this MagicOnionSwaggerBuilder builder, Action<SwaggerOptions> setupAction)
        {
            var _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            var _setupAction = setupAction ?? throw new ArgumentNullException(nameof(setupAction));

            builder.Services.Configure<SwaggerOptions>(setupAction);

            return _builder;
        }
    }
}
