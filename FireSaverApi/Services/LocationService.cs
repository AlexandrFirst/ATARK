using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private LocationPointModel locationPointModel;
        public LocationService(DatabaseContext dataContext)
        {
            this.dataContext = dataContext;
        }


        public async Task<LocationPointModel> CalculateLocationModel()
        {
            // var points = await dataContext.ScalePoints.Include(w => w.WorldPosition).Include(m => m.MapPosition).Take(2).ToListAsync();
            // //TODO: Add processing of multiple points
            // double deltaXPixel = Math.Abs(points[0].MapPosition.Latitude - points[1].MapPosition.Latitude);
            // double deltaYPixel = Math.Abs(points[0].MapPosition.Longtitude - points[1].MapPosition.Longtitude);

            // double deltaXCoord = Math.Abs(points[0].WorldPosition.Latitude - points[1].WorldPosition.Latitude);
            // double deltaYCoord = Math.Abs(points[0].WorldPosition.Longtitude - points[1].WorldPosition.Longtitude);

            // double fromPixelXToCoordXCoef = deltaXCoord / deltaXPixel;
            // double fromCoordXToPixelXCoef = 1 / fromPixelXToCoordXCoef;

            // double fromPixelYToCoordYCoef = deltaYCoord / deltaYPixel;
            // double fromCoordYToPixelYCoef = 1 / fromPixelYToCoordYCoef;

            // locationPointModel = new LocationPointModel()
            // {
            //     FromCoordXToPixelXCoef = fromCoordXToPixelXCoef,
            //     FromPixelXToCoordXCoef = fromPixelXToCoordXCoef,

            //     FromCoordYToPixelYCoef = fromCoordYToPixelYCoef,
            //     FromPixelYToCoordYCoef = fromPixelYToCoordYCoef
            // };

            // return locationPointModel;

            return null;
        }

        public async Task<PositionDto> ImgToWorldPostion(PositionDto imgPostion)
        {
            // var firstPoint = (await dataContext.ScalePoints.Include(w => w.WorldPosition).Include(m => m.MapPosition).Take(1).ToListAsync())[0];

            // locationPointModel = await CalculateLocationModel();

            // double x3PixelCoord = imgPostion.Latitude; 
            // double y3PixelCoord = imgPostion.Longtitude;

            // double x1PixelCoord = firstPoint.MapPosition.Latitude;
            // double y1PixelCoord = firstPoint.MapPosition.Longtitude;

            // double x1WorldCoord = firstPoint.WorldPosition.Latitude;
            // double y1WorldCoord = firstPoint.WorldPosition.Longtitude;

            // double x3WorldCoord = x1WorldCoord + (x3PixelCoord - x1PixelCoord) * locationPointModel.FromPixelXToCoordXCoef;
            // double y3WorldCoord = y1WorldCoord + (y3PixelCoord - y1PixelCoord) * locationPointModel.FromPixelYToCoordYCoef;

            // System.Console.WriteLine("locationPointModel.FromCoordXToPixelXCoef: " + locationPointModel.FromPixelXToCoordXCoef);
            // System.Console.WriteLine("locationPointModel.FromCoordYToPixelYCoef: " + locationPointModel.FromPixelYToCoordYCoef);

            // return new PositionDto()
            // {
            //     Latitude = x3WorldCoord,
            //     Longtitude = y3WorldCoord
            //};
            return null;
        }

        public async Task<PositionDto> WorldToImgPostion(PositionDto worldPostion)
        {
            // var firstPoint = (await dataContext.ScalePoints.Include(w => w.WorldPosition).Include(m => m.MapPosition).Take(1).ToListAsync())[0];

            // locationPointModel = await CalculateLocationModel();

            // double x3WorldCoord = worldPostion.Latitude; 
            // double y3WorldCoord = worldPostion.Longtitude;

            // double x1WorldCoord = firstPoint.WorldPosition.Latitude;
            // double y1WorldCoord = firstPoint.WorldPosition.Longtitude;

            // double x1PixelCoord = firstPoint.MapPosition.Latitude;
            // double y1PixelCoord = firstPoint.MapPosition.Longtitude;

            // double x3PixelCoord = x1PixelCoord + (x3WorldCoord - x1WorldCoord) * locationPointModel.FromCoordXToPixelXCoef;
            // double y3PixelCoord = y1PixelCoord + (y3WorldCoord - y1WorldCoord) * locationPointModel.FromCoordYToPixelYCoef;

            // System.Console.WriteLine("locationPointModel.FromCoordXToPixelXCoef: " + locationPointModel.FromCoordXToPixelXCoef);
            // System.Console.WriteLine("locationPointModel.FromCoordYToPixelYCoef: " + locationPointModel.FromCoordYToPixelYCoef);

            // return new PositionDto()
            // {
            //     Latitude = x3PixelCoord,
            //     Longtitude = y3PixelCoord
            // };
            return null;
        }
    }
}