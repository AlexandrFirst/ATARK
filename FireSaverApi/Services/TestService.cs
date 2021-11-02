using System.Threading.Tasks;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;

namespace FireSaverApi.Services
{
    public class TestService : ITestService
    {
        private readonly DatabaseContext dataContext;

        public TestService(DatabaseContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public Task AddTestToCompartment()
        {
            throw new System.NotImplementedException();
        }

        public Task CheckTestAnwears()
        {
            throw new System.NotImplementedException();
        }

        public Task GetTestInfo()
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveTestFromCompartment()
        {
            throw new System.NotImplementedException();
        }
    }
}