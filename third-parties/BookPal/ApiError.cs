using System.Text;

namespace BookPal;

internal record ApiError(string Code, string Message) {
    public static ApiError FromException<TException>(TException ex)
        where TException : Exception
    {
        var code = ex.GetType().Name
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
            
        var message = ex.Message;

        return new ApiError(code.ToUpperInvariant(), message);
    }
}