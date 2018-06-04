using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace MagicOnion
{
    public static class MagicOnionServiceCollectionExtensions
    {
        public static MagicOnionServerBuilder AddMagicOnionServer(this IServiceCollection services)
        {
            var _service = services ?? throw new ArgumentNullException(nameof(services));

            _service.AddOptions();

            _service.AddSingleton<MagicOnionServiceDefinitionDIWrapper>(new MagicOnionServiceDefinitionDIWrapper());

            return new MagicOnionServerBuilder { Services = _service };
        }

        public static MagicOnionServerBuilder AddMagicOnionServer(this IServiceCollection services, Action<GrpcOptions> configureOptions)
        {
            var _service = services ?? throw new ArgumentNullException(nameof(services));

            _service.AddMagicOnionServer();

            _service.Configure<GrpcOptions>(configureOptions);

            return new MagicOnionServerBuilder { Services = _service };
        }

        public static MagicOnionServerBuilder AddMagicOnionServer(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            var _service = services ?? throw new ArgumentNullException(nameof(services));

            _service.AddMagicOnionServer();

            _service.Configure<GrpcOptions>(configurationSection);

            return new MagicOnionServerBuilder { Services = _service };
        }

        public static MagicOnionClientBuilder AddMagicOnionClient(this IServiceCollection services)
        {
            var _service = services ?? throw new ArgumentNullException(nameof(services));

            _service.AddOptions();

            return new MagicOnionClientBuilder { Services = _service };
        }

        public static MagicOnionClientBuilder AddMagicOnionClient(this IServiceCollection services, Action<GrpcOptions> configureOptions)
        {
            var _service = services ?? throw new ArgumentNullException(nameof(services));

            _service.AddMagicOnionClient();

            _service.Configure<GrpcOptions>(configureOptions);

            return new MagicOnionClientBuilder { Services = _service };
        }
    }
}
