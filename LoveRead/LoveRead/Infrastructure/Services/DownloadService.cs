using System;
using System.IO;
using System.Linq;
using System.Net;

namespace LoveRead.Infrastructure.Services
{
    public class DownloadService : IDownloadService
    {
        private static readonly string ImgPath = Directory.GetCurrentDirectory() + "\\Img";

        public string DownloadFile(string url, string fileName, bool isAsync = false)
        {
            if (!Directory.Exists(ImgPath))
                Directory.CreateDirectory(ImgPath);

            if (new DirectoryInfo(ImgPath).GetFiles().Any(f => f.Name.Contains(fileName)))
                return $"{ImgPath}\\{fileName}.jpg";

            using (WebClient client = new WebClient())
            {
                if (isAsync)
                    client.DownloadFileAsync(new Uri(url), $"Img/{fileName}.jpg");
                else
                    client.DownloadFile(new Uri(url), $"Img/{fileName}.jpg");
            }

            return $"{ImgPath}\\{fileName}.jpg";
        }
    }

    public interface IDownloadService
    {
        string DownloadFile(string url, string fileName, bool isAsync = false);
    }
}
