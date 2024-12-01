﻿using Newtonsoft.Json;
using SharedLibrary;

namespace MovieService.Events
{
    public class MovieViewedEventHandler : IIntegrationEventHandler<MovieViewedIntegrationEvent>
    {
        private readonly ILogger<MovieViewedEventHandler> _logger;

        public MovieViewedEventHandler(ILogger<MovieViewedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(MovieViewedIntegrationEvent @event)
        {
            _logger.LogInformation($"Movie viewed event received: {JsonConvert.SerializeObject(@event)}"); // Handle the event (e.g., update statistics, notify other services)
            return Task.CompletedTask;
        }
    }
}
