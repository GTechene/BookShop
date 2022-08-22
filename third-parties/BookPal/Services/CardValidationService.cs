using BookPal.Model;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace BookPal.Services; 

public class CardValidationService {
    private readonly HashGenerator _hasher;
    private readonly IServer _server;

    private const string ExpectedUser = "user";
    private const string ExpectedPassword = "password";
    
    public CardValidationService(HashGenerator hasher, IServer server)
    {
        _hasher = hasher;
        _server = server;
    }
    
    public CardValidation GetValidationFor(Card card)
    {
        return card.Number switch
        {
            Cards.AlwaysYes => CardValidation.NoValidationRequired(_hasher.Hash(card)),
            Cards.With3DS1 => CardValidation.Requires3Ds1($"{CurrentUrl}/api/payment/card/3ds1"),
            Cards.With3DS2 => CardValidation.Requires3Ds2,
            _ => throw new InvalidCard()
        };
    }

    private string CurrentUrl => 
        _server.Features.Get<IServerAddressesFeature>()!.Addresses.First(url => url.StartsWith("https"));

    public CardValidation Get3DSValidationFor(Card card, (string User, string Password) credentials)
    {
        if(credentials != (ExpectedUser, ExpectedPassword))
        {
            throw new ValidationUsing3DS2Rejected();
        }
        
        return CardValidation.NoValidationRequired(
            _hasher.Hash(card));
    }

}