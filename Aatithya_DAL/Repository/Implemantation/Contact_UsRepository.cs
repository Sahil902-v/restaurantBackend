using System.Net;
using Aatithya_Core.Common;
using Aatithya_Core.Models.Contact_Us;
using Aatithya_Core.Models.User;
using Aatithya_DAL.Entities;
using Aatithya_DAL.Repository.Interface;
using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Server;

namespace Aatithya_DAL.Repository.Implemantation
{
    public class Contact_UsRepository : IContact_UsRepository
    {
        private readonly AatithyaDbContext _context;

        private readonly IMapper _mapper;

        private readonly ILogger<Contact_UsRepository> _logger;


        public Contact_UsRepository(AatithyaDbContext context, IMapper mapper, ILogger<Contact_UsRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // Get All Contact_Us Details

        public async Task<ResponseModel> GetAllContact_Us()
        {
            List<ListContacts_Us> model = new();

            ResponseModel res = new ResponseModel();
            try
            {
                var result = await _context.ContactUs
                    .ToArrayAsync();
                if (result.Any())
                {
                    model = _mapper.Map<List<ListContacts_Us>>(result);
                    res.IsSuccess = true;
                    res.Status = System.Net.HttpStatusCode.OK;
                    res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.DataFound), "Contacts_Us");
                    res.Data = model;
                }
                else
                {
                    res.IsSuccess = false;
                    res.Status = System.Net.HttpStatusCode.OK;
                    res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.DataNotFound), "Contacts_US");
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Insert model"></param>
        /// <returns></returns>

        // Insert Contact

        public async Task<ResponseModel> InsertContact_Us(AddContacts_Us model)
        {
            ResponseModel res = new ResponseModel();

            AddContacts_Us contact_Us = new();
            try
            {
                _context.ContactUs.Add(_mapper.Map<ContactU>(model));
                _context.SaveChanges();
                res.IsSuccess = true;
                res.Status = System.Net.HttpStatusCode.OK;
                res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.Insert), "Contacts_Us");
                res.Data = model;
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Status = System.Net.HttpStatusCode.InternalServerError;
                res.Message = ex.Message;
            }
            return res;
        }


        // Get Contatc_Us By Id

        public async Task<ResponseModel> GetContact_UsById(int Id)
        {
            // Create a new Contacts_Us model
            ListContacts_Us model = new ListContacts_Us();

            // Create a new response model
            ResponseModel res = new ResponseModel();

            try
            {
                // Retrieve the user from the database based on the provided ID
                var result = await _context.ContactUs.FirstOrDefaultAsync(u => u.Id == Id);

                if (result != null)
                {
                    // Map the user entity to the user model
                    model = _mapper.Map<ListContacts_Us>(result);

                    // Prepare a successful response
                    res.IsSuccess = true;
                    res.Status = System.Net.HttpStatusCode.OK;
                    res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.DataFound), "Contacts_Us");
                    res.Data = model;
                }
                else
                {
                    // Prepare a response indicating that the user with the provided ID was not found
                    res.IsSuccess = false;
                    res.Status = System.Net.HttpStatusCode.InternalServerError;
                    res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.DataNotFound), "Contact_Us");
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
            // Return a tuple containing the response model and the user model
            return res;


        }

        //Delete Contact_Us
        public async Task<ResponseModel> DeleteContact_Us(int Id)
        {
            // Create a new response model
            ResponseModel res = new ResponseModel();

            try
            {
              
                // Find the user by their ID and ensure they are not deleted already
                ContactU? result = await _context.ContactUs.Where(u => u.Id == Id).FirstOrDefaultAsync();

                if (result != null)
                {
                    //Delete Contact
                    _context.ContactUs.Remove(result);
                    _context.SaveChanges();
                    // Prepare a successful response
                    res.IsSuccess = true;
                    res.Status = HttpStatusCode.OK;
                    res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.Delete), "Contacts_Us");
                }
                else
                {
                    // Prepare a response indicating that the user with the provided ID was not found
                    res.IsSuccess = false;
                    res.Status = HttpStatusCode.NotFound;
                    res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.IdNotExist), "Contacts_Us");
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







