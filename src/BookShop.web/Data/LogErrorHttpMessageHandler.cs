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
            
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);

            _logger.LogError("Route {Method} {Route} returned a non successful status code {StatusCode} with Payload : {Payload}", request.Method.Method, request.RequestUri, response.StatusCode, errorContent);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Route {Method} {Route} threw an exception", request.Method.Method, request.RequestUri);
            throw;
        }
    }
}