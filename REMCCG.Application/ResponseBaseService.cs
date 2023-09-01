using REMCCG.Application.Common.Models;
using REMCCG.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application
{
    public class ResponseBaseService
    {
        private readonly IMessageProvider _messageProvider;
        public ResponseBaseService(IMessageProvider messageProvider)
        {
            _messageProvider = messageProvider;
        }
        public ServerResponse<T> SetErrorValidation<T>(ServerResponse<T> response, string responseCode, string message)
        {
            response.Error = new ErrorResponse
            {
                ResponseCode = responseCode,
                ResponseDescription = message

            };
            return response;
        }
        public ServerResponse<T> SetError<T>(ServerResponse<T> response, string responseCode, string language)
        {
            response.Error = new ErrorResponse
            {
                ResponseCode = responseCode,
                ResponseDescription = _messageProvider.GetMessage(responseCode, language)

            };
            return response;
        }
        public ServerResponse<T> SetError<T>(ServerResponse<T> response, string responseCode, string message, string language)
        {
            response.Error = new ErrorResponse
            {
                ResponseCode = responseCode,
                ResponseDescription = message

            };
            return response;
        }
        public ServerResponse<T> SetSuccess<T>(ServerResponse<T> response, T data, string responseCode, string language)
        {
            response.SuccessMessage = _messageProvider.GetMessage(responseCode, language);
            response.IsSuccessful = true;
            response.Data = data;
            return response;
        }



        public ServerResponse<T> SetErrorWithStatus<T>(ServerResponse<T> response, string responseCode, string status, string language)
        {
            response.Error = new ErrorResponse
            {
                ResponseCode = responseCode,
                ResponseDescription = _messageProvider.GetMessage(responseCode, language)?.Replace("{status}", status)

            };
            return response;
        }

    }
}
