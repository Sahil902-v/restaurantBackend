using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AatithyaB_Core.Common
{
    public class GetLoggedInUserId
    {
        #region Fields Declaration
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Parameterized Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DepartmentRepository"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        /// <remarks>
        /// Date: 11-04-2024
        /// Developer: Jayshree Patel
        /// </remarks>
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

            return int.TryParse(userIdClaim, out var userId) ? userId : null;
        }
        #endregion
    }
}
