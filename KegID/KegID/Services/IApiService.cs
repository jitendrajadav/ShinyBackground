using Fusillade;

namespace KegID.Services
{
    public interface IApiService<T>
    {
        T GetApi(Priority priority);
    }
}
