using MessageExpert.Core.Authentication.Models;

namespace MessageExpert.Core.Authentication.Services
{

    /// <summary>
    /// <see cref="AuthenticationService"/>
    /// </summary>
    public interface IAuthenticationService
    {
        string CreateIdentity(IClientContext clientContext);
        bool IsAuthenticated { get; }
        T GetContext<T>() where T : IClientContext;
    }
}
