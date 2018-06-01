using Grpc.Core;
using MagicOnion.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace MagicOnion.HttpGateway
{
    public static class MagicOnionHttpGatewayApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMagicOnionHttpGateway(this IApplicationBuilder app)
        {
            var _app = app ?? throw new ArgumentNullException(nameof(app));

            using (var appScope = _app.ApplicationServices.CreateScope())
            {
                var sp = appScope.ServiceProvider;

                var magicOnionServerOptions = sp.GetService<IOptions<MagicOnionServerOptions>>();
                var magicOnionServiceDefinitionDIWrapper = sp.GetService<MagicOnionServiceDefinitionDIWrapper>();

                if (magicOnionServerOptions == null || magicOnionServerOptions.Value == null)
                {
                    throw new ArgumentException(nameof(MagicOnionServerOptions));
                }

                if (magicOnionServiceDefinitionDIWrapper == null || magicOnionServiceDefinitionDIWrapper.MagicOnionServiceDefinition == null)
                {
                    throw new ArgumentException(nameof(MagicOnionServiceDefinitionDIWrapper));
                }

                var channelCredentials = magicOnionServerOptions.Value.Credentials == ServerCredentials.Insecure ? ChannelCredentials.Insecure : ChannelCredentials.Create(null, null);

                var channel = new Channel(
                    host: magicOnionServerOptions.Value.Host,
                    port: magicOnionServerOptions.Value.Port,
                    credentials: channelCredentials);

                app.UseMagicOnionHttpGateway(magicOnionServiceDefinitionDIWrapper.MagicOnionServiceDefinition.MethodHandlers, channel);
            }

            return _app;
        }
    }
}
