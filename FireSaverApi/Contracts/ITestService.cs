using System.Threading.Tasks;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.TestDtos;

namespace FireSaverApi.Contracts
{
    public interface ITestService
    {
        Task<TestInputDto> AddTestToCompartment(int compartmentId, TestInputDto newTestInfo);
        Task<TestInputDto> UpdateTestToCompartment(int testId, TestInputDto newTestInfo);
        Task RemoveTestFromCompartment(int compartmentId);
        Task<Test> GetTestInfo(int testId);
        Task<bool> CheckTestAnwears(AnswerListDto answears);

    }
}