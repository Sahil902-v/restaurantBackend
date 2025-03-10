using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Aatithya_DAL.Entities;
using AutoMapper;
using Aatithya_Core.Common;
using Aatithya_Core.Models.User;
using Aatithya_DAL.Entities;
using Aatithya_DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Aatithya_Core.Common;

namespace Aatithya_DAL.Repository.Implematation
{
    public class UsersRepository : IUsersRepository
    {
        #region Fields Declaration
        /// The database context for the Asset Tracking Product.
        private readonly AatithyaDbContext _context;
        /// The AutoMapper instance for mapping between entity and model.
        private readonly IMapper _mapper;
        /// The logger instance for logging user repository-related activities.
        private readonly ILogger<UsersRepository> _logger;
        private readonly GetLoggedInUserId _getLoggedInUserId;

        #endregion

        #region Parameterized Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersRepository"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        /// <param name="mapper">The AutoMapper instance for mapping between entity and model.</param>
        /// <param name="logger">The logger instance for logging user repository-related activities.</param>
        /// <remarks>
        /// Date: 16-03-2024
        /// Developer: Khushi Shah
        /// </remarks>
        public UsersRepository(AatithyaDbContext context, IMapper mapper, ILogger<UsersRepository> logger, GetLoggedInUserId getLoggedInUserId)
        {
            _context = context;
            _mapper = mapper;
            // Set the logger instance for logging user repository-related activities
            _logger = logger;
            _getLoggedInUserId = getLoggedInUserId;
        }

        #endregion

        #region Get All Users
        /// <summary>
        /// Retrieves all non-deleted users from the database.
        /// </summary>
        /// <returns>A collection of users.</returns>
        /// <remarks>
        /// Date: 16-03-2024
        /// Developer: Khushi Shah
        /// </remarks>
        public async Task<ResponseModel> GetAllUsers()
        {
            // Create a new list to hold user models
            List<ListUserModel> model = new List<ListUserModel>();

            // Create a new response model
            ResponseModel response = new ResponseModel();

            try
            {
                // Retrieve all users from the database where IsDeleted is false
                var result = await _context.Users
                    .ToListAsync();

                if (result.Any())
                {
                    // Map the list of user entities to user models
                    model = _mapper.Map<List<ListUserModel>>(result);

                    // Prepare a successful response
                    response.IsSuccess = true;
                    response.Status = HttpStatusCode.OK;
                    response.Message = string.Format(MessageNotification.GetMessage((int)StatusId.DataFound), "User");
                    response.Data = model;
                }
                else
                {
                    // Prepare a response indicating that no users were found
                    response.IsSuccess = false;
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = string.Format(MessageNotification.GetMessage((int)StatusId.DataNotFound), "User");
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

            // Return a tuple containing the response model and the list of user models
            return response;
        }
        #endregion

        #region Insert Users
        /// <summary>
        /// Adds a new user and associated user details to the database.
        /// </summary>
        /// <param name="model">The Users model to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// Date: 16-03-2024
        /// Developer: Khushi Shah
        /// </remarks>
        public async Task<ResponseModel> InsertUser(AddUserModel model)
        {
            // Create a new response model
            ResponseModel response = new ResponseModel();

            try
            {
                int userId = (int)_getLoggedInUserId.GetUserIdFromToken();
                if (userId == null)
                {
                    response.IsSuccess = false;
                    response.Status = HttpStatusCode.Unauthorized;
                    response.Message = "User ID not found in token.";
                    return await Task.FromResult(response);
                }

                // Check if a user with the same username already exists
                if (await _context.Users.AnyAsync(u => u.UserName == model.UserName))
                {
                    // Return early with a response indicating that the user already exists
                    response.IsSuccess = false;
                    response.Status = HttpStatusCode.BadRequest;
                    response.Message = string.Format(MessageNotification.GetMessage((int)StatusId.AlreadyExist), "User");
                    return response;
                }

                // Add the mapped user model to the database
                _context.Users.Add(_mapper.Map<User>(model));
                _context.SaveChanges();

                // Prepare a successful response
                response.IsSuccess = true;
                response.Status = HttpStatusCode.OK;
                response.Message = string.Format(MessageNotification.GetMessage((int)StatusId.Insert), "User");
                response.Data = model;
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

            // Return the response model
            return response;
        }

        #endregion

        #region Get User By it's Id
        /// <summary>
        /// Retrieves a user by their unique identifier from the database.
        /// </summary>
        /// <param name="Id">The unique identifier of the user.</param>
        /// <returns>A task representing the asynchronous operation. The retrieved User object or null if not found.</returns>
        /// <remarks>
        /// Date: 16-03-2024
        /// Developer: Khushi Shah
        /// </remarks>
        public async Task<ResponseModel> GetUserById(int Id)
        {
            // Create a new user model
            ListUserModel model = new ListUserModel();

            // Create a new response model
            ResponseModel response = new ResponseModel();

            try
            {
                // Retrieve the user from the database based on the provided ID
                var result = await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);

                if (result != null)
                {
                    // Map the user entity to the user model
                    model = _mapper.Map<ListUserModel>(result);

                    // Prepare a successful response
                    response.IsSuccess = true;
                    response.Status = HttpStatusCode.OK;
                    response.Message = string.Format(MessageNotification.GetMessage((int)StatusId.DataFound), "User");
                    response.Data = model;

                }
                else
                {
                    // Prepare a response indicating that the user with the provided ID was not found
                    response.IsSuccess = false;
                    response.Status = HttpStatusCode.InternalServerError;
                    response.Message = string.Format(MessageNotification.GetMessage((int)StatusId.DataNotFound), "User");
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

            // Return a tuple containing the response model and the user model
            return response;
        }
        #endregion

        #region Update Users
        /// <summary>
        /// Updates an existing user in the database with the provided model data.
        /// </summary>
        /// <param name="model">The Users model containing updated user data.</param>
        /// <returns>
        /// A task representing the asynchronous operation. 
        /// The updated User object if the update was successful, otherwise null.
        /// </returns>
        /// <remarks>
        /// Date: 16-03-2024
        /// Developer: Khushi Shah
        /// </remarks>
        public async Task<ResponseModel> UpdateUser(UpdateUserModel model)
        {
            // Create a new response model
            ResponseModel response = new ResponseModel();

            try
            {
                int userId = (int)_getLoggedInUserId.GetUserIdFromToken();
                if (userId == null)
                {
                    response.IsSuccess = false;
                    response.Status = HttpStatusCode.Unauthorized;
                    response.Message = "User ID not found in token.";
                    return await Task.FromResult(response);
                }

                // Check if the username already exists
                if (await _context.Users.AnyAsync(u => u.UserName == model.UserName && u.Id != model.Id))
                {
                    // Return early with a response indicating that the username already exists
                    response.IsSuccess = false;
                    response.Status = HttpStatusCode.BadRequest;
                    response.Message = string.Format(MessageNotification.GetMessage((int)StatusId.AlreadyExist), "User");
                    return response;
                }

                // Fetch the user details from the database
                var userDetails = await _context.Users
                    .Where(u => u.Id == model.Id)
                    .FirstOrDefaultAsync();

                if (userDetails != null)
                {
                    // Map properties from the model to the retrieved entity
                    _mapper.Map(model, userDetails);

                    // Mark the entity as modified and save changes
                    _context.Entry(userDetails).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    // Prepare a successful response
                    response.IsSuccess = true;
                    response.Status = HttpStatusCode.OK;
                    response.Message = string.Format(MessageNotification.GetMessage((int)StatusId.Update), "User");
                }
                else
                {
                    // Prepare a response indicating that the user with the provided ID was not found
                    response.IsSuccess = false;
                    response.Status = HttpStatusCode.BadRequest;
                    response.Message = string.Format(MessageNotification.GetMessage((int)StatusId.IdNotExist), "User");
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

            // Return the response model
            return response;
        }
        #endregion

        #region Delete Users
        /// <summary>
        /// Deletes a user from the database based on the provided user ID.
        /// </summary>
        /// <param name="Id">The ID of the user to be deleted.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// Date: 16-03-2024
        /// Developer: Khushi Shah
        /// </remarks>
        public async Task<ResponseModel> DeleteUser(int Id)
        {
            // Create a new response model
            ResponseModel response = new ResponseModel();

            try
            {
                int userId = (int)_getLoggedInUserId.GetUserIdFromToken();
                if (userId == null)
                {
                    response.IsSuccess = false;
                    response.Status = HttpStatusCode.Unauthorized;
                    response.Message = "User ID not found in token.";
                    return await Task.FromResult(response);
                }

                // Find the user by their ID and ensure they are not deleted already
                User? result = await _context.Users.Where(u => u.Id == Id).FirstOrDefaultAsync();

                if (result != null)
                {

                    // Save changes to the database
                    await _context.SaveChangesAsync();

                    // Prepare a successful response
                    response.IsSuccess = true;
                    response.Status = HttpStatusCode.OK;
                    response.Message = string.Format(MessageNotification.GetMessage((int)StatusId.Delete), "User");
                }
                else
                {
                    // Prepare a response indicating that the user with the provided ID was not found
                    response.IsSuccess = false;
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = string.Format(MessageNotification.GetMessage((int)StatusId.IdNotExist), "User");
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

            // Return the response model
            return response;
        }

        #endregion

    }
}
