using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TweetBook2.Installers
{
    public interface IInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration);
    }
}
