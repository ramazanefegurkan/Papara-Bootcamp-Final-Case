using CommerceHub.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.Auth.Token
{
    public interface ITokenService
    {
        string GetToken(User user);
    }
}
