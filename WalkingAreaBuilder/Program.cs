using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace WalkingAreaBuilder
{

    public enum PointType
    {
        Exit = 10,
        Blocked = 20,
        Wall = 1,
        Free = 0,
        Unprocessed = -1
    }


    public class Route
    {
        public int DangerFactor { get; set; }
        public int RouteLength { get; set; }
        public List<Point> RoutePoints { get; set; } = new List<Point>();
    }

    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool IsInRadiusOf(int radius, Point other)
        {
            return Math.Pow(other.X - this.X, 2) + Math.Pow(other.Y - this.Y, 2) <= Math.Pow(radius, 2);
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point() { X = b.X - a.X, Y = b.Y - a.Y };
        }

    }



    public class BlockedPoints : Point
    {
        public bool IsAlreadyVisited = false;
        public int Radius { get; set; } = 30;
    }

    public class WavePoint : Point
    {
        public int WaveStep { get; set; }
        public int DangerFactor { get; set; }
        public List<WavePoint> AdjacentPoints = new List<WavePoint>();
        public WavePoint ParentPoint { get; set; }




        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            WavePoint other = obj as WavePoint;

            if (this == other)
                return true;

            return this.X == other.X && this.Y == other.Y;
        }

        public override int GetHashCode()
        {
            int hashcode = 1430287;
            hashcode = hashcode * 7302013 ^ X.GetHashCode();
            hashcode = hashcode * 7302013 ^ Y.GetHashCode();
            hashcode = hashcode * 7302013 ^ WaveStep.GetHashCode();
            return hashcode;
        }
    }

    public class ImagePoint
    {
        public int data { get; set; } = (int)PointType.Unprocessed;
        private bool isVisited = false;
        public bool IsVisited { get { return isVisited; } set { isVisited = value; } }

    }



    class Program
    {
        static ImagePoint[,] availablePath;
        static byte[] rgbValues = new byte[0];
        static int stride = 0;
        static int waveStep = 1;
        static WavePoint start = new WavePoint()
        {
            X = 600,
            Y = 300
        },
        end = new WavePoint()
        {
            X = 70,
            Y = 300
        };

        static List<BlockedPoints> blockedPoints = new List<BlockedPoints>()
        {
            // new BlockedPoints()
            // {
            //     X = 480,
            //     Y = 220,
            //     Radius = 40
            // },
            // new BlockedPoints()
            // {
            //     X = 480,
            //     Y = 75,
            //     Radius = 40
            // }
        };

        static void Main(string[] args)
        {
            String filePath = "evacPlan.png";
            Bitmap bmp = new Bitmap(filePath);

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            IntPtr ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            rgbValues = new byte[bytes];
            byte[] r = new byte[bytes / 3];
            byte[] g = new byte[bytes / 3];
            byte[] b = new byte[bytes / 3];

            Marshal.Copy(ptr, rgbValues, 0, bytes);

            stride = bmpData.Stride;

            availablePath = new ImagePoint[bmpData.Height, bmpData.Width];

            for (int column = 0; column < bmpData.Height; column++)
            {
                for (int row = 0; row < bmpData.Width; row++)
                {
                    (byte red, byte green, byte blue) = GetColor(rgbValues, column * stride, row * 3);
                    if (red == 255 && green == 255 && blue == 255)
                    {
                        if (availablePath[column, row] == null)
                            availablePath[column, row] = new ImagePoint() { data = (int)PointType.Free };
                    }
                    else
                    {
                        int borderWidth = 10;
                        for (int i = 0; i < borderWidth; i++)
                        {
                            if (column + i < bmpData.Height)
                            {
                                availablePath[column + i, row] = new ImagePoint() { data = (int)PointType.Wall };
                            }

                            if (column - i > 0)
                            {
                                availablePath[column - i, row] = new ImagePoint() { data = (int)PointType.Wall };
                            }
                            if (row - i > 0)
                            {
                                availablePath[column, row - i] = new ImagePoint() { data = (int)PointType.Wall };
                            }
                            if (row + i < bmpData.Width)
                            {
                                availablePath[column, row + i] = new ImagePoint() { data = (int)PointType.Wall };
                            }
                        }

                        availablePath[column, row] = new ImagePoint() { data = (int)PointType.Wall };
                    }
                }
            }


            for (int i = 0; i < blockedPoints.Count; i++)
            {
                for (int x = blockedPoints[i].Radius; x >= -blockedPoints[i].Radius; x--)
                {
                    int y_threshold = blockedPoints[i].Radius - Math.Abs(x);
                    for (int y = y_threshold; y >= -y_threshold; y--)
                    {
                        SetColor(ref rgbValues, y: (blockedPoints[i].X + x) * 3, x: (blockedPoints[i].Y + y) * stride,
                            red: 255, green: 0, blue: 0);
                        ImagePoint imagePoint = GetImagePoint(blockedPoints[i].X + x, blockedPoints[i].Y + y);
                        if (imagePoint != null)
                        {
                            if (imagePoint.data == (int)PointType.Free)
                                imagePoint.data = (int)PointType.Blocked;
                        }
                    }
                }
            }

            WavePoint currentPoint = start;
            System.Console.WriteLine("Original Start point: x - " + currentPoint.X + "; y - " + currentPoint.Y);

            Route currentRoute = new Route();
            List<Route> routes = new List<Route>();
            bool success = true;
            while (true)
            {
                WavePoint endWavePoint = BuildWaves(currentPoint);


                if (endWavePoint == null)
                {
                    System.Console.WriteLine("End point is not found");
                    Marshal.Copy(rgbValues, 0, ptr, bytes);
                    bmp.UnlockBits(bmpData);
                    bmp.Save("changedPng1.png");

                    success = false;

                    break;
                }

                System.Console.WriteLine("End point: x - " + endWavePoint.X + "; y - " + endWavePoint.Y);

                currentRoute = new Route();
                Random rng = new Random();
                List<Point> routePoints = new List<Point>();

                while (!endWavePoint.IsInRadiusOf(waveStep, start))
                {

                    var prevPoint = endWavePoint;
                    var nextPoint = endWavePoint.ParentPoint;

                    routePoints.Add(endWavePoint);

                    SetColor(ref rgbValues,
                        y: prevPoint.Y * stride, x: prevPoint.X * 3,
                        red: 255, green: 0, blue: 0);

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

                    List<Point> optimizedRoute = OptimizeRoot(routePoints, endWavePoint);

                    for(int i = 0; i < 3; i ++)
                    {
                        optimizedRoute = OptimizeRoot(optimizedRoute, endWavePoint);
                    }

                    for (int i = 0; i < optimizedRoute.Count - 1; i++)
                    {
                        drawLine(optimizedRoute[i], optimizedRoute[i + 1]);
                    }

                    System.Console.WriteLine("Optimized route point size: " + optimizedRoute.Count);
                    System.Console.WriteLine("Initial route point size: " + routePoints.Count);

                    break;
                }

                routes.Add(currentRoute);

                for (int column = 0; column < bmpData.Height; column++)
                {
                    for (int row = 0; row < bmpData.Width; row++)
                    {
                        GetImagePoint(row, column).IsVisited = false;
                    }
                }

            }
            System.Console.WriteLine("Total route count: " + routes.Count);


            Route safestRoute = routes.First();
            double minFactor = Double.MaxValue;

            foreach (var route in routes)
            {
                double currentFactor = Convert.ToDouble(route.DangerFactor);
                if (currentFactor < minFactor)
                {
                    safestRoute = route;
                    minFactor = currentFactor;
                }
                System.Console.WriteLine("Route length: " + route.RouteLength +
                    "; danger factor: " + route.DangerFactor +
                    "; final factor: " + currentFactor);
            }

            System.Console.WriteLine("Safest route factor: " + minFactor);

            if (success)
            {
                Marshal.Copy(rgbValues, 0, ptr, bytes);
                bmp.UnlockBits(bmpData);
                bmp.Save("changedPng1.png");
            }
        }


        public static List<Point> OptimizeRoot(List<Point> routePoints, Point endWavePoint)
        {
            List<Point> optimizedRoute = new List<Point>();
            optimizedRoute.Add(routePoints.First());

            VectorizedPoint vectorizedPoint = new VectorizedPoint(optimizedRoute.Last(), routePoints[1]);

            for (int i = 2; i < routePoints.Count; i++)
            {
                VectorizedPoint newPoint = new VectorizedPoint(optimizedRoute.Last(), routePoints[i]);
                double angle = vectorizedPoint.GetAngle(newPoint);
                //System.Console.WriteLine("Angle size " + angle.ToString());
                if (angle > 0.0001)
                {
                    optimizedRoute.Add(routePoints[i]);
                    if (i + 1 < routePoints.Count)
                    {
                        vectorizedPoint = new VectorizedPoint(optimizedRoute.Last(), routePoints[++i]);
                        //System.Console.WriteLine(optimizedRoute.Last().X + ";" + optimizedRoute.Last().Y);
                    }
                }

            }
            if (optimizedRoute.Last() != (endWavePoint as Point))
            {
                optimizedRoute.Add(endWavePoint);
            }
            return optimizedRoute;
        }
        public static void drawLine(Point p1, Point p2)
        {
            double a = p2.Y - p1.Y;
            double b = p2.X - p1.X;
            double z = a * p1.X - p1.Y * b;

            Func<double, double> f = (double x) =>
            {
                return x * (a / b) - (z / b);
            };

            for (int x = Math.Min(p1.X, p2.X); x <= Math.Max(p1.X, p2.X); x++)
            {
                double y = f(x);
                SetColor(ref rgbValues,
                       y: (int)y * stride, x: x * 3,
                       red: 0, green: 0, blue: 255);

            }
        }

        public static (byte, byte, byte) GetColor(byte[] rgbValues, int x, int y)
        {
            byte red = (byte)(rgbValues[x + y]);
            byte green = (byte)(rgbValues[x + y + 1]);
            byte blue = (byte)(rgbValues[x + y + 2]);
            return (red, green, blue);
        }

        public static void SetColor(ref byte[] rgbValues, int x, int y, byte red, byte green, byte blue)
        {
            rgbValues[x + y] = red;
            rgbValues[x + y + 1] = green;
            rgbValues[x + y + 2] = blue;
        }

        public static void Shuffle<T>(IList<T> list, Random rng)
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

        public static WavePoint BuildWaves(WavePoint currentPoint)
        {
            List<WavePoint> newFront = new List<WavePoint>();
            List<WavePoint> oldFront = new List<WavePoint>() { currentPoint };
            Random random = new Random();
            while (true)
            {
                byte red = (byte)random.Next(0, 255);
                byte green = (byte)random.Next(0, 255);
                byte blue = (byte)random.Next(0, 255);

                foreach (WavePoint oldFrontPoint in oldFront)
                {
                    List<WavePoint> tempNewFront = GetPointAround(oldFrontPoint);

                    foreach (WavePoint newFrontPoint in tempNewFront)
                    {
                        newFrontPoint.ParentPoint = oldFrontPoint;
                        newFrontPoint.WaveStep = oldFrontPoint.WaveStep + 1;
                        if (newFrontPoint.IsInRadiusOf(waveStep, end))
                        {
                            newFront.Clear();
                            oldFront.Clear();

                            return newFrontPoint;
                        }

                        if (GetImagePoint(newFrontPoint.X, newFrontPoint.Y).data == (int)PointType.Blocked)
                        {
                            newFrontPoint.DangerFactor++;
                        }

                        SetColor(ref rgbValues,
                    y: newFrontPoint.Y * stride, x: newFrontPoint.X * 3,
                    red: red, green: green, blue: blue);
                    }
                    newFront.AddRange(tempNewFront);

                }
                if (newFront.Count == 0)
                    return null;

                Shuffle(newFront, random);

                oldFront.Clear();
                oldFront.AddRange(newFront);

                newFront.Clear();

            }
        }


        public static ImagePoint GetImagePoint(int x, int y)
        {
            if (y < 0)
                return null;
            else if (y >= availablePath.GetLength(0))
                return null;
            else if (x < 0)
                return null;
            else if (x >= availablePath.GetLength(1))
                return null;
            else
                return availablePath[y, x];
        }

        public static void AddWavePoint(int m_x, int m_y, List<WavePoint> pointsAround)
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
            GetImagePoint(m_x, m_y).IsVisited = true;
        }

        public static bool IsPointVisited(int x, int y)
        {
            return GetImagePoint(x, y).IsVisited;
        }

        public static List<WavePoint> GetPointAround(WavePoint currentPoint)
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
