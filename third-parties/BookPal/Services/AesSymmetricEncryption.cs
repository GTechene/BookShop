using System.Security.Cryptography;
using System.Text;

namespace BookPal.Services;

public static class AesSymmetricEncryption {
    private async static Task EncryptAsync(Stream cleartext, Stream ciphertext, byte[] key) {
        using var algo = Aes.Create();
        algo.GenerateIV();

        await ciphertext.WriteAsync(algo.IV.AsMemory(0, algo.IV.Length)); 

        var encryptor = algo.CreateEncryptor(key, algo.IV);
        await using var crypto = new CryptoStream(ciphertext, encryptor, CryptoStreamMode.Write);
        await cleartext.CopyToAsync(crypto);

        algo.Clear();
    }

    private async static Task<byte[]> EncryptAsync(byte[] input, byte[] key) {
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

        using var algo = Aes.Create();
        var iv = new byte[algo.BlockSize / 8];
        
        _ = await ciphertext.ReadAsync(iv);

        var decryptor = algo.CreateDecryptor(key, iv);
        await using var crypto = new CryptoStream(ciphertext, decryptor, CryptoStreamMode.Read);
        await crypto.CopyToAsync(cleartext);

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