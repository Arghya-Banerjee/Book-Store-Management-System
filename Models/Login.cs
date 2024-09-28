using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tutorial1.Models
{
    public class Login
    {
        public long Id { get; set; }
        public string RefId { get; set; }
        public string MobileNo { get; set; }
        public string MobileNoAlt { get; set; }
        public string Email { get; set; }
        public int IsActive { get; set; }
        public string UserId { get; set; }
        public string UserPassword { get; set; }
        public int UserType { get; set; }
        public int ReqActive { get; set; }
        public int Opmode { get; set; }
    }
}