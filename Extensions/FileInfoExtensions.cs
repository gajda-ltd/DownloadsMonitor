namespace DownloadsMonitor.Extensions
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Security.Cryptography;

    public static class FileInfoExtensions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA5351:Do Not Use Broken Cryptographic Algorithms", Justification = "<Pending>")]
        public static string GetMD5(this FileInfo self)
        {
            Contract.Assert(self != null);
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(self.FullName);
            return BitConverter.ToString(md5.ComputeHash(stream));
        }
    }
}