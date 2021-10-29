using System.Threading.Tasks;
using FireSaverApi.DataContext;

namespace FireSaverApi.Contracts
{
    public interface ICompartmentHelper
    {
         Task<Compartment> GetCompartmentById(int compartmentId);
    }
}