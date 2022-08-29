using System.Collections.Concurrent;
using System.Text;

namespace BookShop.api;

internal record ApiError(string Code, string Message) {
    public static ApiError FromException<TException>(TException ex)
        where TException : Exception
    {
        var code = ApiErrorCode.FromException<TException>(); 

        var message = ex.Message;

        return new ApiError(code, message);
    }
}

public static class ApiErrorCode {

    private static readonly ConcurrentDictionary<string, string> CodeCache = new();
    private static readonly SemaphoreSlim Sync = new (1);
    
    public static string FromException<TException>()
        where TException : Exception
    {
        var name = typeof(TException).Name;

        var code = GetCodeFromCache(name);

        if (code is not null)
        {
            return code;
        }

        try
        {
            Sync.Wait();
            code = GetCodeFromCache(name);

            if (code is not null)
            {
                return code;
            }
            
            code = ComputeCode(name);

            StoreCodeInCache(name, code);
            
            return code;
        }
        finally
        {
            Sync.Release();
        }

    }
    private static string ComputeCode(string name)
    {
        var code =  name
            .Replace("Exception", "")
            .Select(c => char.IsUpper(c) ? $"_{c}" : $"{c}")
            .Aggregate(new StringBuilder(), (builder, c) => builder.Append(c))
            .ToString();
        
        if (code.StartsWith("_"))
        {
            code = code[1..];
        }

        if (string.IsNullOrWhiteSpace(code))
        {
            code = "unknown";
        }

        return code.ToUpperInvariant();
    }

    private static void StoreCodeInCache(string name, string code)
    {
        CodeCache.TryAdd(name, code);
    }
    
    private static string? GetCodeFromCache(string name)
    {
        return CodeCache.TryGetValue(name, out var code) ? code : null;
    }
} 