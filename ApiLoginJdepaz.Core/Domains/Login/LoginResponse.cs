using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLoginJdepaz.Core.Domains.Login
{
    public class LoginResponse
    {
        public string Jwt { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
