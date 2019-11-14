namespace DownloadsMonitor.Domain.Handlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Models;
    using Queries;

    public sealed class GetFileEntryQueryHandler : IRequestHandler<GetEntryQuery, FileEntry>
    {
        private readonly DownloadsContext context;
        private readonly ILogger<GetFileEntryQueryHandler> logger;

        public GetFileEntryQueryHandler(DownloadsContext context, ILogger<GetFileEntryQueryHandler> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<FileEntry> Handle(GetEntryQuery request, CancellationToken cancellationToken)
        {
            Contract.Assert(request != null);
            FileEntry response = null;

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