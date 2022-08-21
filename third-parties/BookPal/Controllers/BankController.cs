using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BookPal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BankController : ControllerBase {
    private readonly HashOptions _hashOptions;

    private SHA256 _hasher = SHA256.Create();

    public BankController(IOptions<HashOptions> hashOptions)
    {
        _hashOptions = hashOptions.Value;
    }
   
    
    [HttpPost]
    [Route("card")]
    public CardAction QueryCardAction(Card card)
    {
        return card.Number switch
        {
            Cards.Yes => CardAction.NoValidationRequired(GenerateHash(card)),
            Cards.With3DS1 => CardAction.Requires3Ds1(""),
            Cards.With3DS2 => CardAction.Requires3Ds2,
            _ => throw new InvalidCard()
        };
    }

    [HttpPost]
    [Route("card/3ds2")]
    public string Validate3DS2(ValidationRequest request)
    {
        if (request.User != "User" || request.Password != "Password")
        {
            throw new Invalid3DS2Credentials();
        }

        return GenerateHash(request.Card);
    }
    
    private string GenerateHash(Card card)
    {
        var hashableString = $"{_hashOptions.Salt}|{card.Number}|{card.OwnerName}|{card.ExpirationDate:MM/yyyy}|{card.SecurityCode}";

        var bytes = _hasher.ComputeHash(Encoding.UTF8.GetBytes(hashableString));

        return Encoding.UTF8.GetString(bytes);
    }
}

public class Invalid3DS2Credentials : Exception {
}

public record ValidationRequest(Card Card, string User, string Password);

public class InvalidCard : Exception {
}

public record Card(
    string Number,
    string OwnerName,
    DateTime ExpirationDate,
    string SecurityCode);

public record CardAction(CardActionType Type) {
    public static CardAction NoValidationRequired(string paymentHash) => new(CardActionType.NoValidationRequired)
    {
        PaymentHash = paymentHash
    };
    public static CardAction Requires3Ds1(string url) => new(CardActionType.ValidateWith3DS1)
    {
        WebSiteUrl = url
    };
    
    public static CardAction Requires3Ds2 => new(CardActionType.ValidateWith3DS2);
    
    public string? WebSiteUrl { get; init; }
    public string? PaymentHash { get; init; }
}

public enum CardActionType {
    NoValidationRequired, 
    // ReSharper disable once InconsistentNaming
    ValidateWith3DS1,
    // ReSharper disable once InconsistentNaming
    ValidateWith3DS2     
}
