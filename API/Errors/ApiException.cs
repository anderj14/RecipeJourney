
namespace API.Errors
{
    public class ApiException : ApiResponse
    {
        public ApiException(int statusCode, string message = null, bool useSeriousMessages = false, string details = null) : base(statusCode, message, useSeriousMessages)
        {
            Details = details;
        }

        public string Details { get; set; }
    }
}