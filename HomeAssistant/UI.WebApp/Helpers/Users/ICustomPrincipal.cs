using System.Security.Principal;

namespace UI.WebApp.Helpers.Users
{
    public interface ICustomPrincipal : IPrincipal
    {
        int Id { get; set; }
        string Login { get; set; }
    }
}
