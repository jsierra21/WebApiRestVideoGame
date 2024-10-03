namespace Core.ModelResponse
{

    public class ResponseAuth
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
        public string Correo { get; set; }
        public string ExpiroClave { get; set; }
        public string Token { get; set; }
    }
}
