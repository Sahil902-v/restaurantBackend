using System.Linq;
using System.Net;
using Aatithya_Core.Common;
using Aatithya_Core.Models.Contact_Us;
using Aatithya_Core.Models.Images;
using Aatithya_DAL.Entities;
using Aatithya_DAL.Repository.Interface;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Components.Forms.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Aatithya_DAL.Repository.Implemantation
{
    public class ImageRepository : IImageRepository

    {
        private readonly AatithyaDbContext _context;

        private readonly IMapper _mapper;

        private readonly ILogger<IImageRepository> _logger;


        public ImageRepository(AatithyaDbContext context, IMapper mapper, ILogger<IImageRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // Get All Images 

        public async Task<ResponseModel> GetAllImages()
        {
            List<ListImages> model = new();

            ResponseModel res = new ResponseModel();
            try
            {
                var result = await _context.Images
                   .ToArrayAsync();
                if (result.Any())
                {
                    model = _mapper.Map<List<ListImages>>(result);
                    res.IsSuccess = true;
                    res.Status = System.Net.HttpStatusCode.OK;
                    res.Message = "Iamges found Successfully";
                    res.Data = model;
                }
                else
                {
                    res.IsSuccess = false;
                    res.Status = System.Net.HttpStatusCode.OK;
                    res.Message = "Iamges not found ";
                }

            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Status = System.Net.HttpStatusCode.InternalServerError;
                res.Message = ex.Message;
            }
            return res;
        }
        // Get Image By Id

        public async Task<ResponseModel> GetImageById(int id)
        {
            // Create a new Image model
            ListImages model = new ListImages();

            // Create a new response model
            ResponseModel res = new ResponseModel();

            try
            {
                // Retrieve the user from the database based on the provided ID
                var result = await _context.Images.FirstOrDefaultAsync(u => u.Id == id);

                if (result != null)
                {
                    // Map the user entity to the Image model
                    model = _mapper.Map<ListImages>(result);

                    // Prepare a successful response
                    res.IsSuccess = true;
                    res.Status = System.Net.HttpStatusCode.OK;
                    res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.DataFound), "Images");
                    res.Data = model;
                }
                else
                {
                    // Prepare a response indicating that the user with the provided ID was not found
                    res.IsSuccess = false;
                    res.Status = System.Net.HttpStatusCode.InternalServerError;
                    res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.DataNotFound), "Images");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {ErrorMessage}", ex.Message);

                // Prepare an error response
                res.IsSuccess = false;
                res.Status = HttpStatusCode.InternalServerError;
                res.Message = ex.Message;

            }
            // Return a tuple containing the response model and the Image model
            return res;


        }

// Get Image by categoryId
        public async Task<ResponseModel> GetImageByCategoryId(int id)
        {
            // Create a new Image model
            List<ListImages> model = new();

            // Create a new response model
            ResponseModel res = new ResponseModel();

            try
            {
                // Retrieve the user from the database based on the provided ID
                var result = await _context.Images.Where(u => u.ImgCategory == id).ToListAsync();

                if (result != null)
                {
                    // Map the user entity to the Image model
                    model = _mapper.Map<List<ListImages>>(result);

                    // Prepare a successful response
                    res.IsSuccess = true;
                    res.Status = System.Net.HttpStatusCode.OK;
                    res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.DataFound), "Images");
                    res.Data = model;
                }
                else
                {
                    // Prepare a response indicating that the user with the provided ID was not found
                    res.IsSuccess = false;
                    res.Status = System.Net.HttpStatusCode.InternalServerError;
                    res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.DataNotFound), "Images");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {ErrorMessage}", ex.Message);

                // Prepare an error response
                res.IsSuccess = false;
                res.Status = HttpStatusCode.InternalServerError;
                res.Message = ex.Message;

            }
            // Return a tuple containing the response model and the Image model
            return res;
        }

        // Insert Images
        public async Task<ResponseModel> InsertImage(AddImages addImages, IFormFile files)
        {
            ResponseModel res = new ResponseModel();
            List<AddImages> images = new List<AddImages>();
            try
            {
                // Validate file presence
                if (files == null)
                {
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Status = HttpStatusCode.BadRequest,
                        Message = "No files uploaded."
                    };
                }

                const long maxFileSize = 25 * 1024 * 1024; // 25MB
                var allowedFileTypes = new[] { ".jpg", ".jpeg", ".png" };

                // Validate file size
                if (files.Length > maxFileSize)
                {
                    res.IsSuccess = false;
                    res.Status = HttpStatusCode.BadRequest;
                    res.Message = $"File '{files.FileName}' exceeds the size limit of 5 MB.";
                    return res;

                }

                // Validate file type
                var fileExtension = Path.GetExtension(files.FileName).ToLowerInvariant();
                if (!allowedFileTypes.Contains(fileExtension))
                {
                    res.IsSuccess = false;
                    res.Status = HttpStatusCode.BadRequest;
                    res.Message = $"Invalid file type for '{files.FileName}'. Only .jpg, .jpeg, .png are allowed.";
                    return res;

                }

                // Determine upload directory based on category
                string uploadDir = addImages.ImgCategory switch
                {
                    0 => Path.Combine(Directory.GetCurrentDirectory(), "Menu_img"),
                    1 => Path.Combine(Directory.GetCurrentDirectory(), "Gallery_img"),
                    2 => Path.Combine(Directory.GetCurrentDirectory(), "Banquet_img"),
                    3 => Path.Combine(Directory.GetCurrentDirectory(), "Restaurant_img"),
                    4 => Path.Combine(Directory.GetCurrentDirectory(), "TypesofFood_img"),
                    _ => Path.Combine(Directory.GetCurrentDirectory(), "Uploads")
                };

                // Ensure directory exists
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                // Generate a unique file name
                string sanitizedFileName = $"{Guid.NewGuid()}{fileExtension}";
                string filePath = Path.Combine(uploadDir, sanitizedFileName);

                // Save file asynchronously
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await files.CopyToAsync(stream);
                }

                // Create image record
                images.Add(new AddImages
                {
                    ImgUrl = sanitizedFileName,
                    ImgCategory = addImages.ImgCategory,
                    ImgTitle = addImages.ImgTitle
                    
                    
                });

                if (!images.Any())
                {
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Status = HttpStatusCode.BadRequest,
                        Message = "No valid images were uploaded."
                    };
                }

                var model = _mapper.Map<List<Entities.Image>>(images);
                await _context.Images.AddRangeAsync(model);
                await _context.SaveChangesAsync();

                res.IsSuccess = true;
                res.Status = HttpStatusCode.OK;
                res.Message = "Images uploaded successfully.";
                res.Data = sanitizedFileName;
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Status = HttpStatusCode.InternalServerError;
                res.Message = $"An error occurred while uploading the logo: {ex.Message}";
                // Consider logging the exception here
            }
            return res;
        }

        //Delete Image
        public async Task<ResponseModel> DeleteImage(int id)
        {
            // Create a new response model
            ResponseModel res = new ResponseModel();

            try
            {

                // Find the user by their ID and ensure they are not deleted already
                Image? result = await _context.Images.Where(u => u.Id == id).FirstOrDefaultAsync();

                if (result != null)
                {
                    //Delete Contact
                    _context.Images.Remove(result);
                    _context.SaveChanges();
                    // Prepare a successful response
                    res.IsSuccess = true;
                    res.Status = HttpStatusCode.OK;
                    res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.Delete), "Images");
                }
                else
                {
                    // Prepare a response indicating that the user with the provided ID was not found
                    res.IsSuccess = false;
                    res.Status = HttpStatusCode.NotFound;
                    res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.IdNotExist), "Images");
                }
            }
            catch (Exception ex)
            {
                // Log the error message to the database using Serilog
                _logger.LogError("An error occurred: {ErrorMessage}", ex.Message);

                // Prepare an error response
                res.IsSuccess = false;
                res.Status = HttpStatusCode.InternalServerError;
                res.Message = ex.Message;
            }

            // Return the response model
            return res;
        }
    }
}
