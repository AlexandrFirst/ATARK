using FireSaverMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FireSaverMobile.Contracts
{
    public interface IScalePointService
    {
        Task<bool> UpdateScalepointWorldPosition(Position newWorldPosition, int scalePointId);
    }
}
