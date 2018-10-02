using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace core_deamon.core
{


    /// <summary>
    /// Created with the help of this page:  https://dejanstojanovic.net/aspnet/2018/june/clean-service-stop-on-linux-with-net-core-21/
    /// </summary>
    class core_deamon_process : IHostedService
    {
        IApplicationLifetime appLifetime;
        ILogger<core_deamon_process> logger;
        IHostingEnvironment environment;
        IConfiguration configuration;


        public core_deamon_process(IConfiguration configuration, ILogger<core_deamon_process> logger, IHostingEnvironment environment, IApplicationLifetime appLifetime)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.appLifetime = appLifetime;
            this.environment = environment;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //init
            this.logger.LogInformation("StartAsync method called.");
            this.appLifetime.ApplicationStarted.Register(OnStarted);
            this.appLifetime.ApplicationStopping.Register(OnStopping);
            this.appLifetime.ApplicationStopped.Register(OnStopped);

            logger.LogDebug("starting thread");
            var sleep = configuration.GetValue<int>("coreSettings:sleepms");
            var iterations = configuration.GetValue<int>("coreSettings:iterations");
            var counter = 0;
            while (counter < iterations)
            {
                Thread.Sleep(sleep);
                logger.LogInformation("finished task");
            }

            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            this.logger.LogInformation("OnStarted method called");

            // Post-startup code goes here  
        }

        private void OnStopping()
        {
            this.logger.LogInformation("OnStopping method called.");

            // On-stopping code goes here  
        }

        private void OnStopped()
        {
            this.logger.LogInformation("OnStopped method called.");

            // Post-stopped code goes here  
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("StopAsync method called.");
            return Task.CompletedTask;
        }
    }
}
