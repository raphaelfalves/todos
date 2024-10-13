using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace ToDosProject.Shared.Handler
{
    public class CookieHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
                request.Headers.Add("X-Requested-With", ["XMLHtttpRequest"]);

                return await base.SendAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
