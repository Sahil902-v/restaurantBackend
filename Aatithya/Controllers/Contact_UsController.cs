using Aatithya_BLL.Services.Interface;
using Aatithya_Core.Common;
using Aatithya_Core.Models.User;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Aatithya_Core.Models.Contact_Us;

//namespace Aatithya.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class Contact_UsController : ControllerBase
//    {
//    }
//}

namespace Aatithya.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Contact_UsController : ControllerBase
    {
        /// The service for interacting with user data.
        private readonly IContact_UsService _service;
        /// The logger for logging messages within the UsersController.
        private readonly ILogger<Contact_UsController> _logger;
       // public int UserName;


        public Contact_UsController(IContact_UsService service, ILogger<Contact_UsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        //Get All Users

        [HttpGet]
        [Route("GetAllContact_Us")]

        public async Task<IActionResult> GetAllContact_Us()
        {
            // Initialize a response model
            ResponseModel res = new ResponseModel();

            try
            {
                // Call the service to get all users
                var response = await _service.GetAllContact_Us();

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

        [HttpPost]
        [Route("InsertContact_us")]

        public async Task<IActionResult> InsertContact_Us(AddContacts_Us model)
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
                var response = await _service.InsertContact_Us(model);

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



        //Contact_Us Data Fetch By Id 
        [HttpGet]
        [Route("GetContact_UsById/{id}")]

        public async Task<IActionResult> GetContact_UsById(int id)
        {
            // Initialize a response model
            ResponseModel res = new ResponseModel();

            try
            {
                // Call the service to get all users
                var response = await _service.GetContact_UsById(id);

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

        

        // Delete User

        [HttpDelete]
        [Route("DeleteContact_Us/{id}")]

        public async Task<IActionResult> DeleteContact_Us(int id)
        {
            ResponseModel res = new ResponseModel();

            try
            {
                // Check if the provided ID is greater than zero
                if (id > 0)
                {
                    // Delete the user using the service
                    var response = await _service.DeleteContact_Us(id);
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

