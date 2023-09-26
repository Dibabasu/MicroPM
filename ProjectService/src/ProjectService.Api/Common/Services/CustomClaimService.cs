using ProjectService.Api.Common.Interfaces;

namespace ProjectService.Api.Common.Services
{
    public class CustomClaimService : ICustomClaimService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomClaimService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUser()
        {
            var result = string.Empty;

            if (_httpContextAccessor != null)
            {
                result = _httpContextAccessor!.HttpContext!.User.FindFirst("cognito:username")?.Value;
            }
            return result;
        }
    }
}