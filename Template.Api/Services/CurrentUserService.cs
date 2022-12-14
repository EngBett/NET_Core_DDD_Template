using Template.Application.Interfaces;
using System.Security.Claims;
using IdentityModel;
namespace Template.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public string UserId => this.httpContextAccessor?.HttpContext?.User?.FindFirstValue(JwtClaimTypes.Id) ?? "";

        public string Email => this.httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

        public string PhoneNumber => this.httpContextAccessor?.HttpContext?.User?.FindFirstValue(JwtClaimTypes.PhoneNumber);

        public string Customer => $"{this.httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.GivenName)}" +
            $" {this.httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.Surname)}";

    }
}
