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
    public abstract class CompartmentService<TEntityDto, Entity, BaseEntity> :
                ICompartmentService<TEntityDto, Entity> where TEntityDto : CompartmentDto
                                                            where Entity : Compartment
    {
        protected readonly DatabaseContext databaseContext;
        protected readonly IMapper mapper;
        public CompartmentService(DatabaseContext databaseContext, IMapper mapper)
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

                if(building.Floors.Any(f => f.Level == floor.Level)){
                    throw new Exception("Building can't have to same floors");
                }

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


        public async Task<Entity> ChangeCompartmentInfo(int compartmentId, TEntityDto newCompartmentDto)
        {
            var entity = await GetCompartmentById(compartmentId);
            if (compartmentId != entity.Id)
                throw new System.Exception("something went wrong");

            entity = mapper.Map<Entity>(newCompartmentDto);
            databaseContext.Update(entity);
            await databaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteCompartment(int compartmentId)
        {
            var entity = await GetCompartmentById(compartmentId);
            
            if(CanCompartmentBeDeleted(entity))
                databaseContext.Remove(entity);

            await databaseContext.SaveChangesAsync();
        }

        public async Task<Entity> GetCompartmentInfo(int compartmentId)
        {
            var entity = await GetCompartmentById(compartmentId);
            return entity;
        }


        private bool CanCompartmentBeDeleted(Entity compartment){
            if(compartment.InboundUsers.Count > 0){
                return false;
            }
            return true;
        }

        protected abstract Task<BaseEntity> GetBaseCompartment(int baseCompartmentId);
        protected abstract Task<Entity> GetCompartmentById(int CompartmentId);
    }
}