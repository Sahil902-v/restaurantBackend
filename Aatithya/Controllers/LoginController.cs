
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Aatithya_BLL.Services.Interface;
using Aatithya_Core.Common;
using Aatithya_Core.Models.User;
using Aatithya_BLL.Services.Interface;
using Aatithya_Core.Common;
using Aatithya_Core.Models.User;
using Aatithya_DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Aatithya.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        #region Fields Declaration
        /// The login service used by the controller.
        private readonly ILoginService _service;
        /// The logger instance for logging login controller-related activities.
        private readonly ILogger<LoginController> _logger;
        ///  The configuration interface for accessing configuration settings.
        private readonly IConfiguration _configuration;
        #endregion

        #region Parameterized Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginController"/> class.
        /// </summary>
        /// <param name="service">The login service to be injected.</param>
        /// <param name="logger">The logger instance to be injected.</param>
        /// <param name="configuration">The configuration instance to be injected.</param>
        public LoginController(ILoginService service, ILogger<LoginController> logger, IConfiguration configuration)
        {
            // Set the login service, logger and configuration instanceand
            _service = service;
            _logger = logger;
            _configuration = configuration;
        }
        #endregion

        #region Login Authontication
        /// <summary>
        /// Retrieves the user login information from the database based on the provided username and password.
        /// </summary>
        /// <remarks>
        /// This method fetches the user login information from the database for authentication purposes.
        /// </remarks>
        /// <param name="Username">The username of the user attempting to log in.</param>
        /// <param name="Password">The password of the user attempting to log in.</param>
        /// Date: 17-01-2025
        /// Developer: Krupa Lad

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
                    username = data.UserName,
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
        #endregion

        #region JWT Authentication
        /// <summary>
        /// Generates a JWT token for the given user model.
        /// </summary>
        /// <param name="user">The user model for which the token is generated.</param>
        /// <returns>The generated JWT token as a string.</returns>

        private string GenerateToken(ListUserModel user)
        {
            // Create a security key using the JWT key from configuration
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            // Create signing credentials using the security key and HmaSha256 algorithm
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Create claims, including the role
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),



                    new Claim("UserId", user.Id.ToString())
                };
            // Create a JWT token with issuer, audience, expiration time, and signing credentials
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims, // Claims added by Krupa 21-02-2025
                        //expires: DateTime.Now.AddMinutes(15), // Expiration time (15 minutes from now)
                expires: DateTime.Now.AddDays(1), // Expiration time (15 minutes from now)
                signingCredentials: credentials // Signing xredentials
                );

            // Write the token as a string using JwtSecurityTokenHandler
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
    }
}
