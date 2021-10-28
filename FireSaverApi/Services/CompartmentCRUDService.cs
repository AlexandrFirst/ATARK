using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.CompartmentDtos;

namespace FireSaverApi.Services
{
    public abstract class CompartmentCRUDService<TEntityDto, Entity, BaseEntity> :
                ICompartmentCRUDService<TEntityDto, Entity> where TEntityDto : CompartmentDto
                                                            where Entity : Compartment
    {
        protected readonly DatabaseContext databaseContext;
        protected readonly IMapper mapper;
        public CompartmentCRUDService(DatabaseContext databaseContext, IMapper mapper)
        {
            this.mapper = mapper;
            this.databaseContext = databaseContext;
        }

        public async Task<Entity> AddCompartment(int baseCompartmentId, TEntityDto newCompartmentDto)
        {
            BaseEntity baseCompartment = await GetBaseCompartment(baseCompartmentId);

            Entity newCompartment = mapper.Map<Entity>(newCompartmentDto);

            if (typeof(Floor) == typeof(Entity))
            {
                var floor = newCompartment as Floor;
                var building = baseCompartment as Building;

                floor.BuildingWithThisFloor = building;
            }
            else if (typeof(Room) == typeof(Entity))
            {
                var room = newCompartment as Room;
                var floor = baseCompartment as Floor;

                room.RoomFloor = floor;
            }

            await databaseContext.AddAsync(newCompartment);
            await databaseContext.SaveChangesAsync();

            return newCompartment;
        }


        public Task<Entity> ChangeCompartmentInfo(int compartmentId, TEntityDto newCompartmentDto)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteCompartment(int compartmentId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Entity> GetCompartmentInfo(int compartmentId, TEntityDto newCompartmentDto)
        {
            throw new System.NotImplementedException();
        }



        public abstract Task<BaseEntity> GetBaseCompartment(int baseCompartmentId);
        public abstract Task<Entity> GetCompartmentById(int CompartmentId);
    }
}