using System.Threading.Tasks;
using FireSaverApi.Dtos.TestDtos;

namespace FireSaverApi.Contracts
{
    public interface ITestService
    {
         Task<TestInputDto> AddTestToCompartment(int compartmentId, TestInputDto newTestInfo);
         Task RemoveTestFromCompartment(int compartmentId);
         Task<TestOutputDto> GetTestInfo(int testId);
         Task<bool> CheckTestAnwears(AnswerListDto answears);

    }
}