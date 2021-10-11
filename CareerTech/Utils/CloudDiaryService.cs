using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
namespace CareerTech.Utils
{
    public class CloudDiaryService
    {

        //API  CloudinaryUpload to upload Image  
        public static async Task<string> CloudinaryUpload(string imagePath, int Height, int width)
        {
            var myAccount = new Account { ApiKey = "467162261851367", ApiSecret = "yBylAxmAmsDrgQkCdzhbPfLJY6A", Cloud = "mockcareertech" };
            Cloudinary cloudinary = new Cloudinary(myAccount);
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imagePath),
                Transformation = new Transformation().Width(width).Height(Height).Crop("thumb").Gravity("face")
            };
            var uploadResult = await cloudinary.UploadAsync(uploadParams);
            return uploadResult.Url.AbsoluteUri;
        }

    }
}