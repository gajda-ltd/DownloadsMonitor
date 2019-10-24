namespace DownloadsMonitor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Extensions;
    using Models;

    public sealed class Worker : BackgroundService
    {
        private volatile bool disposed;
        private readonly ILogger<Worker> logger;
        private readonly IReadOnlyList<string> extensions = new List<string> { ".azw", ".azw3", ".epub", ".mobi", ".pdf", };
        private readonly FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();

        public Worker(ILogger<Worker> logger)
        {
            this.logger = logger;

            this.fileSystemWatcher.Path = Path.Combine(Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.Personal)), "Downloads");

            this.fileSystemWatcher.Created += (sender, e) =>
            {
                var fileInfo = new FileInfo(e.FullPath);

                if (this.extensions.Contains(fileInfo.Extension))
                {
                    try
                    {
                        using (var context = new DownloadsContext())
                        {
                            var md5 = fileInfo.GetMD5();

                            if (context.Entries.Any(e => e.Length == fileInfo.Length && e.MD5 == md5))
                            {
                                this.logger.LogWarning($"The '{fileInfo.Name}' file was already downloaded.");
                                File.Delete(e.FullPath);
                            }
                            else
                            {
                                context.Entries.Add(new FileEntry { FileName = fileInfo.Name, Length = fileInfo.Length, MD5 = md5, });
                                context.SaveChanges();
                                this.logger.LogInformation($"The '{fileInfo.Name}' file was added.");
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        this.logger.LogError(exception, exception.Message);
                    }
                }
            };

            this.fileSystemWatcher.Changed += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(e.FullPath))
                {
                    return;
                }

                var fileInfo = new FileInfo(e.FullPath);

                if (this.extensions.Contains(fileInfo.Extension))
                {
                    try
                    {
                        using (var context = new DownloadsContext())
                        {
                            var md5 = fileInfo.GetMD5();

                            if (context.Entries.Any(e => e.Length == fileInfo.Length && e.MD5 == md5))
                            {
                                this.logger.LogWarning($"The '{fileInfo.Name}' file was already downloaded.");
                                File.Delete(e.FullPath);
                            }
                            else
                            {
                                context.Entries.Add(new FileEntry { FileName = fileInfo.Name, Length = fileInfo.Length, MD5 = md5, });
                                context.SaveChanges();
                                this.logger.LogInformation($"The '{fileInfo.Name}' file was added.");
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        this.logger.LogError(exception, exception.Message);
                    }
                }
            };
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            this.fileSystemWatcher.EnableRaisingEvents = true;

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(5, cancellationToken);
            }

            this.fileSystemWatcher.EnableRaisingEvents = false;
        }

        ~Worker()
        {
            this.Dispose(false);
        }

        public override void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.fileSystemWatcher.Dispose();
            }

            this.disposed = true;
        }
    }
}
