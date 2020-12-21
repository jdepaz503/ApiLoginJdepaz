using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLoginJdepaz.Core.Domains.Login
{
    public class LoginWithPatron : LoginRequest
    {
        public string Patron { get; set; }
    }
}
