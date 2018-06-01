using MagicOnion.Server;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MagicOnion
{
    public static class MagicOnionBuilderExtensions
    {
        public static MagicOnionBuilder ConfigureMagicOnionOptions(this MagicOnionBuilder builder, Action<MagicOnionOptions> setupAction)
        {
            var _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            var _setupAction = setupAction ?? throw new ArgumentNullException(nameof(setupAction));

            builder.Services.Configure<MagicOnionOptions>(setupAction);

            return _builder;
        }

        public static MagicOnionBuilder AddService<TService, TImplementation>(this MagicOnionBuilder builder)
            where TService : class, IServiceMarker
            where TImplementation : class, TService
        {
            var _builder = builder ?? throw new ArgumentNullException(nameof(builder));

            _builder.Services.AddTransient<IServiceMarker, TImplementation>();
            _builder.Services.AddTransient<TService, TImplementation>();

            return _builder;
        }
    }
}
