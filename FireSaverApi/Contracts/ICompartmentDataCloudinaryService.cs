using System.IO;
using System.Threading.Tasks;
using FireSaverApi.Common;

namespace FireSaverApi.Contracts
{
    public interface ICompartmentDataCloudinaryService
    {
        Task<string> UploadFile(Stream stream);
        Task<string> UploadFile(ImagePointArray imagePoints);
        Task<string> UpdateFile(Stream imageStream, string publicId);
        Task<string> UpdateFile(ImagePointArray imagePoints, string publicId);
        Task<string> DestroyFile(string publicId);
        Task<ImagePointArray> GetCompartmentData(string PublicId);
    }
}