using AutoMapper;
using CustomerInvoice_WebApp.Dtos;
using CustomerInvoice_WebApp.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace CustomerInvoice_WebApp.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                LogException(context, ex);
                await HandleExceptionAsync(context, ex);
            }
        }

        private void LogException(HttpContext context, Exception exception)
        {
            var requestPath = context.Request.Path;
            var logMessage = $"Error occurred processing request: {requestPath} - Exception: {exception}";
            _logger.LogError(logMessage);
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var massage = exception.Message;

            if (exception is AutoMapperMappingException && exception.InnerException is InvalidInputException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                massage = exception.InnerException.Message;
            }
            else
            {
                context.Response.StatusCode = exception switch
                {
                    EntityNotFoundException => StatusCodes.Status404NotFound,
                    InvalidEntityException => StatusCodes.Status400BadRequest,
                    InvalidInputException => StatusCodes.Status400BadRequest,
                    _ => (int)HttpStatusCode.InternalServerError,
                };
            }
            

            return context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = massage
            }.ToString());
        }
    }
}
