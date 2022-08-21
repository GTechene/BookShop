using System.Net;
using System.Text.Json.Serialization;

namespace BookShop.web.Data; 

public class CardHttpClient {
    private readonly HttpClient _httpClient;
    
    public CardHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CardAction> GetCardAction(Card card)
    {
        var response = await _httpClient.PostAsync("/api/Bank/card", JsonContent.Create(card));

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return new CardAction.InvalidCard(card);
        }

        response.EnsureSuccessStatusCode();

        var action = await response.Content.ReadFromJsonAsync<CardAction>();

        if (action is null)
        {
            throw new Exception("Api returned an empty payload");
        }
        
        return action;
    }

    
    public async Task<string?> Validate3DS2(Card card, string user, string password)
    {
        var response = await _httpClient.PostAsync("/api/Bank/card/3ds2", JsonContent.Create(new
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

public record CardAction(
    [property:JsonConverter(typeof(JsonStringEnumConverter))]
    CardActionType Type, 
    string? WebSiteUrl, 
    string? PaymentHash) {
    public record InvalidCard(Card Card) : CardAction(CardActionType.NoValidationRequired, null, null);
};

public enum CardActionType {
    NoValidationRequired, 
    // ReSharper disable once InconsistentNaming
    ValidateWith3DS1,
    // ReSharper disable once InconsistentNaming
    ValidateWith3DS2    
}