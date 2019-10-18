namespace DownloadsMonitor.Extensions
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    public static class FileInfoExtensions
    {
        public static string GetMD5(this FileInfo self)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(self.FullName))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream));
                }
            }
        }
    }
}