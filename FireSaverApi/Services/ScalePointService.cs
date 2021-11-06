using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos;
using FireSaverApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FireSaverApi.Services
{
    public class ScalePointService : IScalePointService, IPositionTransformHelper
    {
        private readonly DatabaseContext context;
        private readonly IMapper mapper;
        private readonly ILocationService locationService;
        private readonly IEvacuationServiceHelper evacuationServiceHelper;

        public ScalePointService(DatabaseContext context,
                                IMapper mapper,
                                ILocationService locationService,
                                IEvacuationServiceHelper evacuationServiceHelper)
        {
            this.context = context;
            this.mapper = mapper;
            this.locationService = locationService;
            this.evacuationServiceHelper = evacuationServiceHelper;
        }

        public async Task<ScalePointDto> AddNewScalePoint(int evacuationPlanId, ScalePointDto inputPoint)
        {
            var pointToInsert = mapper.Map<ScalePoint>(inputPoint);

            var correspondentEvacuation = await evacuationServiceHelper.GetEvacPlanById(evacuationPlanId);
            var scaleModel = correspondentEvacuation.ScaleModel;

            if (!IsScalePointValid(inputPoint, scaleModel))
            {
                throw new Exception("Point is invalid; try to select point further");
            }


            pointToInsert.ScaleModel = scaleModel;

            await context.ScalePoints.AddAsync(pointToInsert);
            await context.SaveChangesAsync();

            return mapper.Map<ScalePointDto>(pointToInsert);
        }

        private bool IsScalePointValid(ScalePointDto scalePoint, ScaleModel scaleModel)
        {
            var allScalePoints = scaleModel.ScalePoints;

            for (int i = 0; i < allScalePoints.Count(); i++)
            {
                double ratioX = getRatio(scalePoint.MapPosition.Latitude,
                                           allScalePoints[i].MapPosition.Latitude,
                                           scaleModel.ApplyingEvacPlans.Width);

                double ratioY = getRatio(scalePoint.MapPosition.Longtitude,
                                           allScalePoints[i].MapPosition.Longtitude,
                                           scaleModel.ApplyingEvacPlans.Height);

                if (ratioX < scaleModel.MinDistanceDifferenceLatitudeCoef ||
                    ratioY < scaleModel.MinDistanceDifferenceLongtitudeCoef)
                {
                    return false;
                }
            }
            return true;
        }

        private double getRatio(double a, double b, double ratioBase)
        {
            double value = Math.Abs(a - b);
            return value / ratioBase;
        }

        public async Task<PositionDto> ConvertImgToWorldPos(PositionDto inputPosition, int compartmentId)
        {
            PositionDto worldPos = await locationService.ImgToWorldPostion(inputPosition, compartmentId);
            return worldPos;
        }

        public async Task<PositionDto> ConvertWorldToImgPos(PositionDto inputPosition, int compartmentId)
        {
            PositionDto imgPos = await locationService.WorldToImgPostion(inputPosition, compartmentId);
            return imgPos;
        }

        public async Task DeleteAllPoints(int evacuationPlanId)
        {
            var evacuationPlan = await evacuationServiceHelper.GetEvacPlanById(evacuationPlanId);
            var scaleModel = evacuationPlan.ScaleModel;

            var evacuationPlanScalePoints = (await context.ScalePoints.Include(s => s.ScaleModel)
                                            .ToListAsync())
                                            .Where(p => p.ScaleModel.Id == scaleModel.Id)
                                            .ToList();

            context.ScalePoints.RemoveRange(evacuationPlanScalePoints);
            ResetScaleModel(scaleModel);

            await context.SaveChangesAsync();

        }

        public async Task DeleteSinglePoint(int scalePointId)
        {
            var point = await context.ScalePoints.Include(m => m.ScaleModel)
                                                .ThenInclude(evPlan => evPlan.ApplyingEvacPlans)
                                                .ThenInclude(c => c.Compartment)
                                                .FirstOrDefaultAsync(s => s.Id == scalePointId);
            if (point == null)
            {
                throw new System.Exception("Scale point is not found");
            }

            var scaleModel = point.ScaleModel;
            var compartmentId = point.ScaleModel.ApplyingEvacPlans.Compartment.Id;

            context.ScalePoints.Remove(point);
            await context.SaveChangesAsync();

            await recalculateScaleModel(scaleModel, compartmentId);
        }

        private async Task recalculateScaleModel(ScaleModel scaleModel, int compartmentId)
        {
            var allScalePoints = (await context.ScalePoints.Include(s => s.ScaleModel)
                                                        .ToListAsync())
                                                        .Where(p => p.ScaleModel.Id == scaleModel.Id)
                                                        .ToList();
            if (allScalePoints.Count == 0)
            {
                ResetScaleModel(scaleModel);
            }
            else
            {
                var newModel = await locationService.CalculateLocationModel(compartmentId);
                UpdateScaleModel(scaleModel, newModel);
            }

            context.Update(scaleModel);
            await context.SaveChangesAsync();
        }

        private void UpdateScaleModel(ScaleModel scaleModel, LocationPointModel newModel)
        {
            scaleModel.FromCoordXToPixelXCoef = newModel.FromCoordXToPixelXCoef;
            scaleModel.FromCoordYToPixelYCoef = newModel.FromCoordYToPixelYCoef;
            scaleModel.FromPixelXToCoordXCoef = newModel.FromPixelXToCoordXCoef;
            scaleModel.FromPixelYToCoordYCoef = newModel.FromPixelYToCoordYCoef;
        }

        private void ResetScaleModel(ScaleModel scaleModel)
        {
            scaleModel.FromCoordXToPixelXCoef = 0;
            scaleModel.FromCoordYToPixelYCoef = 0;
            scaleModel.FromPixelXToCoordXCoef = 0;
            scaleModel.FromPixelYToCoordYCoef = 0;

        }
    }
}