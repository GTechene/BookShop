using System.Net;
using System.Net.Mime;
using BookShop.shared;
using Microsoft.AspNetCore.Diagnostics;

namespace BookShop.api.Errors;

public class ExceptionHandler {

    public class HttpResponseWriter {
        private readonly HttpContext _httpContext;

        public HttpResponseWriter(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }
        
        public async Task Write(HttpStatusCode httpStatusCode, ApiError error)
        {
            _httpContext.Response.ContentType = MediaTypeNames.Application.Json;
            _httpContext.Response.StatusCode = (int)httpStatusCode;
            await _httpContext.Response.WriteAsJsonAsync(error);   
        }
    }

    private delegate Task OnException(Exception exception, HttpResponseWriter writer, ILogger logger);
    private record Handler(Predicate<Exception> Condition, OnException Handle);

    private readonly List<Handler> _handlers = new();
    
    public static Action<IApplicationBuilder> Run(Func<ExceptionHandler, ExceptionHandler> configure) =>
        applicationBuilder =>
            applicationBuilder.Run(async context =>
            {
                var handler = configure(new ExceptionHandler());
                await handler.Apply(context);
            });

    private ExceptionHandler() { }

    public ExceptionHandler On<TException>(HttpStatusCode statusCode, LogLevel logLevel)
        where TException : Exception
    {
        return On<TException>((exception, writer, logger) => DefaultHandler(exception, writer, logger, statusCode, logLevel)
        );
    }
    
    public ExceptionHandler On<TException>(Func<TException, HttpResponseWriter, ILogger, Task> handle)
        where TException: Exception
    {
        var handler = new Handler(ex => ex is TException,
            (ex, writer, logger) => handle((TException)ex, writer, logger)
        );
        _handlers.Add(handler);
        return this;
    }
    
    
    public async Task Apply(HttpContext httpContext)
    {   
        var exceptionHandlerPathFeature = httpContext.Features.Get<IExceptionHandlerPathFeature>();

        var exception = exceptionHandlerPathFeature?.Error;

        if (exception is not null)
        {
            var writer = new HttpResponseWriter(httpContext);
            var logger = httpContext.RequestServices.GetRequiredService<ILogger<ExceptionHandler>>();

            var handlers = _handlers.Where(h => h.Condition(exception)).ToList();

            if (handlers.Count != 1)
            {
                await DefaultHandler(exception, writer, logger, HttpStatusCode.InternalServerError, LogLevel.Error);
                return;
            }

            await handlers.Single().Handle(exception, writer, logger);
        }
    }
    public async static Task DefaultHandler(Exception exception, HttpResponseWriter writer, ILogger logger, HttpStatusCode httpStatusCode, LogLevel logLevel)
    {
        logger.Log(logLevel, exception, "");
        await writer.Write(httpStatusCode, ApiErrorFactory.FromException(exception));
    }
}