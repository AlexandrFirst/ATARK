using FireSaverApi.Helpers;

namespace FireSaverApi.Contracts
{
    public interface IUserContextService
    {
         HttpUserContext GetUserContext();
    }
}