
namespace API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null, bool useSeriousMessages = false)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode, useSeriousMessages);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageForStatusCode(int statusCode, bool useSeriousMessages)
        {
            if (useSeriousMessages)
            {
                return statusCode switch
                {
                    400 => "Bad request. Please check your input.",
                    401 => "Unauthorized access. Please authenticate.",
                    403 => "Forbidden. You do not have permission to access this resource.",
                    404 => "Resource not found.",
                    500 => "Internal server error. Please try again later.",
                    _ => "An unexpected error occurred."
                };
            }
            else
            {
                return statusCode switch
                {
                    400 => "A bad request, you have made",
                    401 => "Unauthorized, are you not",
                    403 => "Forbidden, you cannot access",
                    404 => "Resource found, it was not",
                    500 => "Error are the path to the dark side.  Error lead to anger. Anger leads to hate. Hate leads to career change",
                    _ => "An unexpected error occurred."
                };
            }
        }
    }
}