using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AatithyaB_Core.Common;
using AatithyaB_Core.Models.Contact_Us;

namespace AatithyaB_BLL.Services.Interface
{
    public interface IContact_UsService
    {
        Task<ResponseModel> GetAllContact_Us();

        Task<ResponseModel> GetContact_UsById(int id);
       
        Task<ResponseModel> InsertContact_Us(AddContacts_Us model);

        Task<ResponseModel> DeleteContact_Us(int Id);
    }
}
