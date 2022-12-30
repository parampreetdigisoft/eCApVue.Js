namespace ECap.Web.Models
{
    public class LoginResponse : JsonResponse
    {
        public string CallBackUrl { get; set; }
        public string Languge { get; set; }
       
    }
}
