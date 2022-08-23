using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using BookPal.Model;
using Microsoft.Extensions.Options;

namespace BookPal.Services; 

public static class AesSymmetricEncryption {
    
    public static byte[] GenerateKey()
    {
        using var algo = Aes.Create();
        algo.GenerateKey();
        return algo.Key;
    }

    private async static Task EncryptAsync(Stream cleartext, Stream ciphertext, byte[] key) {
        if (null == cleartext)
            throw new ArgumentNullException(nameof(cleartext));

        if (null == ciphertext)
            throw new ArgumentNullException(nameof(ciphertext));

        if (null == key)
            throw new ArgumentNullException(nameof(key));

        using var algo = Aes.Create();
        // generate a new IV for each encryption operation
        algo.GenerateIV();

        // write IV to the beginning of the stream in cleartext bytes
        await ciphertext.WriteAsync(algo.IV, 0, algo.IV.Length); 

        var encryptor = algo.CreateEncryptor(key, algo.IV);
        using (var crypto = new CryptoStream(ciphertext, encryptor, CryptoStreamMode.Write)) {
            await cleartext.CopyToAsync(crypto);
        }

        algo.Clear();
    }

    private async static Task<byte[]> EncryptAsync(byte[] input, byte[] key) {
        if (null == input)
            throw new ArgumentNullException(nameof(input));

        using var cleartext = new MemoryStream(input);
        using var ciphertext = new MemoryStream();
        await EncryptAsync(cleartext, ciphertext, key);
        return ciphertext.ToArray();
    }

    public async static Task<string> EncryptAsync(string cleartext, byte[] key, Encoding? encoding = null) {
        if (string.IsNullOrEmpty(cleartext))
            throw new ArgumentNullException(nameof(cleartext));

        encoding ??= Encoding.UTF8;

        var encryptedBytes = await EncryptAsync(encoding.GetBytes(cleartext), key);
        return Convert.ToBase64String(encryptedBytes);
    }

    private async static Task DecryptAsync(Stream ciphertext, Stream cleartext, byte[] key) {
        if (null == ciphertext)
            throw new ArgumentNullException(nameof(ciphertext));

        if (null == cleartext)
            throw new ArgumentNullException(nameof(cleartext));

        if (null == key)
            throw new ArgumentNullException(nameof(key));

        using var algo = Aes.Create();
        // read IV from the beginning of the stream
        var iv = new byte[algo.BlockSize / 8];
        
        _ = await ciphertext.ReadAsync(iv, 0, iv.Length);

        var decryptor = algo.CreateDecryptor(key, iv);
        await using (var crypto = new CryptoStream(ciphertext, decryptor, CryptoStreamMode.Read)) {
            await crypto.CopyToAsync(cleartext);
        }

        algo.Clear();
    }

    private async static Task<byte[]> DecryptAsync(byte[] encrypted, byte[] key) {
        if (null == encrypted)
            throw new ArgumentNullException(nameof(encrypted));

        using var ciphertext = new MemoryStream(encrypted);
        using var cleartext = new MemoryStream();
        await DecryptAsync(ciphertext, cleartext, key);
        return cleartext.ToArray();
    }

    public async static Task<string> DecryptAsync(string encrypted, byte[] key, Encoding? encoding = null) {
        if (string.IsNullOrEmpty(encrypted))
            throw new ArgumentNullException(nameof(encrypted));

        var clearBytes = await DecryptAsync(Convert.FromBase64String(encrypted), key);

        encoding ??= Encoding.UTF8;
        return encoding.GetString(clearBytes);
    }
}

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