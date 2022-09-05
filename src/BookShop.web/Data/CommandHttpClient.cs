using BookShop.shared;

namespace BookShop.web.Data; 

public class CommandHttpClient {
    private readonly HttpClient _httpClient;

    public CommandHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<CommandResponse?> GetCommand(string commandId)
    {
        return _httpClient.GetFromJsonAsync<CommandResponse>($"/api/Command/{commandId}");
    }
}