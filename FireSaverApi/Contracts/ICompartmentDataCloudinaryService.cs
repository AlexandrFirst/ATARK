using System.IO;
using System.Threading.Tasks;
using FireSaverApi.Common;

namespace FireSaverApi.Contracts
{
    public interface ICompartmentDataCloudinaryService
    {
        Task<string> UploadFile(Stream stream);
        Task<string> UploadFile(ImagePoint[,] imagePoints);
        Task<string> UpdateFile(Stream imageStream, string publicId);
        Task<string> UpdateFile(ImagePoint[,] imagePoints, string publicId);
        Task<string> DestroyFile(string publicId);
        Task<ImagePoint[,]> GetCompartmentData(string PublicId);
    }
}