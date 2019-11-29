namespace DownloadsMonitor.Domain.Queries
{
    using System.ComponentModel.DataAnnotations;
    using MediatR;
    using Models;

    public sealed class GetEntryQuery : IRequest<FileEntry>
    {
        [Required]
        public long Length { get; set; }

        [Required]
        public string Md5 { get; set; } = string.Empty;
    }
}
