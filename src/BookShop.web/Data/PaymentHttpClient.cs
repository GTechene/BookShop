using System.Net;
using System.Text.Json.Serialization;

namespace BookShop.web.Data; 

public class PaymentHttpClient {
    private readonly HttpClient _httpClient;
    
    public PaymentHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private record ActionResult<T>(T Result); 
    public async Task<PaymentValidation> ValidatePayment(Card card, Price price)
    {
        var response = await _httpClient.PostAsync("/api/Payment/validation", JsonContent.Create(new
        {
            card,
            price
        }));

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return new PaymentValidation.InvalidCard(card);
        }

        response.EnsureSuccessStatusCode();

        var action = await response.Content.ReadFromJsonAsync<ActionResult<PaymentValidation>>();

        if (action is null)
        {
            throw new Exception("Api returned an empty payload");
        }
        
        return action.Result;
    }

    
    public async Task<PaymentValidation?> Validate3DS2(Card card, Price price, string user, string password)
    {
        var response = await _httpClient.PostAsync("/api/Payment/3ds2", JsonContent.Create(new
        {
            price,
            card,
            user,
            password
        }));

        if (!response.IsSuccessStatusCode)
        {
            return new PaymentValidation.PaymentRefused();
        }

        var validation = await response.Content.ReadFromJsonAsync<PaymentValidation>();

        if (validation is null)
        {
            throw new InvalidOperationException();
        }
        
        return validation;
    }
}


public record PaymentValidation(
    [property:JsonConverter(typeof(JsonStringEnumConverter))]
    PaymentValidationType Type,
    string? Payment,
    string? RedirectUrl) {
    public record InvalidCard(Card Card) : PaymentValidation(PaymentValidationType.None, null, null);
    
    public record PaymentRefused() : PaymentValidation(PaymentValidationType.None, null, null);
};

public enum PaymentValidationType {
    None, 
    // ReSharper disable once InconsistentNaming
    With3DS1,
    // ReSharper disable once InconsistentNaming
    With3DS2    
}