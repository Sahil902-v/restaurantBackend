using Aatithya_BLL.Services.Interface;
using Aatithya_Core.Common;
using Aatithya_Core.Models.User;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Aatithya_Core.Common;

namespace Aatithya.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        #region Fields Declaration
        /// The service for interacting with user data.
        private readonly IUsersService _service;
        /// The logger for logging messages within the UsersController.
        private readonly ILogger<UsersController> _logger;
        public int userId;
        public string UserName;
        #endregion

        #region Parameterized Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="service">The service for interacting with user data.</param>
        /// <param name="logger">The logger for logging messages within the UsersController.</param>
        public UsersController(IUsersService service, ILogger<UsersController> logger)
        {
            // Set the project service and logger instance
            _service = service;
            _logger = logger;
            //userId = Convert.ToInt32(HttpContext.Current.Session[ApplicationSession.USERID]);
            //UserName = Convert.ToString(HttpContext.Current.Session[ApplicationSession.USERNAME]);
        }

        #endregion

        #region Get All Users

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <remarks>
        /// This endpoint retrieves all users from the system.
        /// Date: 16-03-2024
        /// Developer: Khushi Shah
        /// </remarks>
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            // Initialize a response model
            ResponseModel responseModel = new ResponseModel();

            try
            {
                // Call the service to get all users
                var response = await _service.GetAllUsers();

                // Return the response with the retrieved users
                //return Ok(new { Response = response, Users = users });

                if (response.IsSuccess)
                {
                    return Ok(new { Response = response });
                }
                if (response.Status == HttpStatusCode.NotFound)
                {
                    return Ok(response);
                }
                else
                {
                    // Data not found or other issues
                    return StatusCode((int)response.Status, response);
                }
            }
            catch (Exception ex)
            {
                // Log the error message to the database using Serilog
                _logger.LogError("An error occurred: {ErrorMessage}", ex.Message);

                // Prepare a bad request response
                responseModel.IsSuccess = false;
                responseModel.Status = HttpStatusCode.InternalServerError;
                responseModel.Message = ex.Message;
                return BadRequest(responseModel);
            }
        }

        #endregion


        #region Insert User

        /// <summary>
        /// Inserts a new user.
        /// </summary>
        /// <remarks>
        /// This endpoint inserts a new user into the system.
        /// Date: 16-03-2024
        /// Developer: Khushi Shah
        /// </remarks>
        /// <param name="model">The user model to insert.</param>
        [HttpPost]
        [Route("InsertUser")]
        public async Task<IActionResult> Insert(AddUserModel model)
        {
            // Initialize a response model
            ResponseModel responseModel = new ResponseModel();

            try
            {
                // Check if the model state is valid
                if (!ModelState.IsValid)
                {
                    // Log the error message to the database using Serilog
                    _logger.LogError("Validation Error: The model state is invalid.");

                    // Prepare a bad request response
                    responseModel.IsSuccess = false;
                    responseModel.Status = HttpStatusCode.BadRequest;
                    responseModel.Message = string.Format(MessageNotification.GetMessage((int)StatusId.ProcessingError));
                    return BadRequest(responseModel); // Return BadRequest with the response model
                }

                //var properties = model.GetType().GetProperties();
                //    foreach (var property in properties)
                //{
                //    string value = property.GetValue(model)?.ToString() ?? string.Empty;

                //    // Example: Passing the property value to another function 
                //    var Status = ExecuteQuery(value);
                //    if(Status != null)
                //    {
                //        return BadRequest("Query contains unsafe keywords.");
                //    }
                //}

                // Insert the user using the service
                var response = await _service.InsertUser(model);

                // Check the response from the service
                if (response.Status == HttpStatusCode.BadRequest)
                {
                    return BadRequest(response); // Return BadRequest with the response model
                }
                else if (response.Status == HttpStatusCode.InternalServerError)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, response); // Return 500 with the response model
                }
                else if (response.Status == HttpStatusCode.OK)
                {
                    return Ok(response); // Return OK with the response model
                }
                else
                {
                    // Handle other potential status codes or default behavior
                    return StatusCode((int)response.Status, response); // Return the status code with the response model
                }
            }
            catch (Exception ex)
            {
                // Log the error message to the database using Serilog
                _logger.LogError("An error occurred: {ErrorMessage}", ex.Message);

                // Prepare a bad request response
                responseModel.IsSuccess = false;
                responseModel.Status = HttpStatusCode.InternalServerError;
                responseModel.Message = ex.Message;
                return BadRequest(responseModel);
            }
        }

        #endregion

        #region User Data Fetch By Id

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <remarks>
        /// This endpoint retrieves a user from the system based on their unique ID.
        /// Date: 16-03-2024
        /// Developer: Khushi Shah
        /// </remarks>
        /// <param name="Id">The ID of the user to retrieve.</param>
        [HttpGet]
        [Route("GetUserById/{Id}")]
        public async Task<IActionResult> GetUserById(int Id)
        {
            // Initialize a response model
            ResponseModel responseModel = new ResponseModel();

            try
            {
                // Call the service to get all users
                var response = await _service.GetAllUsers();

                // Return the response with the retrieved users
                //return Ok(new { Response = response, Users = users });

                if (response.IsSuccess)
                {
                    return Ok(new { Response = response });
                }
                if (response.Status == HttpStatusCode.NotFound)
                {
                    return Ok(response);
                }
                else
                {
                    // Data not found or other issues
                    return StatusCode((int)response.Status, response);
                }
            }
            catch (Exception ex)
            {
                // Log the error message to the database using Serilog
                _logger.LogError("An error occurred: {ErrorMessage}", ex.Message);

                // Prepare a bad request response
                responseModel.IsSuccess = false;
                responseModel.Status = HttpStatusCode.InternalServerError;
                responseModel.Message = ex.Message;
                return BadRequest(responseModel);
            }
        }

        #endregion


        #region Update User

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <remarks>
        /// This endpoint updates an existing user in the system.
        /// Date: 16-03-2024
        /// Developer: Khushi Shah
        /// </remarks>
        /// <param name="model">The user model to update.</param>
        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult> Update(UpdateUserModel model)
        {
            // Initialize a response model
            ResponseModel responseModel = new ResponseModel();

            try
            {
                // Check if the model state is valid
                if (!ModelState.IsValid)
                {
                    // Log the error message to the database using Serilog
                    _logger.LogError("Validation Error: The model state is invalid.");

                    // Prepare a bad request response
                    responseModel.IsSuccess = false;
                    responseModel.Status = HttpStatusCode.BadRequest;
                    responseModel.Message = string.Format(MessageNotification.GetMessage((int)StatusId.ProcessingError));
                    return BadRequest(responseModel); // Return BadRequest with the response model
                }

                // Update the user using the service
                var response = await _service.UpdateUser(model);

                // Check the response from the service
                if (response.Status == HttpStatusCode.BadRequest)
                {
                    return BadRequest(response); // Return BadRequest with the response model
                }
                else if (response.Status == HttpStatusCode.InternalServerError)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, response); // Return 500 with the response model
                }
                else if (response.Status == HttpStatusCode.OK)
                {
                    return Ok(response); // Return OK with the response model
                }
                else
                {
                    // Handle other potential status codes or default behavior
                    return StatusCode((int)response.Status, response); // Return the status code with the response model
                }
            }
            catch (Exception ex)
            {
                // Log the error message to the database using Serilog
                _logger.LogError("An error occurred: {ErrorMessage}", ex.Message);

                // Prepare a bad request response
                responseModel.IsSuccess = false;
                responseModel.Status = HttpStatusCode.InternalServerError;
                responseModel.Message = ex.Message;
                return Ok(responseModel);
            }
        }

        #endregion

        #region Delete User

        /// <summary>
        /// Deletes a user by its ID.
        /// </summary>
        /// <remarks>
        /// This endpoint deletes a user from the system.
        /// Date: 16-03-2024
        /// Developer: Khushi Shah
        /// </remarks>
        /// <param name="Id">The ID of the user to delete.</param>
        [HttpDelete]
        [Route("DeleteUser/{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            // Initialize a response model
            ResponseModel responseModel = new ResponseModel();

            try
            {
                // Check if the provided ID is greater than zero
                if (Id > 0)
                {
                    // Delete the user using the service
                    var response = await _service.DeleteUser(Id);
                    return Ok(response);
                }

                // Log the error message using Serilog
                _logger.LogError("Error: Id must be greater than zero.");

                // Prepare a bad request response
                responseModel.IsSuccess = false;
                responseModel.Status = HttpStatusCode.BadRequest;
                responseModel.Message = string.Format(MessageNotification.GetMessage((int)StatusId.IdGreaterThanZero), "User");
                return BadRequest(responseModel);

            }
            catch (Exception ex)
            {
                // Log the error message using Serilog
                _logger.LogError("An error occurred: {ErrorMessage}", ex.Message);

                // Prepare a bad request response
                responseModel.IsSuccess = false;
                responseModel.Status = HttpStatusCode.InternalServerError;
                responseModel.Message = ex.Message;
                return BadRequest(responseModel);
            }
        }

        #endregion
    }
}
