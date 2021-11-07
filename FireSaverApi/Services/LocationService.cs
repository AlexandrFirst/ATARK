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

            compartment.EvacuationPlan.ScaleModel = mapper.Map<ScaleModel>(locationPointModel);
            dataContext.Update(compartment.EvacuationPlan.ScaleModel);
            await dataContext.SaveChangesAsync();


            return locationPointModel;
        }

        double getDelta(double a, double b)
        {
            return Math.Abs(a - b);
        }

        double getPixelXToCoordXCoef(ScalePoint p1, ScalePoint p2)
        {
            var p1MapPositionDto = mapper.Map<PositionDto>(p1.MapPosition);
            var p1WorldPositionDto = mapper.Map<PositionDto>(p1.WorldPosition);

            var p2MapPositionDto = mapper.Map<PositionDto>(p2.MapPosition);
            var p2WorldPositionDto = mapper.Map<PositionDto>(p2.WorldPosition);

            double deltaXPixel = getDelta(p1MapPositionDto.Latitude, p2MapPositionDto.Latitude);

            double deltaXCoord = getDelta(p1WorldPositionDto.Latitude, p2WorldPositionDto.Latitude);

            double fromPixelXToCoordXCoef = deltaXCoord / deltaXPixel;

            return fromPixelXToCoordXCoef;

        }
        double getPixelYToCoordYCoef(ScalePoint p1, ScalePoint p2)
        {

            var p1MapPositionDto = mapper.Map<PositionDto>(p1.MapPosition);
            var p1WorldPositionDto = mapper.Map<PositionDto>(p1.WorldPosition);

            var p2MapPositionDto = mapper.Map<PositionDto>(p2.MapPosition);
            var p2WorldPositionDto = mapper.Map<PositionDto>(p2.WorldPosition);

            double deltaYPixel = getDelta(p1MapPositionDto.Longtitude, p2MapPositionDto.Longtitude);

            double deltaYCoord = getDelta(p1WorldPositionDto.Longtitude, p2WorldPositionDto.Longtitude);

            double fromPixelYToCoordYCoef = deltaYCoord / deltaYPixel;

            return fromPixelYToCoordYCoef;

        }

        public async Task<PositionDto> ImgToWorldPostion(PositionDto imgPostion, int compartmentId)
        {
            ScalePoint firstPoint = await GetFirstPointAndScaleModel(compartmentId);

            double x3PixelCoord = imgPostion.Latitude;
            double y3PixelCoord = imgPostion.Longtitude;

            var firstPointMapPositionDto = mapper.Map<PositionDto>(firstPoint.MapPosition);
            var firstPointWorldPositionDto = mapper.Map<PositionDto>(firstPoint.WorldPosition);


            double x1PixelCoord = firstPointMapPositionDto.Latitude;
            double y1PixelCoord = firstPointMapPositionDto.Longtitude;

            double x1WorldCoord = firstPointWorldPositionDto.Latitude;
            double y1WorldCoord = firstPointWorldPositionDto.Longtitude;

            double x3WorldCoord = x1WorldCoord + (x3PixelCoord - x1PixelCoord) * locationPointModel.FromPixelXToCoordXCoef;
            double y3WorldCoord = y1WorldCoord + (y3PixelCoord - y1PixelCoord) * locationPointModel.FromPixelYToCoordYCoef;

            System.Console.WriteLine("locationPointModel.FromPixelXToCoordXCoef: " + locationPointModel.FromPixelXToCoordXCoef);
            System.Console.WriteLine("locationPointModel.FromPixelYToCoordYCoef: " + locationPointModel.FromPixelYToCoordYCoef);

            return new PositionDto()
            {
                Latitude = x3WorldCoord,
                Longtitude = y3WorldCoord
            };

        }

        public async Task<PositionDto> WorldToImgPostion(PositionDto worldPostion, int compartmentId)
        {
            ScalePoint firstPoint = await GetFirstPointAndScaleModel(compartmentId);

            double x3WorldCoord = worldPostion.Latitude;
            double y3WorldCoord = worldPostion.Longtitude;

            var firstPointMapPositionDto = mapper.Map<PositionDto>(firstPoint.MapPosition);
            var firstPointWorldPositionDto = mapper.Map<PositionDto>(firstPoint.WorldPosition);

            double x1WorldCoord = firstPointWorldPositionDto.Latitude;
            double y1WorldCoord = firstPointWorldPositionDto.Longtitude;

            double x1PixelCoord = firstPointMapPositionDto.Latitude;
            double y1PixelCoord = firstPointMapPositionDto.Longtitude;

            double x3PixelCoord = x1PixelCoord + (x3WorldCoord - x1WorldCoord) * locationPointModel.FromCoordXToPixelXCoef;
            double y3PixelCoord = y1PixelCoord + (y3WorldCoord - y1WorldCoord) * locationPointModel.FromCoordYToPixelYCoef;

            System.Console.WriteLine("locationPointModel.FromCoordXToPixelXCoef: " + locationPointModel.FromCoordXToPixelXCoef);
            System.Console.WriteLine("locationPointModel.FromCoordYToPixelYCoef: " + locationPointModel.FromCoordYToPixelYCoef);

            return new PositionDto()
            {
                Latitude = x3PixelCoord,
                Longtitude = y3PixelCoord
            };
        }

        async Task<ScalePoint> GetFirstPointAndScaleModel(int compartmentId)
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
                                                                            .ThenInclude(mPos => mPos.MapPosition)
                                                                        .Include(ev => ev.EvacuationPlan)
                                                                            .ThenInclude(s => s.ScaleModel)
                                                                            .ThenInclude(p => p.ScalePoints)
                                                                            .ThenInclude(wPos => wPos.WorldPosition)
                                                                        .FirstOrDefaultAsync(c => c.Id == compartmentId);

            return compartment;
        }
    }
}