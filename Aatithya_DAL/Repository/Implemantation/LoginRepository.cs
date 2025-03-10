using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Aatithya_Core.Common;
//using Aatithya_Core.Models.RoleRights;
//using Aatithya_Core.Models.Screen;
using Aatithya_Core.Models.User;
using Aatithya_DAL.Entities;
using Aatithya_DAL.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Build.Framework;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Aatithya_DAL.Repository.Implematation
{
    public class LoginRepository : ILoginRepository
    {
        #region Fields
        private readonly AatithyaDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<LoginRepository> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Parameterized Constructor
        /// <summary>
		/// Initializes a new instance of the <see cref="LoginRepository"/> class.
		/// </summary>
		/// <param name="context">The application database context.</param>
		/// <param name="mapper">The AutoMapper instance for mapping between entity and model.</param>
		/// <param name="logger">The logger instance for logging login-related activities.</param>
		/// <remarks>
		/// Date: 17-01-2025
		/// Developer: Krupa Lad
		/// </remarks>
        public LoginRepository(AatithyaDbContext context, IMapper mapper, ILogger<LoginRepository> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Login Authentication
        /// <summary>
		/// Retrieves the user login information from the database based on the provided username and password.
		/// </summary>
		/// <remarks>
		/// This method fetches the user login information from the database for authentication purposes.
		/// </remarks>
		/// <param name="username">The username of the user attempting to log in.</param>
		/// <param name="password">The password of the user attempting to log in.</param>
        public async Task<(ResponseModel, ListUserModel)> Login(string username, string password)
        {
            // Create a new user model
            ListUserModel model = new ListUserModel();

            ResponseModel response = new ResponseModel();
            try
            {
                // Retrive the user from the database based on the provided username and password
                var result = await _context.Users
                    .Where(u => EF.Functions.Collate(u.UserName, "Latin1_General_BIN") == username &&
                                EF.Functions.Collate(u.Password, "Latin1_General_BIN") == password)
                    .FirstOrDefaultAsync();
                if (result == null)
                {
                    response.IsSuccess = false;
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = "UserName or Password is Incorrect";
                    return (response, model);
                }

                else
                {
                    return (response, model);
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                response.IsSuccess = false;
                response.Status = HttpStatusCode.NotFound;
                return (response, model);
            }
        }





        //roleRightsWithScreens = await _context.RolesRights
        //    .Where(rr => rr.RoleId == userRole)
        //    .Include(s => s.Screen) // Include the screen details
        //    .Where(s => s. == false) // Assuming `IsDeleted` is a boolean
        //    .ToListAsync();

        //roleRightsWithScreens = await _context.RolesRights
        //    .Include(s => s.Screen)
        //    .Where(rr => rr.RoleId == userRole && rr.Screen.IsDeleted == false)
        //    .ToListAsync();

        //if (roleRightsWithScreens.Any())
        //{
        //    prmissionModel = roleRightsWithScreens
        //        .Select(rr => new ListRoleRightsModel
        //        {
        //            Id = rr.Id,
        //            RoleId = rr.RoleId,
        //            ScreenId = rr.ScreenId,
        //            Insert = rr.Insert,
        //            Update = rr.Update,
        //            Delete = rr.Delete,
        //            View = rr.View,
        //            // Map screen details
        //            ScreenDetails = new ListScreensModel
        //            {
        //                Id = rr.Screen.Id,
        //                ParentId = rr.Screen.ParentId,
        //                RouteLink = rr.Screen.RouteLink,
        //                Icon = rr.Screen.Icon,
        //                Lable = rr.Screen.Lable,
        //                IsDeleted = rr.Screen.IsDeleted
        //            },
        //            RowIndex = 0 // Placeholder value for now
        //        })
        //        .ToList();
        //}

        //for (int i = 0; i < prmissionModel.Count; i++)
        //{
        //    prmissionModel[i].RowIndex = i + 1; // RowIndex starts from 1
        //}

        //    if (result != null)
        //    {
        //        // Map the user entity to the user model
        //        model = _mapper.Map<ListUserModel>(result);
        //        // Prepare a successful response
        //        response.IsSuccess = true;
        //        response.Status = HttpStatusCode.OK;
        //        response.Message = string.Format(MessageNotification.GetMessage((int)StatusId.DataFound), "User");
        //    }
        //    else
        //    {
        //        // Prepare a response indicating that the user with the provided credentials was not found
        //        response.IsSuccess = false;
        //        response.Status = HttpStatusCode.NotFound;
        //        response.Message = "No screen rights found! Contact to Adminstratioor.";
        //    }
        //}
        //catch (Exception ex)
        //{
        //    // Log the error message to the database using Serilog
        //    _logger.LogError("An error occurred: {ErrorMessage}", ex.Message);
        //    // Prepare an error response
        //    response.IsSuccess = false;
        //    response.Status = HttpStatusCode.InternalServerError;
        //    response.Message = ex.Message;
        //}
        // Return a tuple containing the response model and the user model
        //    return (response, model, prmissionmodel);
        //}

        private string EncryptPasswordWithSHA256(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
        #endregion