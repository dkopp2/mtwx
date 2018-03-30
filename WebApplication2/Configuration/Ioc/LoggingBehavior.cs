using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Serilog;

namespace Mtwx.Web.Configuration.Ioc
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger;


        public LoggingBehavior(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = default(TResponse);
            try
            {
                _logger.Information($"Handling {typeof(TRequest).Name}");
                response = await next();
                _logger.Information($"Handled {typeof(TResponse).Name}");

                return response;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unable to handle the request");
            }

            return response;
        }
    }
}