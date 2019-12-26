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
            File.WriteAllBytes($"{Constants.General.InternalImagesFolder}/{fileName}", byteBuffer);
        }
    }
}