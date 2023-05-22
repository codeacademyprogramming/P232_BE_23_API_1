using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service.Exceptions
{
    public class RestException:Exception
    {
        public RestException(HttpStatusCode code,string key,string modelMessage,string? message=null)
        {
            this.Code = code;
            this.Errors = new List<ModelError> { new ModelError(key, modelMessage) };
            this.Message = message;
        }

        public RestException(HttpStatusCode code,string? message)
        {
            this.Code = code;
            this.Message = message;
        }


        public HttpStatusCode Code { get; set; }
        public List<ModelError> Errors { get; set; }
        public string Message { get; set; }
    }

    public class ModelError
    {
        public ModelError(string key,string message)
        {
            Key = key;
            Message = message;
        }
        public string Key { get; set; }
        public string Message { get; set; }
    }
}
