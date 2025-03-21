using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aatithya_Core.Common
{
    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public HttpStatusCode Status { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
