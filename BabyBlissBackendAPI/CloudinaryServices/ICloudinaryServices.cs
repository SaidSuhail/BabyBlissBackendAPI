namespace BabyBlissBackendAPI.CloudinaryServices
{
    public interface ICloudinaryServices
    {
        Task<string> UploadImageAsync(IFormFile file);

    }
}
