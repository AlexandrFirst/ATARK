using FireSaverApi.Helpers;

namespace FireSaverApi.Contracts
{
    public interface IUserContextService
    {
         MyHttpContext GetUserContext();
    }
}