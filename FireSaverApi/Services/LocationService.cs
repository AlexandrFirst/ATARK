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
            if (points.Count < 2)
            {
                throw new Exception("Not enough points to caclulate model");
            }

            double avgFromPixelXToCoordXCoef = 0;
            double avgFromPixelYToCoordYCoef = 0;

            double avgFromCoordXToPixelXCoef = 0;
            double avgFromCoordYToPixelYCoef = 0;

            for (int i = 1; i < points.Count; i++)
            {
                double fromPixelXToCoordXCoef = getPixelXToCoordXCoef(points[0], points[i]);
                double fromPixelYToCoordYCoef = getPixelYToCoordYCoef(points[0], points[i]);

                avgFromPixelXToCoordXCoef += fromPixelXToCoordXCoef;
                avgFromPixelYToCoordYCoef += fromPixelYToCoordYCoef;
            }

            avgFromPixelXToCoordXCoef /= (points.Count - 1);
            avgFromPixelYToCoordYCoef /= (points.Count - 1);

            avgFromCoordXToPixelXCoef = 1 / avgFromPixelXToCoordXCoef;
            avgFromCoordYToPixelYCoef = 1 / avgFromPixelYToCoordYCoef;


            locationPointModel = new LocationPointModel()
            {
                FromCoordXToPixelXCoef = avgFromCoordXToPixelXCoef,
                FromPixelXToCoordXCoef = avgFromPixelXToCoordXCoef,

                FromCoordYToPixelYCoef = avgFromCoordYToPixelYCoef,
                FromPixelYToCoordYCoef = avgFromPixelYToCoordYCoef
            };

            mapper.Map(locationPointModel, compartment.EvacuationPlan.ScaleModel);
            dataContext.Update(compartment.EvacuationPlan.ScaleModel);
            await dataContext.SaveChangesAsync();


            return locationPointModel;
        }

        double getPixelXToCoordXCoef(ScalePoint p1, ScalePoint p2)
        {
            double fromPixelXToCoordXCoef = getFromCoordToPixelCoef(p1, p2, TransformationType.X);
            return fromPixelXToCoordXCoef;
        }

        double getPixelYToCoordYCoef(ScalePoint p1, ScalePoint p2)
        {
            double fromPixelYToCoordYCoef = getFromCoordToPixelCoef(p1, p2, TransformationType.Y);
            return fromPixelYToCoordYCoef;
        }

        double getFromCoordToPixelCoef(ScalePoint p1, ScalePoint p2, TransformationType transformationType)
        {
            var p1MapPositionDto = mapper.Map<PositionDto>(p1.MapPosition);
            var p1WorldPositionDto = mapper.Map<PositionDto>(p1.WorldPosition);

            var p2MapPositionDto = mapper.Map<PositionDto>(p2.MapPosition);
            var p2WorldPositionDto = mapper.Map<PositionDto>(p2.WorldPosition);

            double deltaPixel = 0;
            double deltaCoord = 0;

            switch (transformationType)
            {
                case TransformationType.X:
                    {
                        deltaPixel = getDelta(p1MapPositionDto.Latitude, p2MapPositionDto.Latitude);
                        deltaCoord = getDelta(p1WorldPositionDto.Latitude, p2WorldPositionDto.Latitude);
                        break;
                    }
                case TransformationType.Y:
                    {
                        deltaPixel = getDelta(p1MapPositionDto.Longtitude, p2MapPositionDto.Longtitude);
                        deltaCoord = getDelta(p1WorldPositionDto.Longtitude, p2WorldPositionDto.Longtitude);
                        break;
                    }
                default:
                    {
                        throw new Exception("Unknown tarnsformation type");
                    }
            }

            double fromCoordToPixelCoef = deltaCoord / deltaPixel;
            return fromCoordToPixelCoef;
        }

        double getDelta(double a, double b)
        {
            return Math.Abs(a - b);
        }

        public async Task<PositionDto> ImgToWorldPostion(PositionDto imgPostion, int compartmentId)
        {
            ScalePoint firstPoint = await GetFirstPointAndInitScaleModel(compartmentId);

            var secondPixelPosition = imgPostion;

            var initPixelPosition = mapper.Map<PositionDto>(firstPoint.MapPosition);
            var initCoordPosition = mapper.Map<PositionDto>(firstPoint.WorldPosition);

            TransformationPointModel pointModel = new TransformationPointModel()
            {
                FromFirstPoint = initPixelPosition,
                FromSecondPoint = secondPixelPosition,
                ToFirstPoint = initCoordPosition,
                fromToCoefX = locationPointModel.FromPixelXToCoordXCoef,
                fromToCoefY = locationPointModel.FromPixelYToCoordYCoef
            };

            var transformedPos = ConvertFromToPostion(pointModel, compartmentId);

            return transformedPos;

        }

        public async Task<PositionDto> WorldToImgPostion(PositionDto worldPostion, int compartmentId)
        {
            ScalePoint firstPoint = await GetFirstPointAndInitScaleModel(compartmentId);

            var secondCoordPosition = worldPostion;

            var initPixelPosition = mapper.Map<PositionDto>(firstPoint.MapPosition);
            var initCoordPosition = mapper.Map<PositionDto>(firstPoint.WorldPosition);

            TransformationPointModel pointModel = new TransformationPointModel()
            {
                FromFirstPoint = initCoordPosition,
                FromSecondPoint = secondCoordPosition,
                ToFirstPoint = initPixelPosition,
                fromToCoefX = locationPointModel.FromCoordXToPixelXCoef,
                fromToCoefY = locationPointModel.FromCoordYToPixelYCoef
            };

            var transformedPos = ConvertFromToPostion(pointModel, compartmentId);

            return transformedPos;
        }


        private PositionDto ConvertFromToPostion(TransformationPointModel transformationPointModel, int compartmentId)
        {
            double fromSecondPointLatitude = transformationPointModel.FromSecondPoint.Latitude;
            double fromSecondPointLongtitude = transformationPointModel.FromSecondPoint.Longtitude;

            double fromFirstPointLatitude = transformationPointModel.FromFirstPoint.Latitude;
            double fromFirstPointLongtitude = transformationPointModel.FromFirstPoint.Longtitude;

            double toFirstPointLatitude = transformationPointModel.ToFirstPoint.Latitude;
            double toFirstPointLongtitude = transformationPointModel.ToFirstPoint.Longtitude;

            double toSecondPointLatitude = toFirstPointLatitude + (fromSecondPointLatitude - fromFirstPointLatitude) * transformationPointModel.fromToCoefX;
            double toSecondPointLogtitude = toFirstPointLongtitude + (fromSecondPointLongtitude - fromFirstPointLongtitude) * transformationPointModel.fromToCoefY;


            return new PositionDto()
            {
                Latitude = toSecondPointLatitude,
                Longtitude = toSecondPointLogtitude
            };
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