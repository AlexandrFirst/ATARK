using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FireSaverApi.Common;
using FireSaverApi.Contracts;
using FireSaverApi.Helpers;
using Microsoft.Extensions.Options;

namespace FireSaverApi.Services
{
    public class CompartmentDataCloudinaryService : CloudinaryService, ICompartmentDataCloudinaryService
    {
        public CompartmentDataCloudinaryService(IOptions<CloudinarySettings> cloudinarySettings)
            : base(cloudinarySettings) { }

        public async Task<string> DestroyFile(string publicId)
        {
            var result = await cloudinary.DestroyAsync(new DeletionParams(publicId));
            return result.Result;
        }

        public async Task<ImagePoint[,]> GetCompartmentData(string publicId)
        {
            var result = await cloudinary.GetResourceAsync(publicId);

            FileStream f = File.OpenRead(result.Url);
            StreamReader reader = new StreamReader(f);
            string line = "";

            List<List<ImagePoint>> listImagePoints = new List<List<ImagePoint>>();

            while ((line = await reader.ReadLineAsync()) != null)
            {
                List<ImagePoint> lineImagePoints = new List<ImagePoint>();
                char[] points = line.ToCharArray();
                for (int i = 0; i < points.GetLength(0); i++)
                {
                    lineImagePoints.Add(new ImagePoint() { data = int.Parse(points[i].ToString()) });
                }
            }

            int size0 = listImagePoints.Count;
            int size1 = listImagePoints[0].Count;
            ImagePoint[,] imagePoints = new ImagePoint[size1, size0];

            for (int i = 0; i < size0; i++)
            {
                for (int j = 0; j < size1; j++)
                {
                    imagePoints[j, i] = listImagePoints[i][j];
                }
            }

            return imagePoints;
        }

        public async Task<string> UpdateFile(ImagePoint[,] imagePoints, string publicId, string newFileName)
        {
            if ((await DestroyFile(publicId)).Equals("ok"))
            {
                return await UploadFile(imagePoints, newFileName);
            }
            return null;
        }

        public async Task<string> UpdateFile(Stream imageStream, string publicId, string newFileName)
        {
            if ((await DestroyFile(publicId)).Equals("ok"))
            {
                return await UploadFile(imageStream, newFileName);
            }
            return null;
        }

        public async Task<string> UploadFile(Stream stream, string fileName)
        {
            FileDescription file = new FileDescription(fileName, stream);

            RawUploadParams uploadParams = new RawUploadParams()
            {
                File = file
            };
            RawUploadResult uploadResult = await cloudinary.UploadAsync(uploadParams);
            return uploadResult.PublicId;

        }

        public async Task<string> UploadFile(ImagePoint[,] imagePoints, string fileName)
        {
            StringBuilder text = new StringBuilder();

            for (int i = 0; i < imagePoints.GetLength(0); i++)
            {
                string dataLine = "";
                for (int j = 0; j < imagePoints.GetLength(1); j++)
                {
                    dataLine += imagePoints[i, j].data.ToString();
                }
                text.AppendLine(dataLine);
            }

            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(ms, Encoding.UTF8, leaveOpen: true))
                {
                    foreach (ReadOnlyMemory<char> chunk in text.GetChunks())
                    {
                        await sw.WriteAsync(chunk);
                    }
                }
                return await UploadFile(ms, fileName);
            }
        }
    }
}