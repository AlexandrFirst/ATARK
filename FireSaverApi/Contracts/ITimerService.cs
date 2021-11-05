using System.Collections.Generic;
using System.Threading.Tasks;
using FireSaverApi.Dtos;
using FireSaverApi.Helpers;

namespace FireSaverApi.Contracts
{
    public interface ITimerService
    {
        void IncreaseFailedUserTestFaledCount(int userId, int testId, int testFailedThreshold);
        List<int> GetFailedTestUsers();
        bool IsUserForTestBanned(int userId, int testId);
        void ClearUSerFailedTest(int userId, int testId);
    }
}