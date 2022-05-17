using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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

        public async Task<ImagePointArray> GetCompartmentData(string publicId)
        {
            var result = await cloudinary.GetResourceAsync(new GetResourceParams(publicId) { ResourceType = ResourceType.Raw });

            List<List<ImagePoint>> listImagePoints = new List<List<ImagePoint>>();

            WebClient wc = new WebClient();
            using (StreamReader reader = new StreamReader(wc.OpenRead(new Uri(result.Url))))
            {
                string line = "";

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    List<ImagePoint> lineImagePoints = new List<ImagePoint>();
                    char[] points = line.ToCharArray();
                    for (int i = 0; i < points.GetLength(0); i++)
                    {
                        lineImagePoints.Add(new ImagePoint() { data = int.Parse(points[i].ToString()) });
                    }
                    listImagePoints.Add(lineImagePoints);
                }
            }
            int size0 = listImagePoints.Count - 1;
            int size1 = listImagePoints[0].Count - 1;
            ImagePoint[,] imagePoints = new ImagePoint[size0, size1];

            for (int i = 0; i < size0; i++)
            {
                for (int j = 0; j < size1; j++)
                {
                    imagePoints[i, j] = listImagePoints[i][j];
                }
            }

            return new ImagePointArray(imagePoints);
        }

        public async Task<string> UpdateFile(ImagePointArray imagePoints, string publicId)
        {
            return await UploadFile(imagePoints);
            if ((await DestroyFile(publicId)).Equals("ok"))
            {

            }
            return null;
        }

        public async Task<string> UpdateFile(Stream imageStream, string publicId)
        {
            return await UploadFile(imageStream);

            if ((await DestroyFile(publicId)).Equals("ok"))
            {

            }
            return null;
        }

        public async Task<string> UploadFile(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            FileDescription file = new FileDescription(Guid.NewGuid().ToString(), stream);
            RawUploadParams uploadParams = new RawUploadParams()
            {
                File = file
            };
            RawUploadResult uploadResult = await cloudinary.UploadAsync(uploadParams);
            return uploadResult.PublicId;

        }

        public async Task<string> UploadFile(ImagePointArray imagePoints)
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

            MemoryStream ms = new MemoryStream();

            StreamWriter sw = new StreamWriter(ms, Encoding.UTF8);


            foreach (ReadOnlyMemory<char> chunk in text.GetChunks())
            {
                await sw.WriteAsync(chunk);
            }
            return await UploadFile(sw.BaseStream);

        }
    }
}