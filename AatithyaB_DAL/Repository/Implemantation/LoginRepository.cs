using AatithyaB_Core.Common;
using System.Net;
using AatithyaB_DAL.Entities;
using AatithyaB_DAL.Repository.Interface;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AatithyaB_Core.Models.User;

namespace AatithyaB_DAL.Repository.Implemantation
{
    public class LoginRepository : ILoginRepository
    {
        private readonly AatithyaDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<LoginRepository> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginRepository(AatithyaDbContext context, IMapper mapper, ILogger<LoginRepository> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        // Login Authentication

        public async Task<(ResponseModel, ListUser)> Login(string username, string password)
        {
            // Create a new user model
            ListUser model = new ListUser();

            // Create a new response model
            ResponseModel response = new ResponseModel();
            try
            {
                // Retrive the user from the database based on the provided username and password
                var result = await _context.Users
                    .Where(u => EF.Functions.Collate(u.Username, "Latin1_General_BIN") == username &&
                                EF.Functions.Collate(u.Password, "Latin1_General_BIN") == password)
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    response.IsSuccess = false;
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = "UserName or Password is Incorrect";
                    return (response, model);
                }

                if (result != null)
                {
                    // Map the user entity to the user model
                    model = _mapper.Map<ListUser>(result);
                    // Prepare a successful response
                    response.IsSuccess = true;
                    response.Status = HttpStatusCode.OK;
                    response.Message = string.Format(MessageNotification.GetMessage((int)StatusId.DataFound), "User");
                }
                else
                {
                    // Prepare a response indicating that the user with the provided credentials was not found
                    response.IsSuccess = false;
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = "No screen rights found! Contact to Adminstratioor.";
                }
            }
            catch (Exception ex)
            {
                // Log the error message to the database using Serilog
                _logger.LogError("An error occurred: {ErrorMessage}", ex.Message);
                // Prepare an error response
                response.IsSuccess = false;
                response.Status = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
            }
            return (response, model);



        }
        public async Task<int?> GetPermissionVersionByUserIdAsync()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new Exception("Invalid or missing UserId in the token.");
            }

            var permissionVersion = await _context.Users
                                                  .Where(u => u.Id == userId)
                                                  .Select(u => u.PermissionVersion)
                                                  .FirstOrDefaultAsync();
            if (permissionVersion != null)
            {
                return permissionVersion;
            }
            else
            {
                return null;
            }
        }
    }


}
