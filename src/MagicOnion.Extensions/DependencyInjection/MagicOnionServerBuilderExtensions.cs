using MagicOnion.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MagicOnion
{
    public static class MagicOnionServerBuilderExtensions
    {
        public static MagicOnionServerBuilder ConfigureMagicOnionOptions(this MagicOnionServerBuilder builder, Action<MagicOnionOptions> setupAction)
        {
            var _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            var _setupAction = setupAction ?? throw new ArgumentNullException(nameof(setupAction));

            builder.Services.Configure<MagicOnionOptions>(setupAction);

            return _builder;
        }

        public static MagicOnionServerBuilder ConfigureGrpcOptions(this MagicOnionServerBuilder builder, Action<GrpcOptions> setupAction)
        {
            var _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            var _setupAction = setupAction ?? throw new ArgumentNullException(nameof(setupAction));

            builder.Services.Configure<GrpcOptions>(setupAction);

            return _builder;
        }

        public static MagicOnionServerBuilder ConfigureGrpcOptions(this MagicOnionServerBuilder builder, IConfigurationSection configurationSection)
        {
            var _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            var _configurationSection = configurationSection ?? throw new ArgumentNullException(nameof(configurationSection));

            builder.Services.Configure<GrpcOptions>(_configurationSection);

            return _builder;
        }

        public static MagicOnionServerBuilder AddService<TService, TImplementation>(this MagicOnionServerBuilder builder)
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
