namespace CommerceHub.Schema
{
    public class AuthorizationRequest 
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class AuthorizationResponse 
    {
        public string AccessToken { get; set; }
    }
}
