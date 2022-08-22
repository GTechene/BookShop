using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;

namespace BookPal.Services; 

public class HashGenerator {
    private readonly HashOptions _hashOptions;

    private SHA256 _hasher = SHA256.Create();

    public HashGenerator(IOptions<HashOptions> hashOptions)
    {
        _hashOptions = hashOptions.Value;
    }

    public string Hash(object obj)
    {
        var hashableString = $"{_hashOptions.Salt}|{obj}";

        var bytes = _hasher.ComputeHash(Encoding.UTF8.GetBytes(hashableString));

        return Encoding.UTF8.GetString(bytes);
    }
}