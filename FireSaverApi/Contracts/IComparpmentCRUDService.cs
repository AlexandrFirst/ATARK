using System.Threading.Tasks;

namespace FireSaverApi.Contracts
{
    public interface ICompartmentCRUDService<TEntityDto, Entity>
    {
        Task<Entity> AddCompartment(int baseCompartmentId, TEntityDto newCompartmentDto);
        Task<Entity> ChangeCompartmentInfo(int compartmentId, TEntityDto newCompartmentDto);
        Task<Entity> GetCompartmentInfo(int compartmentId, TEntityDto newCompartmentDto);
        Task DeleteCompartment(int compartmentId);
    }
}