// ReSharper disable once CheckNamespace
namespace System;

public static class DecimalExtensions
{
    private static readonly decimal[] Mults = {
        1M, 
        1e1M, 1e2M, 1e3M, 1e4M, 1e5M, 1e6M, 1e7M, 1e8M, 1e9M, 1e10M,
        1e11M, 1e12M, 1e13M, 1e14M, 1e15M, 1e16M, 1e17M, 1e18M, 1e19M, 
        1e20M, 1e21M, 1e22M, 1e23M, 1e24M, 1e25M, 1e26M, 1e27M, 1e28M
    };

    public static (int IntegerPart, decimal FractionalPart) SplitParts(this decimal amount)
    {
        var integerPart = (int)amount;
        
        var bits = decimal.GetBits(amount);
        var scale = (bits[3] >> 16) & 31;

        var fractionalPart = (amount - integerPart) * Mults[scale];

        return (integerPart, fractionalPart);
    }
}