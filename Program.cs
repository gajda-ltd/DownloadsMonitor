namespace DownloadsMonitor
{
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Models;

    internal static class Program
    {
        internal static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLocalization(options => options.ResourcesPath = "Resources");
                    services.AddLogging(loggingBuilder => { loggingBuilder.AddSeq(); });
                    services.AddDbContext<DownloadsContext>();
                    services.AddMediatR(typeof(Program).Assembly);
                    services.AddHostedService<Worker>();
                });
    }
}
