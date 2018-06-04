using Grpc.Core;
using MagicOnion.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicOnion
{
    public static class MagicOnionClientBuilderExtensions
    {
        public static MagicOnionClientBuilder ConfigureGrpcOptions(this MagicOnionClientBuilder builder, Action<GrpcOptions> setupAction)
        {
            var _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            var _setupAction = setupAction ?? throw new ArgumentNullException(nameof(setupAction));

            builder.Services.Configure<GrpcOptions>(setupAction);

            return _builder;
        }

        public static MagicOnionClientBuilder ConfigureGrpcOptions(this MagicOnionClientBuilder builder, IConfigurationSection configurationSection)
        {
            var _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            var _configurationSection = configurationSection ?? throw new ArgumentNullException(nameof(configurationSection));

            builder.Services.Configure<GrpcOptions>(_configurationSection);

            return _builder;
        }

        public static MagicOnionClientBuilder AddService<TService>(this MagicOnionClientBuilder builder)
            where TService : class, IService<TService>
        {
            var _builder = builder ?? throw new ArgumentNullException(nameof(builder));

            _builder.Services.AddTransient(typeof(TService), sp =>
            {
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

                var client = MagicOnionClient.Create<TService>(channel);
                return client;
            });

            return _builder;
        }
    }
}
