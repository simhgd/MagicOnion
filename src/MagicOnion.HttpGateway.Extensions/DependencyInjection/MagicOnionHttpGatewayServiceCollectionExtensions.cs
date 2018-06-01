using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace MagicOnion.HttpGateway
{
    public static class MagicOnionHttpGatewayServiceCollectionExtensions
    {
        public static MagicOnionHttpGatewayBuilder AddMagicOnionHttpGateway(this IServiceCollection services)
        {
            var _service = services ?? throw new ArgumentNullException(nameof(services));

            _service.AddOptions();

            //_service.Where(p=>p.ServiceType == typeof(IOptions<MagicOnionServerOptions>)).FirstOrDefault()

            return new MagicOnionHttpGatewayBuilder { Services = _service };
        }
    }
}
