using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ToDosProject.Domain.Entities;
using ToDosProject.Web.Services;

namespace ToDosProject.Tests.Helpers
{
    public class MockAcessor
    {

        public static Mock<IHttpContextAccessor> CreateAccessorAuthenticated(User user)
        {
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();

            List<Claim> claims = [new(ClaimTypes.Name, user.Email!)];

            var id = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));

            httpContext.User = new ClaimsPrincipal(id);

            httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(httpContext);
            return httpContextAccessorMock;
        }

        public static Mock<IHttpContextAccessor> CreateAcessorUnauthenticated()
        {
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();

            // Configurando o HttpContext dentro do mock
            httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(httpContext);

            return httpContextAccessorMock;
        }
    }
}
