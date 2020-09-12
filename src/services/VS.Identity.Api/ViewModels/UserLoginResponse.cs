using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VS.Identity.Api.ViewModels
{
    public class UserLoginResponse
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public UserToken UserToken { get; set; }
    }
}
