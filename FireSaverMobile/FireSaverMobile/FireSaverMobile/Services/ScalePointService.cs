using FireSaverMobile.Contracts;
using FireSaverMobile.Helpers;
using FireSaverMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FireSaverMobile.Services
{
    public class ScalePointService : BaseHttpService, IScalePointService
    {
        public async Task<bool> UpdateScalepointWorldPosition(Position newWorldPosition, int scalePointId)
        {
            var response = await newWorldPosition.PutRequest(client, $"http://{serverAddr}/ScalePoints/points/singlePoint/{scalePointId}");
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }
    }
}
