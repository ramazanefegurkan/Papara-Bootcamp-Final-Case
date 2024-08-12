using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Base
{
    public class Session
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
    }
}
