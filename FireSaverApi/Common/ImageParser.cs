using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace FireSaverApi.Common
{
    public enum PointType
    {
        Exit = 4,
        Blocked = 3,
        Wall = 2,
        Free = 1,
        Unprocessed = 0
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

    public class BlockedPoint : Point
    {
        public bool IsAlreadyVisited = false;
        public int Radius { get; set; } = 30;
    }

    public class WavePoint : Point
    {
        public int WaveStep { get; set; } = 1;
        public int DangerFactor { get; set; } = 0;
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

    public class ImagePointArray
    {
        private readonly ImagePoint[,] imagePoints;

        public ImagePointArray(ImagePoint[,] imagePoints)
        {
            this.imagePoints = imagePoints;
        }

        public ImagePoint this[int index1, int index2]
        {
            get
            {
                return imagePoints[index1, index2];
            }
            set
            {
                imagePoints[index1, index2] = value;
            }
        }
        public int GetLength(int n)
        {
            return imagePoints.GetLength(n);
        }
    }

    public class ImageParser
    {
        private Stream dataImageStream;

        public ImageParser(Stream dataImageStream)
        {
            this.dataImageStream = dataImageStream;
        }

        public ImagePointArray ParseImage()
        {
            Bitmap bmp = new Bitmap(dataImageStream);
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            IntPtr ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;

            byte[] rgbValues = new byte[bytes];

            byte[] r = new byte[bytes / 3];
            byte[] g = new byte[bytes / 3];
            byte[] b = new byte[bytes / 3];

            Marshal.Copy(ptr, rgbValues, 0, bytes);

            int stride = bmpData.Stride;
            ImagePoint[,] availablePath = new ImagePoint[bmpData.Height, bmpData.Width];

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
            bmp.UnlockBits(bmpData);

            return new ImagePointArray(availablePath);
        }

        public static void BlockPoints(ref ImagePointArray imagePoints, BlockedPoint blockedPoint)
        {

            for (int x = blockedPoint.Radius; x >= -blockedPoint.Radius; x--)
            {
                int y_threshold = blockedPoint.Radius - Math.Abs(x);
                for (int y = y_threshold; y >= -y_threshold; y--)
                {
                    ImagePoint imagePoint = RouteBuilderHelper.GetImagePoint(blockedPoint.X + x, blockedPoint.Y + y, imagePoints);
                    if (imagePoint != null)
                    {
                        if (imagePoint.data == (int)PointType.Free)
                            imagePoint.data = (int)PointType.Blocked;
                    }
                }
            }

        }

        private (byte, byte, byte) GetColor(byte[] rgbValues, int x, int y)
        {
            byte red = (byte)(rgbValues[x + y]);
            byte green = (byte)(rgbValues[x + y + 1]);
            byte blue = (byte)(rgbValues[x + y + 2]);
            return (red, green, blue);
        }
    }
}