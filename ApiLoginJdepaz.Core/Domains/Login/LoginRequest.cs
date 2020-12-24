using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApiLoginJdepaz.Core.Domains.Login
{
    public class LoginRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string username { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string pass_user { get; set; }        
    }
}
