using MagicOnion.Server;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MagicOnion
{
    public static class MagicOnionServiceCollectionExtensions
    {
        public static MagicOnionBuilder AddMagicOnion(this IServiceCollection services)
        {
            var _service = services ?? throw new ArgumentNullException(nameof(services));

            _service.AddOptions();

            _service.AddSingleton<MagicOnionServiceDefinitionDIWrapper>(new MagicOnionServiceDefinitionDIWrapper());

            return new MagicOnionBuilder { Services = _service };
        }

        public static MagicOnionBuilder AddMagicOnion(this IServiceCollection services, Action<MagicOnionOptions> configureOptions)
        {
            var _service = services ?? throw new ArgumentNullException(nameof(services));

            _service.AddMagicOnion();

            _service.Configure<MagicOnionOptions>(configureOptions);

            return new MagicOnionBuilder { Services = _service };
        }
    }
}
