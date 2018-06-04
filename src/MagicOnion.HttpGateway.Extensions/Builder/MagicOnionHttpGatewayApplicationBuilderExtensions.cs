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

                var magicOnionServiceDefinitionDIWrapper = sp.GetService<MagicOnionServiceDefinitionDIWrapper>();
                if (magicOnionServiceDefinitionDIWrapper?.MagicOnionServiceDefinition == null)
                {
                    throw new InvalidOperationException("Unable to find the required services. Please add all the required services by calling 'IServiceCollection.AddMagicOnionServer' inside the call to 'ConfigureServices(...)' in the application startup code.");
                }

                var grpcOptions = sp.GetService<IOptions<GrpcOptions>>();
                if (grpcOptions?.Value == null)
                {
                    throw new ArgumentException(nameof(GrpcOptions));
                }

                var channel = new Channel(grpcOptions.Value.Host,
                    grpcOptions.Value.Port,
                    grpcOptions.Value.InsecureCredentials
                        ? ChannelCredentials.Insecure
                        : new SslCredentials(grpcOptions.Value.RootCertificates, new KeyCertificatePair(grpcOptions.Value.CertificateChain, grpcOptions.Value.PrivateKey)));

                app.UseMagicOnionHttpGateway(magicOnionServiceDefinitionDIWrapper.MagicOnionServiceDefinition.MethodHandlers, channel);
            }

            return _app;
        }
    }
}
