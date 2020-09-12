using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VS.Identity.Api.Extensions
{
    public class AppSettings
    {
        public string SecretKey { get; set; }
        public double ExpirationHours { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
