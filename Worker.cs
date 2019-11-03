namespace DownloadsMonitor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Extensions;

    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using Models;

    public sealed class Worker : BackgroundService
    {
        private const string DEFAULT_FOLDER = "Downloads";
        private volatile bool disposed;
        private readonly IReadOnlyList<string> extensions = new List<string> { ".azw", ".azw3", ".epub", ".mobi", ".pdf", };
        private readonly FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        public Worker(ILogger<Worker> logger)
        {
            this.fileSystemWatcher.Path = Path.Combine(Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.Personal)), DEFAULT_FOLDER);

            this.fileSystemWatcher.Created += (sender, e) =>
            {
                var fileInfo = new FileInfo(e.FullPath);

                if (this.extensions.Contains(fileInfo.Extension))
                {
                    try
                    {
                        using var context = new DownloadsContext();
                        var md5 = fileInfo.GetMD5();

                        if (context.Entries.Any(c => c.Length == fileInfo.Length && c.MD5 == md5))
                        {
                            logger.LogWarning("The '{name}' file was already downloaded.", fileInfo.Name);

                            try
                            {
                                logger.LogDebug("Deleting '{name}' file.", fileInfo.Name);
                                File.Delete(fileInfo.FullName);
                                logger.LogDebug("Deleted '{name}' file.", fileInfo.Name);
                            }
                            catch (Exception exception)
                            {
                                logger.LogError(exception.Message, exception);
                            }
                        }
                        else
                        {
                            context.Entries.Add(new FileEntry
                            {
                                FileName = fileInfo.Name,
                                Length = fileInfo.Length,
                                MD5 = md5,
                            });
                            context.SaveChanges();
                            logger.LogInformation("The '{name}' file was added.", fileInfo.Name);
                        }
                    }
                    catch (Exception exception)
                    {
                        logger.LogError(exception, exception.Message);
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
                        using var context = new DownloadsContext();
                        var md5 = fileInfo.GetMD5();

                        if (context.Entries.Any(c => c.Length == fileInfo.Length && c.MD5 == md5))
                        {
                            logger.LogWarning("The '{name}' file was already downloaded.", fileInfo.Name);
                            try
                            {
                                logger.LogDebug("Deleting '{name}' file.", fileInfo.Name);
                                File.Delete(fileInfo.FullName);
                                logger.LogDebug("Deleted '{name}' file.", fileInfo.Name);
                            }
                            catch (Exception exception)
                            {
                                logger.LogError(exception.Message, exception);
                            }
                        }
                        else
                        {
                            context.Entries.Add(new FileEntry { FileName = fileInfo.Name, Length = fileInfo.Length, MD5 = md5, });
                            context.SaveChanges();
                            logger.LogInformation("The '{name}' file was added.", fileInfo.Name);
                        }
                    }
                    catch (Exception exception)
                    {
                        logger.LogError(exception, exception.Message);
                    }
                }
            };
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            this.fileSystemWatcher.EnableRaisingEvents = true;

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(5, cancellationToken).ConfigureAwait(false);
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
