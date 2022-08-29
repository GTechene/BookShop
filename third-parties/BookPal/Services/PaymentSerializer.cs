using System.Text.Json;
using BookPal.Model;
using Microsoft.Extensions.Options;

namespace BookPal.Services;

public class PaymentSerializer {
    private readonly IOptions<CypherOptions> _cypherOptions;

    public PaymentSerializer(IOptions<CypherOptions> cypherOptions)
    {
        _cypherOptions = cypherOptions;
    }
    
    public async Task<string> Serialize(Payment payment)
    {
        var paymentJson = JsonSerializer.Serialize(payment);

        return await Encrypt(paymentJson);
    }
    
    private async Task<string> Encrypt(string plainText)
    {
        var key = Convert.FromBase64String(_cypherOptions.Value.Key);

        return await AesSymmetricEncryption.EncryptAsync(plainText, key);
    }
    
    public async Task<Payment> Deserialize(string cipherText)
    {
        var json = await DecryptString(cipherText);

        var payment = JsonSerializer.Deserialize<Payment>(json);

        if (payment is null)
        {
            throw new InvalidOperationException();
        }
        
        return payment;
    }
    
    private async Task<string> DecryptString(string cipherText)
    {
        var key = Convert.FromBase64String(_cypherOptions.Value.Key);

        return await AesSymmetricEncryption.DecryptAsync(cipherText, key);
    }
}