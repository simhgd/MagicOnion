using Grpc.Core;

namespace MagicOnion
{
    public class MagicOnionServerOptions
    {
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 12345;
        public ServerCredentials Credentials { get; set; } = ServerCredentials.Insecure;
    }
}
