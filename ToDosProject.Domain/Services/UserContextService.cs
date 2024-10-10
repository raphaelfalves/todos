using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ToDosProject.Domain.Services
{
    public class UserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Id do usuário não encontrado.");
        }
    }
}
