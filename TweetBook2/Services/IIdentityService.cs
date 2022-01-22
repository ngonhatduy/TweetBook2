using System.Threading.Tasks;
using TweetBook2.Domain;

namespace TweetBook2.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password);
        Task<AuthenticationResult> Login(string email, string password);

    }
}
