using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Aatithya_BLL.Services.Interface;
using Aatithya_Core.Common;
using Aatithya_Core.Models.User;
using AutoMapper.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Aatithya.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        //Fields Declaration

        /// The login service used by the controller.
        private readonly ILoginService _service;

        /// The logger instance for logging login controller-related activities.
        private readonly ILogger<LoginController> _logger;

        ///  The configuration interface for accessing configuration settings.
        private readonly IConfiguration _configuration;

        // Parameterized Constructor

        public LoginController(ILoginService service, ILogger<LoginController> logger, IConfiguration configuration)
        {
            // Set the login service, logger and configuration instanceand
            _service = service;
            _logger = logger;
            _configuration = configuration;
        }

        //Login Authontication

        [HttpPost]
        [Route("LoginAuthentication")]
        public async Task<IActionResult> Login(string Username, string Password)
        {
            // Initializa a response model
            ResponseModel responseModel = new ResponseModel();

            try
            {
                // Check if username or password is empty or numm
                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
                {
                    // Log the error message to the database using Serilog
                    _logger.LogError("Error: Username or password not provided.");

                    // Prepare a bad request response 
                    responseModel.IsSuccess = false;
                    responseModel.Status = HttpStatusCode.BadRequest;
                    responseModel.Message = "Username or password not provided.";
                    return BadRequest(responseModel);
                }

                var (response, data) = await _service.Login(Username, Password);

                // Check if login was unsuccessful
                if (data.Id == 0)
                {
                    // Return the appropriate response based on the status code
                    return response.Status switch
                    {
                        HttpStatusCode.BadRequest => BadRequest(response),
                        HttpStatusCode.InternalServerError => StatusCode((int)HttpStatusCode.InternalServerError, response),
                        HttpStatusCode.OK => Ok(response),
                        _ => StatusCode((int)response.Status, response)
                    };
                }

                // Call GenerateToken method and return the token
                string token = GenerateToken(data);

                // Embed the user data and token directly in the response model's data
                response.Data = new
                {
                    id = data.Id,
                    //employeeId = data.EmployeesId,
                    username = data.Username,
                    permissionVersion = data.PermissionVersion,
                    token,
                };
                return Ok(response);

            }
            catch (Exception ex)
            {
                // Log the error message to the database using Serilog
                _logger.LogError("An error occurred: {ErrorMessage}", ex.Message);

                // Prepare an internal server error response
                responseModel.IsSuccess = false;
                responseModel.Status = HttpStatusCode.InternalServerError;
                responseModel.Message = "An error occurred while processing your request.";
                return StatusCode((int)HttpStatusCode.InternalServerError, responseModel);
            }


        }

        private string GenerateToken(ListUser user)
        {
            // Create a security key using the JWT key from configuration
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            // Create signing credentials using the security key and HmaSha256 algorithm
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Create claims, including the role
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new("PermissionVersion", user.PermissionVersion.ToString()), // Token version claim
                    new Claim("UserId", user.Id.ToString())
                };
            // Create a JWT token with issuer, audience, expiration time, and signing credentials
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims, 
                expires: DateTime.Now.AddDays(69),
                signingCredentials: credentials
                );

            // Write the token as a string using JwtSecurityTokenHandler
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
