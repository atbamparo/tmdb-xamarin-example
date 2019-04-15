using System.Net.Http;
using System.Threading.Tasks;

namespace TMDbExample.Core.Repository
{
    public interface IRequestHandler
    {
        Task<T> SendAsync<T>(HttpMethod method, string requestUri);
    }
}