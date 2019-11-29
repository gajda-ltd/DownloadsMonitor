namespace DownloadsMonitor.Domain.Handlers
{
    using System.Diagnostics.Contracts;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public abstract class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            this.logger = logger;
        }

        public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            Contract.Requires(next != null);
            this.logger.LogInformation($"Handling {typeof(TRequest).Name}");
            var response = await next().ConfigureAwait(true);
            this.logger.LogInformation($"Handled {typeof(TResponse).Name}");

            return response;
        }
    }
}
