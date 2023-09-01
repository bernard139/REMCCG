using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net.Http;
using System.Net;
using REMCCG.Application.Common.Constants.ErrorBuilds;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace REMCCG.Application.Common.Exceptions
{

    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;


        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext, IMessageProvider messageProvider)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex, messageProvider);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception, IMessageProvider messageProvider)
        {
            var getLanguage = Convert.ToString(context.Request.Headers[ResponseCodes.LANGUAGE]);
            context.Response.ContentType = "Application/json";
            var response = context.Response;

            var message = string.Empty;

            var errorResponse = new ErrorResponse
            {
                ResponseDescription = messageProvider.GetMessage(ResponseCodes.EXCEPTION, getLanguage),
                ResponseCode = ResponseCodes.EXCEPTION

            };
            CaseSwirching(exception, messageProvider, getLanguage, response, errorResponse);
            var result = JsonConvert.SerializeObject(errorResponse);
            await context.Response.WriteAsync(result);
        }

        private void CaseSwirching(Exception exception, IMessageProvider messageProvider, string getLanguage, HttpResponse response, ErrorResponse errorResponse)
        {
            switch (exception)
            {
                case ApplicationException ex:
                    if (ex.Message.Contains("Invalid token"))
                    {
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        errorResponse.ResponseDescription = messageProvider.GetMessage(ResponseCodes.INVALID_TOKEN, getLanguage);
                        errorResponse.ResponseCode = ResponseCodes.INVALID_TOKEN;
                        _logger.LogError(ex, "Invalid token");
                        break;
                    }
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.ResponseDescription = messageProvider.GetMessage(ResponseCodes.BAD_REQUEST, getLanguage);
                    errorResponse.ResponseCode = ResponseCodes.BAD_REQUEST;
                    _logger.LogError(ex, "Bad request");
                    break;
                case KeyNotFoundException ex:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse.ResponseDescription = messageProvider.GetMessage(ResponseCodes.NOT_FOUND, getLanguage);
                    errorResponse.ResponseCode = ResponseCodes.NOT_FOUND;
                    _logger.LogError(ex, "Not found");
                    break;


                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.ResponseDescription = messageProvider.GetMessage(ResponseCodes.EXCEPTION, getLanguage);
                    errorResponse.ResponseCode = ResponseCodes.EXCEPTION;
                    _logger.LogError("An error occurred");
                    break;
            }
            _logger.LogError(exception.Message);
        }
    }
}
