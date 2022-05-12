using CloudinaryDotNet;
using FireSaverApi.Helpers;
using Microsoft.Extensions.Options;

namespace FireSaverApi.Services
{
    public class CloudinaryService
    {
        protected readonly CloudinarySettings cloudinarySettings;
        protected Cloudinary cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> cloudinarySettings)
        {
            this.cloudinarySettings = cloudinarySettings.Value;

            var cloudinaryAccount = new Account(cloudinarySettings.Value.CloudName,
                                                cloudinarySettings.Value.APIKey,
                                                cloudinarySettings.Value.APISecret);

            cloudinary = new Cloudinary(cloudinaryAccount);
            cloudinary.Api.Secure = true;
        }
    }
}