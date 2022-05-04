using System.IO;
using System.Threading.Tasks;
using FireSaverApi.Common;

namespace FireSaverApi.Contracts
{
    public interface ICompartmentDataCloudinaryService
    {
        Task<string> UploadFile(Stream stream, string fileName);
        Task<string> UploadFile(ImagePoint[,] imagePoints, string fileName);
        Task<string> UpdateFile(Stream imageStream, string publicId, string newFileName);
        Task<string> UpdateFile(ImagePoint[,] imagePoints, string publicId, string newFileName);
        Task<string> DestroyFile(string publicId);
        Task<ImagePoint[,]> GetCompartmentData(string PublicId);
    }
}