namespace DownloadsMonitor.Domain.Handlers
{
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Commands;
    using MediatR;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;
    using Models;
    using Resources;

    public sealed class AddFileEntryCommandHandler : IRequestHandler<AddFileEntryCommand>
    {
        private readonly DownloadsContext context;
        private readonly IStringLocalizer localizer;
        private readonly ILogger<GetFileEntryQueryHandler> logger;

        public AddFileEntryCommandHandler(DownloadsContext context, IStringLocalizerFactory factory, ILogger<GetFileEntryQueryHandler> logger)
        {
            Contract.Assert(factory != null);
            var type = typeof(SharedResources);
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
    }
}
