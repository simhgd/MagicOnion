using Microsoft.Extensions.DependencyInjection;

namespace MagicOnion
{
    public class MagicOnionServerBuilder
    {
        public IServiceCollection Services { internal set; get; }
    }
}
