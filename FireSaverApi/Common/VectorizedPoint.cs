using System;

namespace FireSaverApi.Common
{
    public class VectorizedPoint
    {
        private readonly Point p1;
        private readonly Point p2;

        public readonly Point vectorCoord;


        public VectorizedPoint(Point p1, Point p2)
        {
            this.p1 = p1;
            this.p2 = p2;

            vectorCoord = p2 - p1;
        }

        public double GetAngle(VectorizedPoint other)
        {
            double angle = Math.Acos(Math.Cos((vectorCoord.X * other.vectorCoord.X + vectorCoord.Y * other.vectorCoord.Y)
            / (Math.Sqrt(Math.Pow(vectorCoord.X, 2) + Math.Pow(vectorCoord.Y, 2)) +
                Math.Sqrt(Math.Pow(other.vectorCoord.X, 2) + Math.Pow(other.vectorCoord.Y, 2)))));

            return angle * Math.PI / 180;
        }
    }
}