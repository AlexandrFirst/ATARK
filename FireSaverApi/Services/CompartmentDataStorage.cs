using System.Collections.Generic;
using FireSaverApi.Common;

namespace FireSaverApi.Services
{
    public class CompartmentDataStorage
    {
        Dictionary<int, ImagePoint[,]> LoadedCompartmentData;
        Dictionary<int, List<BlockedPoint>> blockedCompartmentArea;
        public CompartmentDataStorage()
        {
            LoadedCompartmentData = new Dictionary<int, ImagePoint[,]>();
            blockedCompartmentArea = new Dictionary<int, List<BlockedPoint>>();
        }

        public void LoadData(int compartmentId, ImagePoint[,] imagePoints)
        {
            LoadedCompartmentData.TryAdd(compartmentId, imagePoints);
        }

        public void AddBlockPointToCompartment(int compartmentId, BlockedPoint blockedPoint)
        {
            List<BlockedPoint> blockedPoints;
            if (!blockedCompartmentArea.TryGetValue(compartmentId, out blockedPoints))
            {
                blockedPoints = new List<BlockedPoint>();
                
                blockedPoints.Add(blockedPoint);
                blockedCompartmentArea.Add(compartmentId, blockedPoints);
            }
            else
            {
                blockedCompartmentArea[compartmentId].Add(blockedPoint);
            }
        }

        public ImagePoint[,] GetCompartmentDataById(int compartmentId)
        {
            if (LoadedCompartmentData.ContainsKey(compartmentId))
            {
                ImagePoint[,] compartmentData = LoadedCompartmentData[compartmentId];
                return compartmentData;
            }
            return null;
        }

        public BlockedPoint[] GetBlockedPointsByCompartmentId(int compartmentId)
        {
            if (blockedCompartmentArea.ContainsKey(compartmentId))
            {
                BlockedPoint[] blockedPoints = blockedCompartmentArea[compartmentId].ToArray();
                return blockedPoints;
            }
            return null;
        }

        public bool RemoveCompartmentData(int compartmentId)
        {
            bool isRemoved = false;
            isRemoved = LoadedCompartmentData.Remove(compartmentId);
            blockedCompartmentArea.Remove(compartmentId);
            return isRemoved;
        }
    }
}