using System.IO;

namespace MagicOnion
{
    public class GrpcOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool InsecureCredentials { get; set; }
        public string CertificateChain { get; set; }
        public string PrivateKey { get; set; }
        public string RootCertificates { get; set; }

        private string _certificateChainPath;
        private string _privateKeyPath;
        private string _rootCertificatesPath;

        public string CertificateChainPath
        {
            get => _certificateChainPath;
            set
            {
                _certificateChainPath = value;
                if (!File.Exists(_certificateChainPath))
                    throw new FileNotFoundException(nameof(CertificateChainPath), _certificateChainPath);
                CertificateChain = File.ReadAllText(_certificateChainPath);
            }
        }
        public string PrivateKeyPath
        {
            get => _privateKeyPath;
            set
            {
                _privateKeyPath = value;
                if (!File.Exists(_privateKeyPath))
                    throw new FileNotFoundException(nameof(PrivateKeyPath), _privateKeyPath);
                PrivateKey = File.ReadAllText(_privateKeyPath);
            }
        }
        public string RootCertificatesPath
        {
            get => _rootCertificatesPath;
            set
            {
                _rootCertificatesPath = value;
                if (!File.Exists(_rootCertificatesPath))
                    throw new FileNotFoundException(nameof(RootCertificatesPath), _rootCertificatesPath);
                RootCertificates = File.ReadAllText(_rootCertificatesPath);
            }
        }
    }
}
