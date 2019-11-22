namespace DownloadsMonitor.Domain.Queries
{
    using System.ComponentModel.DataAnnotations;
    using DownloadsMonitor.Models;
    using MediatR;

    public sealed class GetEntryQuery : IRequest<FileEntry>
    {
        public long Length { get; set; }

        [Required]
        public string Md5 { get; set; } = string.Empty;
    }
}