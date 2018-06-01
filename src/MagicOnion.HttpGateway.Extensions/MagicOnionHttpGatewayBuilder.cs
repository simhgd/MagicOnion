using Microsoft.Extensions.DependencyInjection;

namespace MagicOnion.HttpGateway
{
    public class MagicOnionHttpGatewayBuilder
    {
        public IServiceCollection Services { internal set; get; }
    }
}
