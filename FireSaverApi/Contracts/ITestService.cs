using System.Threading.Tasks;

namespace FireSaverApi.Contracts
{
    public interface ITestService
    {
         Task AddTestToCompartment();
         Task RemoveTestFromCompartment();
         Task GetTestInfo();
         Task CheckTestAnwears();

    }
}