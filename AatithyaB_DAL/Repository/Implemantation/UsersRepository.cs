using System.Linq.Expressions;
using System.Net;
using AatithyaB_Core.Common;
using AatithyaB_Core.Models.User;
using AatithyaB_DAL.Entities;
using AatithyaB_DAL.Repository.Interface;
using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace AatithyaB_DAL.Repository.Implemantation
{
    public class UsersRepository : IUsersRepository
    {
       private readonly AatithyaDbContext _context;

       private readonly IMapper _mapper;

       private readonly GetLoggedInUserId _getLoggedInUserId;

       private readonly ILogger<UsersRepository> _logger;


        public UsersRepository(AatithyaDbContext context, IMapper mapper, GetLoggedInUserId getLoggedInUserId)
        {
            _context = context;
            _mapper = mapper;
            _getLoggedInUserId = getLoggedInUserId;
        }

        // Get All Users
        public async Task<ResponseModel> GetAllUser()
        {
            List<ListUser> model = new();
            ResponseModel res = new ResponseModel();
            try
            {
                var result = await _context.Users
                    .ToArrayAsync();
                if (result.Any())
                {
                    model = _mapper.Map<List<ListUser>>(result);
                    res.IsSuccess = true;
                    res.Status = System.Net.HttpStatusCode.OK;
                    res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.DataFound), "Users");
                    res.Data = model;
                }
                else
                {
                    res.IsSuccess = false;
                    res.Status = System.Net.HttpStatusCode.OK;
                    res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.DataNotFound), "Users");
                }
            }
            catch (Exception ex) { 
                res.IsSuccess = false;
                res.Status = System.Net.HttpStatusCode.InternalServerError;
                res.Message = ex.Message;
            }
            return res;
            
        }


        // Insert Users
        public async Task<ResponseModel> InsertUser(AddUser model)
        {
            ResponseModel res = new ResponseModel();

            AddUser User = new();
            try
            {
                int uid = (int)_getLoggedInUserId.GetUserIdFromToken();
                if (uid == null)
                {
                    res.IsSuccess = false;
                    res.Status = System.Net.HttpStatusCode.Unauthorized;
                    res.Message = "You are false;";
                    return await Task.FromResult(res);
                }

                var exixting = await _context.Users.Where(u => u.Username == model.Username).FirstOrDefaultAsync();
                if (exixting != null)
                {
                    res.IsSuccess = false;
                    res.Status = System.Net.HttpStatusCode.BadRequest;
                    res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.AlreadyExist), "Users");
                    return await Task.FromResult(res);
                }

                _context.Users.Add(_mapper.Map<User>(model));
                _context.SaveChanges();
                res.IsSuccess = true;
                res.Status = System.Net.HttpStatusCode.OK;
                res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.Insert), "Users");
                res.Data = model;
            }
            catch (Exception ex) {
                res.IsSuccess = false;
                res.Status = System.Net.HttpStatusCode.InternalServerError;
                res.Message = ex.Message;
            }
            return res;
        }

        public async Task<ResponseModel> UpdateUser(UpdateUser model)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                int UserId = (int)_getLoggedInUserId.GetUserIdFromToken();
                if (UserId == null)
                {
                    res.IsSuccess = false;
                    res.Status = System.Net.HttpStatusCode.Unauthorized;
                    res.Message = "User Id Not found in taken";
                    return await Task.FromResult(res);
                }
                if (await _context.Users.AnyAsync(u => u.Username == model.Username && u.Id == model.Id))
                {
                    res.IsSuccess = false;
                    res.Status = System.Net.HttpStatusCode.BadRequest;
                    res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.AlreadyExist), "User");
                    return res;
                }

                var userDetails = await _context.Users
                    .Where(u => u.Id == model.Id)
                    .FirstOrDefaultAsync();

                if (userDetails == null)
                {
                    _mapper.Map(model, userDetails);

                    _context.Entry(userDetails).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    res.IsSuccess = true;
                    res.Status = System.Net.HttpStatusCode.OK;
                    res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.Update), "User");
                }
                else
                {
                    res.IsSuccess = false;
                    res.Status = System.Net.HttpStatusCode.BadRequest;
                    res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.IdNotExist), "User");
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
            return res;
         }

        public async Task<ResponseModel> GetUserById(int Id)
        {
            Console.WriteLine(Id);
            // Create a new user model
            ListUser model = new ListUser();

            // Create a new response model
            ResponseModel res = new ResponseModel();

            try
            {
                // Retrieve the user from the database based on the provided ID
                var result = await _context.Users.FirstOrDefaultAsync(u => u.Id == Id );

                if (result != null)
                {
                    // Map the user entity to the user model
                    model = _mapper.Map<ListUser>(result);

                    // Prepare a successful response
                    res.IsSuccess = true;
                    res.Status = HttpStatusCode.OK;
                    res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.DataFound), "User");
                    res.Data = model;

                }
                else
                {
                    // Prepare a response indicating that the user with the provided ID was not found
                    res.IsSuccess = false;
                    res.Status = HttpStatusCode.InternalServerError;
                    res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.DataNotFound), "User");
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

            // Return a tuple containing the response model and the user model
            return res;
        }




        //Delete Users
        public async Task<ResponseModel> DeleteUser(int Id)
        {
            // Create a new response model
            ResponseModel res = new ResponseModel();

            try
            {
                int userId = (int)_getLoggedInUserId.GetUserIdFromToken();
                if (userId == null)
                {
                    res.IsSuccess = false;
                    res.Status = HttpStatusCode.Unauthorized;
                    res.Message = "User ID not found in token.";
                    return await Task.FromResult(res);
                }

                // Find the user by their ID and ensure they are not deleted already
                User? result = await _context.Users.Where(u => u.Id == Id).FirstOrDefaultAsync();

                if (result != null)
                {
                    //Delete User
                    _context.Users.Remove(result);
                    _context.SaveChanges();
                    // Prepare a successful response
                    res.IsSuccess = true;
                    res.Status = HttpStatusCode.OK;
                    res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.Delete), "User");
                }
                else
                {
                    // Prepare a response indicating that the user with the provided ID was not found
                    res.IsSuccess = false;
                    res.Status = HttpStatusCode.NotFound;
                    res.Message = string.Format(MessageNotification.GetMessage((int)StatusId.IdNotExist), "User");
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
