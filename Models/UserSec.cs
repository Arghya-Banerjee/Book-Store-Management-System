using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


    public class UserSec 
    {
        public long Id { get; set; }
        public string RefId { get; set; }
        public string UserId { get; set; }
        public int UserType { get; set; }
        public int IsActive { get; set; }
        public int SchoolMedium { get; set; }
        public UserRole vUserRole;
        public UserSec() { }
        // added 19.7.19 for school user.to store category and subcategory, after first requisition
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        // added 03.09.21
        public int ReqActive { get; set; }

        // added 13.9.24
        public int ACAD_ID { get; set; }
        public string ACAD_YEAR { get; set; }
    }


    public enum UserRole
    {
        SCHOOL = 1,
        DIRECTORATE = 2,
        TB = 3,
        ADMIN = 4
        
    }