using Grpc.Core;
using MagicOnion.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace MagicOnion
{
    public static class MagicOnionClientBuilderExtensions
    {
        public static MagicOnionClientBuilder ConfigureMagicOnionClientOptions<TService>(this MagicOnionClientBuilder builder, Action<MagicOnionClientOptions<TService>> setupAction)
            where TService : class, IService<TService>
        {
            var _builder = builder ?? throw new ArgumentNullException(nameof(builder));

            _builder.Services.Configure(setupAction);

            return _builder;
        }

        public static MagicOnionClientBuilder AddService<TService>(this MagicOnionClientBuilder builder, Action<MagicOnionClientOptions<TService>> setupAction = null)
            where TService : class, IService<TService>
        {
            var _builder = builder ?? throw new ArgumentNullException(nameof(builder));

            if (setupAction != null)
                _builder.ConfigureMagicOnionClientOptions(setupAction);

            _builder.Services.AddTransient(typeof(TService), sp =>
            {
                var magicOnionClientOptions = sp.GetService<IOptions<MagicOnionClientOptions<TService>>>();
                if (magicOnionClientOptions?.Value == null)
                {
                    throw new ArgumentException(nameof(MagicOnionClientOptions<TService>));
                }

                var channel = new Channel(magicOnionClientOptions.Value.Host,
                magicOnionClientOptions.Value.Port,
                magicOnionClientOptions.Value.InsecureCredentials
                    ? ChannelCredentials.Insecure
                    : new SslCredentials(magicOnionClientOptions.Value.RootCertificates, new KeyCertificatePair(magicOnionClientOptions.Value.CertificateChain, magicOnionClientOptions.Value.PrivateKey)));

                var client = MagicOnionClient.Create<TService>(channel);
                return client;
            });

            return _builder;
        }

        public static MagicOnionClientBuilder AddService<TService>(this MagicOnionClientBuilder builder, IConfigurationSection configurationSection)
            where TService : class, IService<TService>
        {
            var _builder = builder ?? throw new ArgumentNullException(nameof(builder));

            _builder.Services.Configure<MagicOnionClientOptions<TService>>(configurationSection);
            _builder.AddService<TService>();

            return _builder;
        }
    }
}
