namespace DownloadsMonitor.Domain.Commands
{
    using System.ComponentModel.DataAnnotations;
    using MediatR;

    public sealed class DeleteFileCommand : IRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;
    }
}