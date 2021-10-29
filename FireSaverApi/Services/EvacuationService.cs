using System;
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
                Compartment = compartment
            };

            await dataContext.EvacuationPlans.AddAsync(newEvacPlan);
            await dataContext.SaveChangesAsync();

            return mapper.Map<EvacuationPlanDto>(newEvacPlan);
        }

        public async Task<EvacuationPlanDto> changeEvacuationPlanToCompartment(int compartmentId, IFormFile planImage)
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

        public async Task<EvacuationPlanDto> getEvacuationPlanToCompartment(int compartmentId)
        {
            var compartment = await compartmentHelper.GetCompartmentById(compartmentId);
            var evacPlan = compartment.EvacuationPlan;
            return mapper.Map<EvacuationPlanDto>(evacPlan);
        }

        public async Task removeEvacuationPlanToCompartment(int compartmentId)
        {
            var compartment = await compartmentHelper.GetCompartmentById(compartmentId);
            var evacPlan = compartment.EvacuationPlan;

            dataContext.EvacuationPlans.Remove(evacPlan);
            await dataContext.SaveChangesAsync();
        }

       //TODO: make evacuation plan builder
    }
}