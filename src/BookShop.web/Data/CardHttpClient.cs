using System.Net;
using System.Text.Json.Serialization;

namespace BookShop.web.Data; 

public class CardHttpClient {
    private readonly HttpClient _httpClient;
    
    public CardHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CardValidation> GetCardAction(Card card)
    {
        var response = await _httpClient.PostAsync("/api/Payment/card/validation", JsonContent.Create(card));

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return new CardValidation.InvalidCard(card);
        }

        response.EnsureSuccessStatusCode();

        var action = await response.Content.ReadFromJsonAsync<CardValidation>();

        if (action is null)
        {
            throw new Exception("Api returned an empty payload");
        }
        
        return action;
    }

    
    public async Task<string?> Validate3DS2(Card card, string user, string password)
    {
        var response = await _httpClient.PostAsync("/api/Payment/card/3ds2", JsonContent.Create(new
        {
            Card = card,
            User = user,
            Password = password
        }));

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<string>();
    }
}

public record CardValidation(
    [property:JsonConverter(typeof(JsonStringEnumConverter))]
    CardValidationType Type, 
    string? WebSiteUrl, 
    string? PaymentHash) {
    public record InvalidCard(Card Card) : CardValidation(CardValidationType.None, null, null);
};

public enum CardValidationType {
    None, 
    // ReSharper disable once InconsistentNaming
    With3DS1,
    // ReSharper disable once InconsistentNaming
    With3DS2    
}