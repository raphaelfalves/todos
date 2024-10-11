using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using ToDosProject.Domain.Entities;
using ToDosProject.Domain.Interfaces;

namespace ToDosProject.Web.Services
{
    public class CookieAuthenticationStateProvider(ApiServiceClient apiServiceClient) : AuthenticationStateProvider, ICookieAuthenticationStateProvider
    {
        private bool _isAuthenticated = false;
        public async Task<bool> CheckAuthenticatedAsync()
        {
            await GetAuthenticationStateAsync();
            return _isAuthenticated;
        }

        public void NotifyAuthenticationStateChanged() =>
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            _isAuthenticated = false;

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var userInfo = await apiServiceClient.Info();

            if (userInfo == null)
                return new AuthenticationState(user);

            var claims = GetClaims(userInfo);

            var id = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));

            user = new ClaimsPrincipal(id);

            _isAuthenticated = true;
            return new AuthenticationState(user);
        }

        private static List<Claim> GetClaims(User user)
        {
            List<Claim> claims =
            [
                new(ClaimTypes.Name, user.Email!),
                new(ClaimTypes.Email, user.Email!),
            ];

            return claims;
        }

    }
}
