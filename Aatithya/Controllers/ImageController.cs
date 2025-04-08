using System.Net;
using Aatithya_BLL.Services.Interface;
using Aatithya_Core.Common;
using Aatithya_Core.Models.Images;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aatithya.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        /// The service for interacting with user data.
        private readonly IImageService _service;
        /// The logger for logging messages within the UsersController.
        private readonly ILogger<ImageController> _logger;

        public ImageController(IImageService service, ILogger<ImageController> logger)
        {
            _service = service;
            _logger = logger;
        }

        //Get All Images

        [HttpGet]
        [Route("GetAllImages")]

        public async Task<IActionResult> GetAllImages()
        {
            // Initialize a response model
            ResponseModel res = new ResponseModel();

            try
            {
                // Call the service to get all Images
                var response = await _service.GetAllImages();

               

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

        // Fetch Image By Id 

        [HttpGet]
        [Route("GetImageById/{id}")]

        public async Task<IActionResult> GetImageById(int id)
        {
            // Initialize a response model
            ResponseModel res = new ResponseModel();

            try
            {
                // Call the service to get all users
                var response = await _service.GetImageById(id);


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

        // Fetch Image By Id 

        [HttpGet]
        [Route("GetImageByCategoryId/{id}")]

        public async Task<IActionResult> GetImageByCategoryId(int id)
        {
            // Initialize a response model
            ResponseModel res = new ResponseModel();

            try
            {
                // Call the service to get all users
                var response = await _service.GetImageByCategoryId(id);


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

        // Insert Image 
        [HttpPost]
        [Route("InsertImage")]

        public async Task<IActionResult> InsertImage([FromForm]AddImages addImages , IFormFile file)
        {
            ResponseModel res = new ResponseModel();

            try
            {
                var result = await _service.InsertImage(addImages, file);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
                    
            }
            catch (Exception ex)
            {
                var response = new ResponseModel
                {
                    IsSuccess = false,
                    Status = HttpStatusCode.InternalServerError,
                    Message = $"An unexpectd error occurred: {ex.Message}"
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }


        // Delete User

        [HttpDelete]
        [Route("DeleteImage/{id}")]

        public async Task<IActionResult> DeleteImage(int id)
        {
            ResponseModel res = new ResponseModel();

            try
            {
                // Check if the provided ID is greater than zero
                if (id > 0)
                {
                    // Delete the user using the service
                    var response = await _service.DeleteImage(id);
                    return Ok(response);
                }

                // Log the error message using Serilog
                _logger.LogError("Error: Id must be greater than zero.");

                // Prepare a bad request response
                res.IsSuccess = false;
                res.Status = HttpStatusCode.BadRequest;
                res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.IdGreaterThanZero), "Image");
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
