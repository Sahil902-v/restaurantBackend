using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Aatithya_Core.Common
{
    public class GetLoggedInUserId
    {
        #region Fields Declaration
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Parameterized Constructor
        public GetLoggedInUserId(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Get Current User Id
        public int? GetUserIdFromToken()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == "UserId")?.Value;
            Console.WriteLine(userIdClaim);
            return int.TryParse(userIdClaim, out var userId) ? userId : null;
        }
        #endregion
    }
}
