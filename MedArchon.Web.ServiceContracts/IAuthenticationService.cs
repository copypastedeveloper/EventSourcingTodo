namespace MedArchon.Web.ServiceContracts
{
    public interface IAuthenticationService
    {
        bool AuthenticateUser(AuthenticationToken token);
    }
}
