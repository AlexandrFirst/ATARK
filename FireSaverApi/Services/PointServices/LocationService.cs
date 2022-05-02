using System;
using System.Collections.Generic;
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
    public class LocationService : ILocationService
    {
        private enum TransformationType { X, Y }

        class TransformationPointModel
        {
            public PositionDto FromFirstPoint { get; set; }
            public PositionDto FromSecondPoint { get; set; }
            public PositionDto ToFirstPoint { get; set; }

            public double fromToCoefX { get; set; }
            public double fromToCoefY { get; set; }
        }


        private readonly DatabaseContext dataContext;
        private readonly IMapper mapper;
        private LocationPointModel locationPointModel;
        public LocationService(DatabaseContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
        }


        public async Task<LocationPointModel> CalculateLocationModel(int compartmentId)
        {
            var compartment = await dataContext.Compartment.Include(s => s.EvacuationPlan)
                                                            .ThenInclude(m => m.ScaleModel)
                                                            .ThenInclude(p => p.ScalePoints)
                                                            .FirstOrDefaultAsync(c => c.Id == compartmentId);

            if (compartment.EvacuationPlan == null && compartment.EvacuationPlan.ScaleModel == null)
            {
                throw new Exception("Evacuation plan or scale model is not set");
            }

            var points = compartment.EvacuationPlan.ScaleModel.ScalePoints;
            if (points.Count < 3)
            {
                throw new Exception("Not enough points to caclulate model");
            }

            List<PositionDto> worldPositions = mapper.Map<List<PositionDto>>(points.Select(p => p.WorldPosition).ToList<string>());
            List<PositionDto> mapPositions = mapper.Map<List<PositionDto>>(points.Select(p => p.MapPosition).ToList<string>());

            double[] deltaRealX = new double[points.Count - 1];
            double[] deltaRealY = new double[points.Count - 1];

            double[] deltaImageX = new double[points.Count - 1];
            double[] deltaImageY = new double[points.Count - 1];

            for (int i = 1; i < points.Count; i++)
            {
                deltaRealY[i - 1] = worldPositions[0].Longtitude - worldPositions[i].Longtitude;
                deltaRealX[i - 1] = worldPositions[0].Latitude - worldPositions[i].Latitude;

                deltaImageY[i - 1] = mapPositions[0].Longtitude - mapPositions[i].Longtitude;
                deltaImageX[i - 1] = mapPositions[0].Latitude - mapPositions[i].Latitude;
            }

            double imageXToRealXProjectCoef = 0;
            double imageXToRealYProjectCoef = 0;

            double imageYToRealXProjectCoef = 0;
            double imageYToRealYProjectCoef = 0;

            for (int i = 1; i < points.Count - 1; i++)
            {
                double m_imageXToRealXProjectCoef = (deltaImageX[i] * deltaRealX[0] - deltaRealY[i] * deltaImageX[0]) /
                    (deltaRealX[i] - deltaRealY[i] * deltaRealX[0]);
                double m_imageXToRealYProjectCoef = (deltaImageX[0] - deltaRealX[0] * m_imageXToRealXProjectCoef) / deltaRealY[0];

                double m_imageYToRealXProjectCoef = (deltaImageY[i] * deltaRealX[0] - deltaRealY[i] * deltaImageY[0]) /
                    (deltaRealX[i] - deltaRealY[i] * deltaRealX[0]);
                double m_imageYToRealYProjectCoef = (deltaImageY[0] - deltaRealX[0] * m_imageYToRealXProjectCoef) / deltaRealY[0];

                imageXToRealXProjectCoef += m_imageXToRealXProjectCoef;
                imageXToRealYProjectCoef += m_imageXToRealYProjectCoef;

                imageYToRealXProjectCoef += m_imageYToRealXProjectCoef;
                imageYToRealYProjectCoef += m_imageYToRealYProjectCoef;
            }

            imageXToRealXProjectCoef /= (points.Count - 1);
            imageXToRealYProjectCoef /= (points.Count - 1);

            imageYToRealXProjectCoef /= (points.Count - 1);
            imageYToRealYProjectCoef /= (points.Count - 1);


            locationPointModel = new LocationPointModel()
            {
                ImageXToRealXProjectCoef = imageXToRealXProjectCoef,
                ImageXToRealYProjectCoef = imageXToRealYProjectCoef,
                ImageYToRealXProjectCoef = imageYToRealXProjectCoef,
                ImageYToRealYProjectCoef = imageYToRealYProjectCoef
            };

            mapper.Map(locationPointModel, compartment.EvacuationPlan.ScaleModel);
            dataContext.Update(compartment.EvacuationPlan.ScaleModel);
            await dataContext.SaveChangesAsync();


            return locationPointModel;
        }

        public async Task<PositionDto> WorldToImgPostion(PositionDto worldPostion, int compartmentId)
        {
            ScalePoint firstPoint = await GetFirstPointAndInitScaleModel(compartmentId);
            ScaleModel scaleModel = firstPoint.ScaleModel;

            var secondCoordPosition = worldPostion;

            var firstPosPixel = mapper.Map<PositionDto>(firstPoint.MapPosition);
            var firstPosCoord = mapper.Map<PositionDto>(firstPoint.WorldPosition);


            double deltaRealX = firstPosCoord.Latitude - worldPostion.Latitude;
            double deltaRealY = firstPosCoord.Latitude - worldPostion.Longtitude;


            PositionDto transformedPos = new PositionDto()
            {
                Latitude = firstPosPixel.Latitude,
                Longtitude = firstPosPixel.Longtitude
            };

            double deltaImageX = deltaRealX * (1.0 / scaleModel.ImageXToRealXProjectCoef) +
                deltaRealY * (1.0 / scaleModel.ImageYToRealXProjectCoef);
            double deltaImageY = deltaRealX * (1.0 / scaleModel.ImageXToRealYProjectCoef) +
                deltaRealY * (1.0 / scaleModel.ImageYToRealYProjectCoef);

            transformedPos.Latitude += deltaImageX;
            transformedPos.Longtitude += deltaImageY;

            return transformedPos;
        }


        async Task<ScalePoint> GetFirstPointAndInitScaleModel(int compartmentId)
        {
            var compartment = await GetCompartmentById(compartmentId);

            var scalePoints = compartment.EvacuationPlan.ScaleModel.ScalePoints;

            var firstPoint = scalePoints.Take(1).ToList()[0];

            locationPointModel = mapper.Map<LocationPointModel>(compartment.EvacuationPlan.ScaleModel);

            await CheckScaleModelValidityAndUpdateIfInvalid(locationPointModel, compartmentId);
            return firstPoint;
        }

        async Task CheckScaleModelValidityAndUpdateIfInvalid(LocationPointModel locationPointModel, int compartmentId)
        {
            if (locationPointModel.isInvalid())
            {
                locationPointModel = await CalculateLocationModel(compartmentId);
                if (locationPointModel.isInvalid())
                {
                    throw new Exception("Location scale model is invalid.Reset scale points");
                }
            }
        }

        async Task<Compartment> GetCompartmentById(int compartmentId)
        {
            var compartment = await dataContext.Compartment.Include(ev => ev.EvacuationPlan)
                                                                            .ThenInclude(s => s.ScaleModel)
                                                                            .ThenInclude(p => p.ScalePoints)
                                                                        .FirstOrDefaultAsync(c => c.Id == compartmentId);

            return compartment;
        }
    }
}