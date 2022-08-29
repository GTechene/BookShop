using BookShop.shared;

namespace BookShop.api.Errors; 

public static class ApiErrorFactory {
    public static ApiError FromException<TException>(TException ex)
        where TException : Exception
    {
        var code = ApiErrorCode.FromException<TException>(); 

        var message = ex.Message;

        return new ApiError(code, message);
    }
}
