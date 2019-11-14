namespace DownloadsMonitor.Domain.Queries
{
    using DownloadsMonitor.Models;
    using MediatR;

    public sealed class GetEntryQuery : IRequest<FileEntry>
    {
        public long Length { get; set; }

        public string Md5 { get; set; }
    }
}