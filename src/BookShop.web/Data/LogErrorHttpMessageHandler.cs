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

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);

                _logger.LogError("Route {Route} returned a non successfull status code {StatusCode} with Payload : {Payload}", request.RequestUri, response.StatusCode, errorContent);
            }
            
            return response;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}