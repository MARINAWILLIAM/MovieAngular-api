namespace Securtiy_api.Errors
{
    public class ApiValdtionErrorResponse:ApiResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public ApiValdtionErrorResponse():base(400)
        {
            Errors=new List<string>();


        }

    }
}
