using System;
using System.Collections.Generic;
using System.Linq;

namespace FireSaverApi.Common
{
    public class RouteBuilder
    {
        private readonly ImagePoint[,] availablePath;
        private readonly BlockedPoint[] blockedPoints;
        private readonly int waveStep;

        public RouteBuilder(
            ImagePoint[,] availablePoints,
            BlockedPoint[] blockedPoints,
            int waveStep = 1)
        {
            this.availablePath = availablePoints;
            this.blockedPoints = blockedPoints;
            this.waveStep = waveStep;
        }

        private WavePoint BuildWaves(WavePoint userPosition, List<Point> exits)
        {
            if (userPosition.X < 0 || userPosition.X > availablePath.GetLength(1) ||
                userPosition.Y < 0 || userPosition.Y > availablePath.GetLength(0))
            {
                int radius = 30;
                for (int x = radius; x >= -radius; x--)
                {
                    int y_threshold = radius - Math.Abs(x);
                    for (int y = y_threshold; y >= -y_threshold; y--)
                    {

                        ImagePoint imagePoint = RouteBuilderHelper.GetImagePoint(userPosition.X + x, userPosition.Y + y, availablePath);
                        if (imagePoint != null)
                        {
                            if (imagePoint.data == (int)PointType.Free)
                            {
                                userPosition.X = x;
                                userPosition.Y = y;
                                goto begin;
                            }

                        }
                    }
                }
            }
        begin:

            List<WavePoint> newFront = new List<WavePoint>();
            List<WavePoint> oldFront = new List<WavePoint>() { userPosition };

            Random random = new Random();
            while (true)
            {
                foreach (WavePoint oldFrontPoint in oldFront)
                {
                    List<WavePoint> tempNewFront = GetPointAround(oldFrontPoint);

                    foreach (WavePoint newFrontPoint in tempNewFront)
                    {
                        newFrontPoint.ParentPoint = oldFrontPoint;
                        newFrontPoint.WaveStep = oldFrontPoint.WaveStep + 1;
                        if (exits.Any(e => e.IsInRadiusOf(1, newFrontPoint)))
                        {
                            newFront.Clear();
                            oldFront.Clear();

                            return newFrontPoint;
                        }

                        if (RouteBuilderHelper.GetImagePoint(newFrontPoint.X, newFrontPoint.Y, availablePath).data == (int)PointType.Blocked)
                        {
                            newFrontPoint.DangerFactor++;
                        }

                        newFront.AddRange(tempNewFront);

                    }
                }
                if (newFront.Count == 0)
                    return null;

                Shuffle(newFront, random);

                oldFront.Clear();
                oldFront.AddRange(newFront);

                newFront.Clear();

            }
        }

        private void Shuffle<T>(IList<T> list, Random rng)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public Route BuildRoute(WavePoint currectUserPosition, List<Point> exits)
        {
            List<Route> routes = new List<Route>();
            while (true)
            {
                WavePoint endWavePoint = BuildWaves(currectUserPosition, exits);


                if (endWavePoint == null)
                {
                    break;
                }

                System.Console.WriteLine("End point: x - " + endWavePoint.X + "; y - " + endWavePoint.Y);

                Route currentRoute = new Route();
                Random rng = new Random();

                while (!endWavePoint.IsInRadiusOf(waveStep, currectUserPosition))
                {
                    var prevPoint = endWavePoint;
                    var nextPoint = endWavePoint.ParentPoint;

                    currentRoute.RouteLength++;
                    currentRoute.RoutePoints.Add(new Point { X = endWavePoint.X, Y = endWavePoint.Y });


                    if (endWavePoint.DangerFactor != 0)
                    {
                        currentRoute.DangerFactor += endWavePoint.DangerFactor;
                        foreach (var blockPoint in blockedPoints)
                        {
                            if (endWavePoint.IsInRadiusOf(blockPoint.Radius, blockPoint))
                            {
                                blockPoint.IsAlreadyVisited = true;
                            }
                        }
                    }
                    endWavePoint = endWavePoint.ParentPoint;
                }

                System.Console.WriteLine("Start point: x - " + endWavePoint.X + "; y - " + endWavePoint.Y);

                if (currentRoute.DangerFactor == 0 || routes.Select(r => r.RouteLength).Contains(currentRoute.RouteLength))
                {
                    routes.Add(currentRoute);
                    break;
                }

                routes.Add(currentRoute);

                for (int column = 0; column < availablePath.GetLength(0); column++)
                {
                    for (int row = 0; row < availablePath.GetLength(1); row++)
                    {
                        RouteBuilderHelper.GetImagePoint(row, column, availablePath).IsVisited = false;
                    }
                }
            }
            Route safestRoute = routes.FirstOrDefault();
            if (safestRoute == null)
            {
                throw new Exception("No routes are available");
            }
            double minFactor = double.MaxValue;

            foreach (var route in routes)
            {
                double currentFactor = Convert.ToDouble(route.DangerFactor);
                if (currentFactor < minFactor)
                {
                    safestRoute = route;
                    minFactor = currentFactor;
                }
            }

            return safestRoute;
        }

        private bool IsPointVisited(int x, int y)
        {
            return RouteBuilderHelper.GetImagePoint(x, y, availablePath).IsVisited;
        }
        private void AddWavePoint(int m_x, int m_y, List<WavePoint> pointsAround)
        {
            if (IsPointVisited(m_x, m_y))
                return;

            WavePoint newWavePoint = new WavePoint()
            {
                X = m_x,
                Y = m_y
            };

            if (blockedPoints.Any(p => p.IsInRadiusOf(p.Radius, newWavePoint) &&
                p.IsAlreadyVisited))
            {
                return;
            }

            pointsAround.Add(new WavePoint()
            {
                X = m_x,
                Y = m_y
            });
            RouteBuilderHelper.GetImagePoint(m_x, m_y, availablePath).IsVisited = true;
        }

        private List<WavePoint> GetPointAround(WavePoint currentPoint)
        {
            List<WavePoint> pointsAround = new List<WavePoint>();

            bool isUpDirectionAvailable = currentPoint.Y - waveStep >= 0 &&
                availablePath[currentPoint.Y - waveStep, currentPoint.X].data != (int)PointType.Wall;

            bool isDownDirectionAvailable = currentPoint.Y + waveStep < availablePath.GetLongLength(0) &&
                availablePath[currentPoint.Y + waveStep, currentPoint.X].data != (int)PointType.Wall;

            bool isRightDirectionAvailable = currentPoint.X + waveStep < availablePath.GetLongLength(1) &&
                availablePath[currentPoint.Y, currentPoint.X + waveStep].data != (int)PointType.Wall;

            bool isLeftDirectionAvailable = currentPoint.X - waveStep >= 0 &&
                availablePath[currentPoint.Y, currentPoint.X - waveStep].data != (int)PointType.Wall;


            if (isUpDirectionAvailable)
            {
                int m_x = currentPoint.X;
                int m_y = currentPoint.Y - waveStep;
                AddWavePoint(m_x, m_y, pointsAround);

                if (isRightDirectionAvailable)
                {
                    AddWavePoint(m_x + waveStep, m_y, pointsAround);
                }

                if (isLeftDirectionAvailable)
                {
                    AddWavePoint(m_x - waveStep, m_y, pointsAround);
                }
            }


            if (isDownDirectionAvailable)
            {
                int m_x = currentPoint.X;
                int m_y = currentPoint.Y + waveStep;
                AddWavePoint(m_x, m_y, pointsAround);

                if (isRightDirectionAvailable)
                {
                    AddWavePoint(m_x + waveStep, m_y, pointsAround);
                }

                if (isLeftDirectionAvailable)
                {
                    AddWavePoint(m_x - waveStep, m_y, pointsAround);
                }
            }

            if (isLeftDirectionAvailable)
            {
                int m_x = currentPoint.X - waveStep;
                int m_y = currentPoint.Y;
                AddWavePoint(m_x, m_y, pointsAround);

            }

            if (isRightDirectionAvailable)
            {
                int m_x = currentPoint.X + waveStep;
                int m_y = currentPoint.Y;
                AddWavePoint(m_x, m_y, pointsAround);
            }

            return pointsAround;
        }

    }
}