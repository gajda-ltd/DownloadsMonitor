namespace DownloadsMonitor.Domain.Commands
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using MediatR;

    public sealed class AddFileEntryCommand : IRequest
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(256)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        public long Length { get; set; }

        [Required]
        [StringLength(48)]
        public string MD5 { get; set; } = string.Empty;
    }
}