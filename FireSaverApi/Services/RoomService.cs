using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.CompartmentDtos;
using Microsoft.EntityFrameworkCore;

namespace FireSaverApi.Services
{
    public class RoomService : CompartmentService<RoomDto, Room, Floor>
    {
        public RoomService(DatabaseContext databaseContext, IMapper mapper) : base(databaseContext, mapper)
        {
        }

        protected override async Task<Floor> GetBaseCompartment(int baseCompartmentId)
        {
            var floor = await databaseContext.Floors.FirstOrDefaultAsync(f => f.Id == baseCompartmentId);
            if (floor == null)
            {
                throw new System.Exception("floor is not found");
            }
            return floor;
        }

        protected override async Task<Room> GetCompartmentById(int CompartmentId)
        {
            var room = await databaseContext.Rooms.Include(u => u.InboundUsers)
                                                  .Include(i => i.Iots)
                                                  .Include(t => t.CompartmentTest)
                                                  .FirstOrDefaultAsync(r => r.Id == CompartmentId);
            if (room == null)
            {
                throw new System.Exception("Room is not found");
            }
            return room;
        }
    }
}