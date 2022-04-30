using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace WalkingAreaBuilder
{
    class WavePoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int WaveStep { get; set; }
        public List<WavePoint> AdjacentPoints = new List<WavePoint>();
        public WavePoint ParentPoint { get; set; }


        public bool IsInRadiusOf(int radius, WavePoint other)
        {
            return Math.Pow(other.X - this.X, 2) + Math.Pow(other.Y - this.Y, 2) <= Math.Pow(radius, 2);
        }

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

    class ImagePoint
    {
        public int data { get; set; } = -1;
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
            X = 450,
            Y = 340
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
                            availablePath[column, row] = new ImagePoint() { data = 0 };
                    }
                    else
                    {
                        int borderWidth = 10;
                        for (int i = 0; i < borderWidth; i++)
                        {
                            if (column + i < bmpData.Height)
                            {
                                availablePath[column + i, row] = new ImagePoint() { data = 1 };
                            }

                            if (column - i > 0)
                            {
                                availablePath[column - i, row] = new ImagePoint() { data = 1 };
                            }
                            if (row - i > 0)
                            {
                                availablePath[column, row - i] = new ImagePoint() { data = 1 };
                            }
                            if (row + i < bmpData.Width)
                            {
                                availablePath[column, row + i] = new ImagePoint() { data = 1 };
                            }
                        }

                        availablePath[column, row] = new ImagePoint() { data = 1 };
                    }
                }
            }

            WavePoint currentPoint = start;
            System.Console.WriteLine("Original Start point: x - " + currentPoint.X + "; y - " + currentPoint.Y);
            WavePoint endWavePoint = BuildWaves(currentPoint);
            System.Console.WriteLine("End point: x - " + endWavePoint.X + "; y - " + endWavePoint.Y);
            if (endWavePoint == null)
            {
                System.Console.WriteLine("End point is not found");
                bmp.UnlockBits(bmpData);
                return;
            }

            Random rng = new Random();

            while (!endWavePoint.IsInRadiusOf(waveStep, start))
            {
                var prevPoint = endWavePoint;
                var nextPoint = endWavePoint.ParentPoint;
                for (int i = Math.Min(prevPoint.X, nextPoint.X), j = Math.Min(prevPoint.Y, nextPoint.Y);
                    i <= Math.Max(prevPoint.X, nextPoint.X) && j <= Math.Max(prevPoint.Y, nextPoint.Y);
                    i++, j++)
                {
                    if (i > Math.Max(prevPoint.X, nextPoint.X))
                        i = Math.Max(prevPoint.X, nextPoint.X);
                    if (j > Math.Max(prevPoint.Y, nextPoint.Y))
                        j = Math.Max(prevPoint.Y, nextPoint.Y);

                    SetColor(ref rgbValues,
                        y: j * stride, x: i * 3,
                        red: 255, green: 242, blue: 0);
                }

                endWavePoint = endWavePoint.ParentPoint;
            }
            System.Console.WriteLine("Start point: x - " + endWavePoint.X + "; y - " + endWavePoint.Y);

            Marshal.Copy(rgbValues, 0, ptr, bytes);
            bmp.UnlockBits(bmpData);
            bmp.Save("changedPng.png");
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
            return availablePath[y, x];
        }

        public static void AddWavePoint(int m_x, int m_y, List<WavePoint> pointsAround)
        {
            if (IsPointVisited(m_x, m_y))
                return;

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
                availablePath[currentPoint.Y - waveStep, currentPoint.X].data == 0;

            bool isDownDirectionAvailable = currentPoint.Y + waveStep < availablePath.GetLongLength(0) &&
                availablePath[currentPoint.Y + waveStep, currentPoint.X].data == 0;

            bool isRightDirectionAvailable = currentPoint.X + waveStep < availablePath.GetLongLength(1) &&
                availablePath[currentPoint.Y, currentPoint.X + waveStep].data == 0;

            bool isLeftDirectionAvailable = currentPoint.X - waveStep >= 0 &&
                availablePath[currentPoint.Y, currentPoint.X - waveStep].data == 0;


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
