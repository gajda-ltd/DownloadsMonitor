namespace DownloadsMonitor.Domain.Handlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Commands;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;
    using Models;
    using Queries;
    using Resources;

    public sealed class FileEntryHandlers : IRequestHandler<AddFileEntryCommand>, IRequestHandler<GetEntryQuery, FileEntry>
    {
        private readonly DownloadsContext context;
        private readonly IStringLocalizer localizer;
        private readonly ILogger<FileEntryHandlers> logger;

        public FileEntryHandlers(DownloadsContext context, IStringLocalizerFactory factory, ILogger<FileEntryHandlers> logger)
        {
            Contract.Assert(factory != null);
            var type = typeof(SharedResources);
            Contract.Requires(type != null);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            this.context = context;
            this.localizer = factory.Create("SharedResources", assemblyName.Name);
            this.logger = logger;
        }

        public async Task<Unit> Handle(AddFileEntryCommand request, CancellationToken cancellationToken)
        {
            Contract.Assert(request != null);
            await this.context.Entries.AddAsync(new FileEntry { FileName = request.FileName, Length = request.Length, MD5 = request.MD5, }, cancellationToken).ConfigureAwait(false);
            await this.context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            this.logger.LogInformation(this.localizer["FileWasAdded"], request.FileName);
            return Unit.Value;
        }

        public async Task<FileEntry> Handle(GetEntryQuery request, CancellationToken cancellationToken)
        {
            Contract.Assert(request != null);
            FileEntry response = default;

            try
            {
                response = await this.context.Entries
                    .FirstOrDefaultAsync(c => c.Length == request.Length && c.MD5 == request.Md5, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, exception.Message);
            }

            return response;
        }
    }
}