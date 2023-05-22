
using Service.Exceptions;

namespace Api.Common
{
    public class ErrorResponseModel
    {
        public List<ModelError> Errors { get; set; }
        public string Message { get; set; }
    }
}
