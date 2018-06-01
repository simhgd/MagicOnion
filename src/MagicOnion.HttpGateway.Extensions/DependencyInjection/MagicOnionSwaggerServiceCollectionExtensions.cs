using Microsoft.Extensions.DependencyInjection;
using System;


namespace MagicOnion.HttpGateway.Extensions.DependencyInjection
{
    public static class MagicOnionSwaggerServiceCollectionExtensions
    {
        public static MagicOnionSwaggerBuilder AddMagicOnionSwagger(this IServiceCollection services)
        {
            var _service = services ?? throw new ArgumentNullException(nameof(services));

            _service.AddOptions();

            return new MagicOnionSwaggerBuilder { Services = _service };
        }
    }
}
