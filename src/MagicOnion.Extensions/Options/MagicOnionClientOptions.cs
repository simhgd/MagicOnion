namespace MagicOnion
{
    public class MagicOnionClientOptions<TService> : GrpcOptions
        where TService : class, IService<TService>
    {
    }
}
