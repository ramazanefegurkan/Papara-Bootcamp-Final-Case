using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Base
{
    public interface ISessionContext
    {
        public HttpContext HttpContext { get; set; }
        public Session Session { get; set; }
    }
}
