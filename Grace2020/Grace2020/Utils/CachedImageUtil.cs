using FFImageLoading;
using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Grace2020.Utils
{
    public class CustomStreamImageSource : StreamImageSource
    {
        public string Key { get; set; }
    }

    public class CustomCacheKeyFactory : ICacheKeyFactory
    {
        public string GetKey(ImageSource imageSource, object bindingContext)
        {
            if(imageSource is UriImageSource uri)
            {
                return $"{uri.Uri}+Uri";
            }
            else if(imageSource is FileImageSource file)
            {
                return $"{file.File}+File";
            }
            else
            {
                return null;
            }
        }
    }

    public class CachedImageUtil
    {
        public static async Task<byte[]> GetByteArrayAsync(string url)
        {
            byte[] byteArray;
            var cacheStream = await ImageService.Instance.Config.DownloadCache.DownloadAndCacheIfNeededAsync(
                url,
                ImageService.Instance.LoadUrl(url),
                ImageService.Instance.Config,
                CancellationToken.None);

            using (cacheStream.ImageStream)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await cacheStream.ImageStream.CopyToAsync(memoryStream);
                    byteArray = memoryStream.ToArray();
                }
            }

            return byteArray;
        }
    }
}
