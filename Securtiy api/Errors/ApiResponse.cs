namespace Securtiy_api.Errors
{
    public class ApiResponse
    {
        //steopone
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public ApiResponse(int statusCode,string? message =null)
        {
            statusCode = StatusCode;
            message = Message??GetDefaultMessageForStatusCode(statusCode);
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                200=> "This email is already in user",
                400 => "BadRequest , you have made ",
                401 => "UnAuthorized, sorry you are Not",
                404 => "Resource Was Not Found",
                500 => "Errors are the path to the dark side. errors lead to anger . anger lead to hate. Hate leads to career change  ",
                _ => null
            };
        }
    }
}
