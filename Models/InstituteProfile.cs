using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


    public class InstituteProfile
    {
       
        public string RefId { get; set; }
        public string MobileNo { get; set; }
        public string MobileNoAlt { get; set; }
        public string Email { get; set; }
        public int IsActive { get; set; }
        public string UdiseCode { get; set; }
        public string InstituName { get; set; }
        public string InstituteAddress { get; set; }
        public int InstitutePin { get; set; }
        public int DistrictId { get; set; }
        public long BlockId { get; set; }
        public int Opmode { get; set; }
        public int SchoolMedium { get; set; }
    }
    public class InstituteMaster
    {
        public string UdiseCode { get; set; }
        public string InstituName { get; set; }
        public string InstituteAddress { get; set; }
        public int InstitutePin { get; set; }
        public int DistrictId { get; set; }
        public long BlockId { get; set; }
        public int Opmode { get; set; }
        public int SchoolMedium { get; set; }
        public long RowStartIndex { get; set; }
        public long RowEndIndex { get; set; }
        public string strSortFields { get; set; }
        public string strCondition { get; set; }
        public long TotalRecords { get; set; }
        public long RowNumber { get; set; }
        public string District { get; set; }
        public string BlockName { get; set; }
    }

    public class UserProfile
    {
        public long Id { get; set; }
        public int Opmode { get; set; }
        public long RowStartIndex { get; set; }
        public long RowEndIndex { get; set; }
        public string strSortFields { get; set; }
        public string strCondition { get; set; }
        public long TotalRecords { get; set; }
        public long RowNumber { get; set; }
        public string RefId { get; set; }
        public string UserId { get; set; }
        public int UserType { get; set; }
        public string MobileNo { get; set; }
    }