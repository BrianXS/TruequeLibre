using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting.Internal;

namespace API.Utils
{
    public static class Base64ToFile
    {
        public static void ConvertToFile(string image, string fileName)
        {
            var byteBuffer = Convert.FromBase64String(image);
            var fileType = image[0].Equals('/') ? ".jpg" : ".png";
            File.WriteAllBytes($"{Constants.General.InternalImagesFolder}/{fileName}{fileType}", byteBuffer);
        }

        public static string ConvertToBase64(string name)
        {
            var byteBuffer = File.ReadAllBytes($"{Constants.General.InternalImagesFolder}/{name}");
            return Convert.ToBase64String(byteBuffer);
        }
    }
}