using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApi.Models
{
    public class Contact
    {
        public Guid? ContactID { get; set; }
        public string full_name { get; set; }
        public string Email { get; set; }
        public string Business_phone { get; set; }

        public string password { get; set; }
    }
}