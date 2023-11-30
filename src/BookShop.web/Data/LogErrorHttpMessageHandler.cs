using System.Text.Json;
using BookShop.shared;

namespace BookShop.web.Data; 

public class LogErrorHttpMessageHandler : DelegatingHandler {
    private readonly ILogger<LogErrorHttpMessageHandler> _logger;

    public LogErrorHttpMessageHandler(ILogger<LogErrorHttpMessageHandler> logger)
    {
        _logger = logger;

    }
    
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            var apiError = JsonSerializer.Deserialize<ApiError>(content);

            if (apiError is not null && !string.IsNullOrWhiteSpace(apiError.Message))
            {
                LogError("Route {Method} {Route} returned a non successful status code {StatusCode} with Error : {Error}", request.Method.Method, request.RequestUri, response.StatusCode, apiError.Message);
            }
            else
            {
                LogError("Route {Method} {Route} returned a non successful status code {StatusCode} with Payload : {Payload}", request.Method.Method, request.RequestUri, response.StatusCode, content);    
            }

            return response;
        }
        catch (Exception ex)
        {
            LogException(ex,
                "Route {Method} {Route} threw an exception", request.Method.Method, request.RequestUri);
            throw;
        }
    }

    private void LogException(Exception ex, string messageTemplate, params object?[] args)
    {
        _logger.LogError(ex, messageTemplate, args);
    }
    
    private void LogError(string messageTemplate, params object?[] args)
    {
        _logger.LogError(messageTemplate, args);
    }
}