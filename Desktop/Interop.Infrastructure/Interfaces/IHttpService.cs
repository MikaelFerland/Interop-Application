namespace Interop.Infrastructure.Interfaces
{
    public interface IHttpService
    {
        bool Login(string usernane, string password);
        void Run();
    }
}
