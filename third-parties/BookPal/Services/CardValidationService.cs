using BookPal.Model;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace BookPal.Services; 

public class PaymentValidationService {
    private readonly HashGenerator _hasher;
    private readonly IServer _server;

    private const string ExpectedUser = "user";
    private const string ExpectedPassword = "password";
    
    public PaymentValidationService(HashGenerator hasher, IServer server)
    {
        _hasher = hasher;
        _server = server;
    }
    
    public PaymentValidation Validate(Card card, Price price)
    {
        return card.Number switch
        {
            Cards.AlwaysYes => PaymentValidation.Validated(new Payment(price, card, _hasher.Hash(card))),
            Cards.With3DS1 => PaymentValidation.Requires3Ds1($"{CurrentUrl}/api/payment/3ds1"),
            Cards.With3DS2 => PaymentValidation.Requires3Ds2,
            _ => throw new InvalidCard()
        };
    }
    
    public bool Validate(Payment payment)
    {
        var epectedHash = _hasher.Hash(payment.Card);
        return epectedHash == payment.Hash;
    }

    private string CurrentUrl => 
        _server.Features.Get<IServerAddressesFeature>()!.Addresses.First(url => url.StartsWith("https"));

    public PaymentValidation Validate3DSecure(Card card, Price price, (string User, string Password) credentials)
    {
        if(credentials != (ExpectedUser, ExpectedPassword))
        {
            throw new ValidationUsing3DS2Rejected();
        }
        
        return PaymentValidation.Validated(new Payment(price, card, _hasher.Hash(card)));
    }

    
    
}