namespace DownloadsMonitor.Domain.Handlers
{
    using Microsoft.Extensions.Logging;
    using Models;
    using Queries;

    public class FileEntryLoggingBehavior : LoggingBehavior<GetEntryQuery, FileEntry>
    {
        public FileEntryLoggingBehavior(ILogger<LoggingBehavior<GetEntryQuery, FileEntry>> logger) : base(logger)
        {
        }
    }
}