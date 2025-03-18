using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AatithyaB_Core.Common
{
    public enum StatusId
    {
        Insert = 1, Update = 2, Delete = 3, DataFound = 4, AlreadyExist = 5, NameRestore = 6, IdGreaterThanZero = 7, IdNotExist = 8, DataNotFound = 9, ProcessingError = 11, NotExist
    }

    public static class MessageNotification
    {
        public static string GetMessage(int status)
        {
            string message = "";
            switch (status)
            {

                case 1:
                    message = "{0} inserted successfully.";
                    break;
                case 2:
                    message = "{0} updated successfully.";
                    break;
                case 3:
                    message = "{0} deleted successfully.";
                    break;
                case 4:
                    message = "{0} data found successfully.";
                    break;
                case 5:
                    message = "{0} name already exist.";
                    break;
                case 6:
                    message = "{0} has been activated.";
                    break;
                case 7:
                    message = "{0} Id Must be grater than 0";
                    break;
                case 8:
                    message = "{0} Id does not exist.";
                    break;
                case 9:
                    message = "{0} data not found.";
                    break;
                case 10:
                    message = "{0} does not exist.";
                    break;
                case 11:
                    message = "An error occurred while processing the request.";

                    break;
                default:
                    break;
            }
            return message;
        }
    }
}
