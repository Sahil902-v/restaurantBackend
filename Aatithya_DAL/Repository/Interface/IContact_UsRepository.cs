using Aatithya_Core.Common;
using Aatithya_Core.Models.Contact_Us;

namespace Aatithya_DAL.Repository.Interface
{
    public interface IContact_UsRepository
    {
        Task<ResponseModel> GetAllContact_Us();
      
        Task<ResponseModel> GetContact_UsById(int id);
        Task<ResponseModel> InsertContact_Us(AddContacts_Us model);

        Task<ResponseModel> DeleteContact_Us(int Id);

    }
}
