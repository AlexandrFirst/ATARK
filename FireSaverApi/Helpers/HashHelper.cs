using System;
using System.Security.Cryptography;
using System.Text;
using FireSaverApi.Dtos;

namespace FireSaverApi.Helpers
{
    public static class CalcHelper
    {
        public static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static double ComputeDistanceBetweenPoints(PositionDto p1, PositionDto p2)
        {
            var dist = Math.Sqrt(Math.Pow(p1.Latitude - p2.Latitude, 2) + Math.Pow(p1.Longtitude - p2.Longtitude, 2));
            return dist;
        }
    }
}