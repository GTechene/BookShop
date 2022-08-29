namespace BookShop.api.Errors;

public static class ApplicationBuilderExtensions {
    public static IApplicationBuilder UseExceptionHandler(this IApplicationBuilder applicationBuilder,
        Func<ExceptionHandler, ExceptionHandler> configure)
    {
        return applicationBuilder.UseExceptionHandler(handler => {
            var runner = ExceptionHandler.Run(configure);
            runner(handler);
        });
    }
}