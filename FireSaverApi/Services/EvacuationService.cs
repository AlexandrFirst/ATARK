using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.EvacuationPlanDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FireSaverApi.Services
{
    public class EvacuationService : IEvacuationService
    {
        private readonly DatabaseContext dataContext;
        private readonly IMapper mapper;
        private readonly ICompartmentHelper compartmentHelper;
        private readonly IPlanImageUploadService planImageUploadService;
        private readonly IEvacuationServiceHelper evacServiceHelper;

        public EvacuationService(DatabaseContext dataContext,
                                 IMapper mapper,
                                 ICompartmentHelper compartmentHelper,
                                 IPlanImageUploadService planImageUploadService,
                                 IEvacuationServiceHelper evacServiceHelper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
            this.compartmentHelper = compartmentHelper;
            this.planImageUploadService = planImageUploadService;
            this.evacServiceHelper = evacServiceHelper;
        }

        public async Task<EvacuationPlanDto> addEvacuationPlanToCompartment(int compartmentId, IFormFile planImage)
        {
            var compartment = await compartmentHelper.GetCompartmentById(compartmentId);
            var uploadPlanResponse = await planImageUploadService.UploadPlanImage(planImage);

            var newEvacPlan = new EvacuationPlan()
            {
                PublicId = uploadPlanResponse.PublicId,
                Url = uploadPlanResponse.Url,
                UploadTime = DateTime.Now,
                Compartment = compartment,
                ScaleModel = new ScaleModel()
            };

            await dataContext.EvacuationPlans.AddAsync(newEvacPlan);
            await dataContext.SaveChangesAsync();

            return mapper.Map<EvacuationPlanDto>(newEvacPlan);
        }

        public async Task<EvacuationPlanDto> changeEvacuationPlanOfCompartment(int compartmentId, IFormFile planImage)
        {
            var compartment = await compartmentHelper.GetCompartmentById(compartmentId);
            var uploadPlanResponse = await planImageUploadService.UploadPlanImage(planImage);
            var evacPlan = compartment.EvacuationPlan;
            await planImageUploadService.DeletePlanImage(evacPlan.PublicId);


            evacPlan.PublicId = uploadPlanResponse.PublicId;
            evacPlan.Url = uploadPlanResponse.Url;
            evacPlan.UploadTime = DateTime.Now;

            dataContext.EvacuationPlans.Update(evacPlan);
            await dataContext.SaveChangesAsync();

            return mapper.Map<EvacuationPlanDto>(evacPlan);
        }

        public async Task<EvacuationPlanDto> getEvacuationPlanofCompartment(int compartmentId)
        {
            var compartment = await compartmentHelper.GetCompartmentById(compartmentId);
            var evacPlan = compartment.EvacuationPlan;
            return mapper.Map<EvacuationPlanDto>(evacPlan);
        }

        public async Task removeEvacuationPlanOfCompartment(int compartmentId)
        {
            var compartment = await compartmentHelper.GetCompartmentById(compartmentId);
            var evacPlan = compartment.EvacuationPlan;

            dataContext.EvacuationPlans.Remove(evacPlan);
            await dataContext.SaveChangesAsync();
        }

        public async Task<List<EvacuationPlanDto>> GetEvacuationPlansFromCompartmentByUserId(int userId)
        {
            var currentUser = await dataContext.Users.Include(c => c.CurrentCompartment)
                                                     .ThenInclude(e => e.EvacuationPlan)
                                                     .FirstOrDefaultAsync(u => u.Id == userId);

            if (currentUser.CurrentCompartment == null)
                throw new Exception("The current compartment is not specified");

            return await GetEvacuationPlansFromCompartment(currentUser.CurrentCompartment);
        }

        public async Task<List<EvacuationPlanDto>> GetEvacuationPlansFromCompartmentByCompartmentId(int compartmentId)
        {
            var currentCompartment = await dataContext.Compartment.Include(e => e.EvacuationPlan).FirstOrDefaultAsync(u => u.Id == compartmentId);
            return await GetEvacuationPlansFromCompartment(currentCompartment);
        }


        private async Task<List<EvacuationPlanDto>> GetEvacuationPlansFromCompartment(Compartment currentCompartment)
        {
            List<EvacuationPlan> result = new List<EvacuationPlan>();
            result.Add(currentCompartment.EvacuationPlan);


            if (currentCompartment.GetType() == typeof(Room))
            {
                var attached = await dataContext.Floors.Include(f => f.BuildingWithThisFloor)
                                                     .Include(r => r.Rooms)
                                                     .Include(ev => ev.EvacuationPlan)
                                                     .FirstOrDefaultAsync(f => f.Rooms.Any(r => r.Id == currentCompartment.Id));

                result.Add(attached.EvacuationPlan);

                var restFloorsEvacPlans = await dataContext.Floors.Where(f => f.Level < attached.Level)
                                                .OrderByDescending(f => f.Level)
                                                .Select(f => f.EvacuationPlan).ToListAsync();
                result.AddRange(restFloorsEvacPlans);
            }
            else
            {
                var restFloorsEvacPlans = await dataContext.Floors.Where(f => f.Level < (currentCompartment as Floor).Level)
                                               .OrderByDescending(f => f.Level)
                                               .Select(f => f.EvacuationPlan).ToListAsync();
                result.AddRange(restFloorsEvacPlans);
            }

            return mapper.Map<List<EvacuationPlanDto>>(result);
        }

    }
}