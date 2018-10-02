﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace core_deamon.core
{
    public class ApplicationLifetimeHostedService : IHostedService
    {
        IApplicationLifetime appLifetime;
        IHostingEnvironment environment;
        IConfiguration configuration;


        public ApplicationLifetimeHostedService(
            IConfiguration configuration,
            IHostingEnvironment environment,
            IApplicationLifetime appLifetime)
        {
            this.configuration = configuration;
            this.appLifetime = appLifetime;
            this.environment = environment;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //("StartAsync method called.");

            this.appLifetime.ApplicationStarted.Register(OnStarted);
            this.appLifetime.ApplicationStopping.Register(OnStopping);
            this.appLifetime.ApplicationStopped.Register(OnStopped);

            return Task.CompletedTask;

        }

        private void OnStarted()
        {
           // this.logger.LogInformation("OnStarted method called.");

            // Post-startup code goes here  
        }

        private void OnStopping()
        {
           // this.logger.LogInformation("OnStopping method called.");

            // On-stopping code goes here  
        }

        private void OnStopped()
        {
            //this.logger.LogInformation("OnStopped method called.");

            // Post-stopped code goes here  
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            //this.logger.LogInformation("StopAsync method called.");

            return Task.CompletedTask;
        }
    }
}
