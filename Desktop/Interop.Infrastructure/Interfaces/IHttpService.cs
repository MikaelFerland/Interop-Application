using System.Threading;
using System.Threading.Tasks;

namespace Interop.Infrastructure.Interfaces
{
    public interface IHttpService
    {
        Task<bool> Login(string usernane, string password, string ipAddress, string port);
        Task Run(CancellationToken cancellationToken);
    }
}
