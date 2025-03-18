using System.Net;
using AatithyaB_BLL.Services.Interface;
using AatithyaB_Core.Common;
using AatithyaB_Core.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AatithyaB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        /// The service for interacting with user data.
        private readonly IUsersService _service;
        /// The logger for logging messages within the UsersController.
        private readonly ILogger<UsersController> _logger;
        


        public UsersController(IUsersService service, ILogger<UsersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        //Get All Users

        [HttpGet]
        [Route("GetAllUsers")]

        public async Task<IActionResult> GetAllUsers()
        {
            // Initialize a response model
            ResponseModel res = new ResponseModel();

            try
            {
                // Call the service to get all users
                var response = await _service.GetAllUser();

                // Return the response with the retrieved users
                //return Ok(new { Response = response, Users = users });

                if (response.IsSuccess)
                {
                    return Ok(new { Response = response });
                }
                if (response.Status == System.Net.HttpStatusCode.NotFound)
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

                res.IsSuccess = false;
                res.Status = System.Net.HttpStatusCode.InternalServerError;
                res.Message = ex.Message;

                return BadRequest(res);

            }
        }


        // Insert User

        [HttpPost]
        [Route("InsertUser")]

        public async Task <IActionResult> Insert(AddUser model)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                // Check if the model state is valid
                if (!ModelState.IsValid)
                {
                    // Log the error message to the database using Serilog
                    _logger.LogError("Validation Error: The model state is invalid.");

                    // Prepare a bad request response
                    res.IsSuccess = false;
                    res.Status = System.Net.HttpStatusCode.BadRequest;
                    res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.ProcessingError));
                    return BadRequest(res); // Return BadRequest with the response model
                }

                // Insert the user using the service
                var response = await _service.InsertUser(model);

                // Check the response from the service
                if (response.Status == System.Net.HttpStatusCode.BadRequest)

                {
                    return BadRequest(response); // Return BadRequest with the response model

                }
                else if (response.Status == System.Net.HttpStatusCode.InternalServerError)
                {
                    return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, response);
                }
                else if (response.Status == System.Net.HttpStatusCode.OK)
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
                res.IsSuccess = false;
                res.Status = HttpStatusCode.InternalServerError;
                res.Message = ex.Message;
                return BadRequest(res);
            }


        }


        //User Data Fetch By Id 

        [HttpGet]
        [Route("GetUserById/{Id}")]
        public async Task<IActionResult> GetUserById(int Id)
        {

            Console.WriteLine(Id);
            // Initialize a response model
            ResponseModel res = new ResponseModel();

            try
            {
                // Call the service to get all users
                var response = await _service.GetUserById(Id);

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
                res.IsSuccess = false;
                res.Status = HttpStatusCode.InternalServerError;
                res.Message = ex.Message;
                return BadRequest(res);
            }
        }

        // Update User

        [HttpPut]
        [Route("UpdateUser")]

        public async Task<IActionResult> Update(UpdateUser model)
        {
            ResponseModel res = new ResponseModel();

            try
            {
                // Check if the model state is valid
                if (!ModelState.IsValid)
                {
                    // Log the error message to the database using Serilog
                    _logger.LogError("Validation Error: The model state is invalid.");

                    // Prepare a bad request response

                    res.IsSuccess = false;
                    res.Status = HttpStatusCode.BadRequest;
                    res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.ProcessingError));
                    return BadRequest(res); // Return BadRequest with the response model
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
                res.IsSuccess = false;
                res.Status = HttpStatusCode.InternalServerError;
                res.Message = ex.Message;
                return Ok(res);
            }
        }

        // Delete User

        [HttpDelete]
        [Route("DeleteUser/{id}")]

        public async Task<IActionResult> DeleteUser(int id)
        {
            ResponseModel res = new ResponseModel();

            try
            {
                // Check if the provided ID is greater than zero
                if (id > 0)
                {
                    // Delete the user using the service
                    var response = await _service.DeleteUser(id);
                    return Ok(response);
                }

                // Log the error message using Serilog
                _logger.LogError("Error: Id must be greater than zero.");

                // Prepare a bad request response
                res.IsSuccess = false;
                res.Status = HttpStatusCode.BadRequest;
                res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.IdGreaterThanZero), "User");
                return BadRequest(res);

            }

            catch (Exception ex)
            {
                // Log the error message using Serilog
                _logger.LogError("An error occurred: {ErrorMessage}", ex.Message);

                // Prepare a bad request response
                res.IsSuccess = false;
                res.Status = HttpStatusCode.InternalServerError;
                res.Message = ex.Message;
                return BadRequest(res);
            }
        }

    }
}
