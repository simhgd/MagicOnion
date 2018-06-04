using Grpc.Core;
using MagicOnion.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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
                if (magicOnionServiceDefinitionDIWrapper == null)
                {
                    throw new InvalidOperationException("Unable to find the required services. Please add all the required services by calling 'IServiceCollection.AddMagicOnionServer' inside the call to 'ConfigureServices(...)' in the application startup code.");
                }

                var magicOnionOptions = sp.GetService<IOptions<MagicOnionOptions>>();
                var grpcOptions = sp.GetService<IOptions<GrpcOptions>>();
                if (grpcOptions?.Value == null)
                {
                    throw new InvalidOperationException("Unable to find the GrpcOptions options. Please config options by calling 'IServiceCollection.AddMagicOnionServer(..., options) or MagicOnionServerBuilder.ConfigureGrpcOptions'.");
                }
                var magicOnionServices = sp.GetServices<IServiceMarker>();
                var magicOnionServicesType = magicOnionServices.Select(s => s.GetType());
                magicOnionServiceDefinitionDIWrapper.MagicOnionServiceDefinition = MagicOnionEngine.BuildServerServiceDefinition(magicOnionServicesType, magicOnionOptions.Value);

                var services = sp.GetService<IServiceCollection>();

                var serverPort = new ServerPort(grpcOptions.Value.Host,
                    grpcOptions.Value.Port,
                    grpcOptions.Value.InsecureCredentials
                        ? ServerCredentials.Insecure
                        : new SslServerCredentials(new List<KeyCertificatePair> {
                            new KeyCertificatePair(grpcOptions.Value.CertificateChain, grpcOptions.Value.PrivateKey)
                        }));

                var server = new Grpc.Core.Server
                {
                    Services = { magicOnionServiceDefinitionDIWrapper.MagicOnionServiceDefinition },
                    Ports = { serverPort }
                };

                server.Start();
            }

            return _app;
        }
    }
}
