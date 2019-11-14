namespace DownloadsMonitor.Domain.Commands
{
    using MediatR;

    public sealed class DeleteFileCommand : IRequest
    {
        public string Name { get; set; }

        public string FullName { get; set; }
    }
}