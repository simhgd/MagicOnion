using Grpc.Core;
using MagicOnion.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace MagicOnion
{
    public static class MagicOnionApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMagicOnion(this IApplicationBuilder app)
        {
            var _app = app ?? throw new ArgumentNullException(nameof(app));

            using (var appScope = _app.ApplicationServices.CreateScope())
            {
                var sp = appScope.ServiceProvider;

                var magicOnionServiceDefinitionDIWrapper = sp.GetRequiredService<MagicOnionServiceDefinitionDIWrapper>();

                var magicOnionOptions = sp.GetService<IOptions<MagicOnionOptions>>();
                var magicOnionServerOptions = sp.GetService<IOptions<MagicOnionServerOptions>>();
                var magicOnionServices = sp.GetServices<IServiceMarker>();
                var magicOnionServicesType = magicOnionServices.Select(s => s.GetType());
                magicOnionServiceDefinitionDIWrapper.MagicOnionServiceDefinition = MagicOnionEngine.BuildServerServiceDefinition(magicOnionServicesType, magicOnionOptions.Value);

                var services = sp.GetService<IServiceCollection>();

                var server = new Grpc.Core.Server
                {
                    Services = { magicOnionServiceDefinitionDIWrapper.MagicOnionServiceDefinition },
                    Ports = { new ServerPort(magicOnionServerOptions.Value.Host, magicOnionServerOptions.Value.Port, magicOnionServerOptions.Value.Credentials) }
                };

                server.Start();
            }

            return _app;
        }
    }
}
