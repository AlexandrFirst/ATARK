using System.Threading.Tasks;

namespace FireSaverApi.Contracts
{
    public interface ICompartmentService<TEntityDto, Entity>
    {
        Task<Entity> AddCompartment(int baseCompartmentId, TEntityDto newCompartmentDto);
        Task<Entity> ChangeCompartmentInfo(int compartmentId, TEntityDto newCompartmentDto);
        Task<Entity> GetCompartmentInfo(int compartmentId);
        Task DeleteCompartment(int compartmentId);
    }
}