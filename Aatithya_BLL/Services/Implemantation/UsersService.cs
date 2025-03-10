using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aatithya_DAL.Repository.Interface;
using Aatithya_BLL.Services.Interface;
using Aatithya_Core.Common;
using Aatithya_Core.Models.User;
using Aatithya_DAL.Repository.Interface;
using Aatithya_Core.Common;

namespace Aatithya_BLL.Services.Implematiation
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _repository;

        public UsersService(IUsersRepository repository)
        {
            _repository = repository;
        }

        #region Get All Users Method

        /// <summary>
        /// Retrieves all users from the database.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// Date: 16-03-2024
        /// Developer: Khushi Shah
        public async Task<ResponseModel> GetAllUsers()
        {
            return await _repository.GetAllUsers();
        }


        #endregion

        #region Get User By Id

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="Id">The unique identifier of the user to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user if found; otherwise, null.</returns>
        /// Date: 16-03-2024
        /// Developer: Khushi Shah
        public async Task<ResponseModel> GetUserById(int Id)
        {
            return await _repository.GetUserById(Id);
        }

        #endregion

        #region Insert User Method

        /// <summary>
        /// Inserts a new user into the database.
        /// </summary>
        /// <param name="model">The user model to insert.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// Date: 16-03-2024
        /// Developer: Khushi Shah
        public async Task<ResponseModel> InsertUser(AddUserModel model)
        {
            return await _repository.InsertUser(model);
        }

        #endregion

        #region Update User Method

        /// <summary>
        /// Updates an existing user in the database.
        /// </summary>
        /// <param name="model">The updated user model.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// Date: 16-03-2024
        /// Developer: Khushi Shah
        public async Task<ResponseModel> UpdateUser(UpdateUserModel model)
        {
            return await _repository.UpdateUser(model);
        }

        #endregion

        #region Delete User Method

        /// <summary>
        /// Deletes a user from the database.
        /// </summary>
        /// Date: 16-03-2024
        /// Developer: Khushi Shah
        /// <param name="Id">The unique identifier of the user to delete.</param>
        public async Task<ResponseModel> DeleteUser(int Id)
        {
            return await _repository.DeleteUser(Id);
        }

        #endregion
    }
}
