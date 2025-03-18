using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AatithyaB_BLL.Services.Interface;

namespace AatithyaB.Middleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<TokenValidationMiddleware> _logger;

        private readonly IConfiguration _configuration;

        public TokenValidationMiddleware(RequestDelegate next, ILogger<TokenValidationMiddleware> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            var excludedPaths = new List<string>
            {
                "/api/Login/LoginAuthentication",
                "/api/Images/GetAllImages",
                "/api/Images/GetImageById"
            };
            // Skip token validation for specified paths
            if (excludedPaths.Any(path => context.Request.Path.StartsWithSegments(path, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
                return;
            }

            var authTokens = context.Request.Headers["Authorization"];
            var token = authTokens.FirstOrDefault()?.Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Token is missing.");
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            // Validate the token and extract claims
            var permissionVersionFromToken = GetPermissionVersionFromToken(token);
            if (!permissionVersionFromToken.HasValue)
            {
                _logger.LogWarning("Token validation failed or permission version claim missing.");
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            int permissionVersionClaim = permissionVersionFromToken.Value;

            using (var scope = serviceProvider.CreateScope())
            {
                var loginService = scope.ServiceProvider.GetRequiredService<ILoginService>();

                try
                {
                    int? permissionVersion = await loginService.GetPermissionVersionByUserIdAsync();

                    if (!permissionVersion.HasValue)
                    {
                        _logger.LogWarning("Permission version not found for user.");
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        await context.Response.WriteAsync("Invalid Employee or Role!!");
                        await context.Response.CompleteAsync();
                        return;
                    }

                    if (permissionVersion != permissionVersionClaim)
                    {
                        _logger.LogWarning("Permission version mismatch.");
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        await context.Response.WriteAsync("Unauthorized");
                        return;
                    }

                    // Store PermissionVersion in context for further use if validation succeeds
                    context.Items["PermissionVersion"] = permissionVersionClaim;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error occurred in permission version validation: {Message}", ex.Message);
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    await context.Response.WriteAsync("Internal Server Error");
                    return;
                }
            }

            await _next(context);

        }

        private int? GetPermissionVersionFromToken(string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                {
                    _logger.LogError("Received an empty or null JWT token.");
                    return null;
                }

                var tokenHandler = new JwtSecurityTokenHandler();

                if (!tokenHandler.CanReadToken(token))
                {
                    _logger.LogError("JWT token format is invalid.");
                    return null;
                }

                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "");

                if (key.Length < 32)  // Ensure a strong secret key
                {
                    _logger.LogError("JWT secret key is too short or missing.");
                    return null;
                }

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = !string.IsNullOrEmpty(_configuration["Jwt:Issuer"]),
                    ValidateAudience = !string.IsNullOrEmpty(_configuration["Jwt:Audience"]),
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                var permissionVersionClaim = principal.Claims.FirstOrDefault(claim => claim.Type == "PermissionVersion");

                if (int.TryParse(permissionVersionClaim?.Value, out int permissionVersionInt))
                {
                    return permissionVersionInt;
                }

                _logger.LogWarning("Permission version claim not found in token.");
                return null;
            }
            catch (SecurityTokenMalformedException ex)
            {
                _logger.LogError("Malformed token: {Message}", ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("Token validation failed: {Message}", ex.Message);
                return null;
            }
        }

    }
}
