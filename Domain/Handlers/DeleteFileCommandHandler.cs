namespace DownloadsMonitor.Domain.Handlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Commands;
    using MediatR;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;
    using Resources;

    public sealed class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand>
    {
        private readonly IStringLocalizer localizer;

        private readonly ILogger<DeleteFileCommandHandler> logger;

        public DeleteFileCommandHandler(IStringLocalizerFactory factory, ILogger<DeleteFileCommandHandler> logger)
        {
            Contract.Assert(factory != null);
            var type = typeof(SharedResources);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            this.localizer = factory.Create("SharedResources", assemblyName.Name);
            this.logger = logger;
        }

        public async Task<Unit> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            Contract.Assert(request != null);
            this.logger.LogWarning(this.localizer["FileAlreadyDownloaded"], request.FullName);

            try
            {
                this.logger.LogDebug(this.localizer["DeletingFile"], request.Name);
                await Task.Run(() => { File.Delete(request.FullName); }, cancellationToken).ConfigureAwait(false);
                this.logger.LogDebug(this.localizer["DeletedFile"], request.Name);
            }
            catch (Exception exception)
            {
                logger.LogError(exception.Message, exception);
            }

            return Unit.Value;
        }
    }
}