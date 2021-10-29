using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FireSaverApi.Contracts;
using FireSaverApi.Dtos.EvacuationPlanDtos;
using FireSaverApi.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace FireSaverApi.Services
{
    public class PlanImageUploadService : IPlanImageUploadService
    {
        private readonly CloudinarySettings cloudinarySettings;
        private Cloudinary cloudinary;

        public PlanImageUploadService(IOptions<CloudinarySettings> cloudinarySettings)
        {
            this.cloudinarySettings = cloudinarySettings.Value;

            var cloudinaryAccount = new Account(cloudinarySettings.Value.CloudName,
                                                cloudinarySettings.Value.APIKey,
                                                cloudinarySettings.Value.APISecret);

            cloudinary = new Cloudinary(cloudinaryAccount);
            cloudinary.Api.Secure = true;
        }

        public async Task DeletePlanImage(string planImagePublicId)
        {
            try
            {
                await cloudinary.DestroyAsync(new DeletionParams(planImagePublicId));
            }
            catch
            {
                throw new System.Exception("Something went wrong while deleting");
            }
        }

        public async Task<PlanUploadResponse> UploadPlanImage(IFormFile planImage)
        {
            if (planImage.Length <= 0)
            {
                throw new System.Exception("File can't have zero size");
            }

            ImageUploadResult result = null;
            using (var stream = planImage.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(planImage.FileName, stream)
                };
                result = await cloudinary.UploadAsync(uploadParams);
            }

            var response = new PlanUploadResponse()
            {
                PublicId = result.PublicId,
                Url = result.Url.ToString()
            };

            return response;

        }
    }
}