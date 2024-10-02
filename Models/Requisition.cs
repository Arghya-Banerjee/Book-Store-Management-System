using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


    public class Requisition
    {
        public long Id { get; set; }
        public int Status { get; set; }
        public string ReqCode { get; set; }
        public string UdiseCode { get; set; }
        public DateTime? ReqDate { get; set; }
        public int CategoryId { get; set; }
        public int MediumId { get; set; }
        public int SubCategoryId { get; set; }
        public string Medium { get; set; }
        public string Category { get; set; }
        public string SubCategoryName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string xData { get; set; }
        public long RowStartIndex { get; set; }
        public long RowEndIndex { get; set; }
        public string strSortFields { get; set; }
        public string strCondition { get; set; }
        public long TotalRecords { get; set; }
        public long RowNumber { get; set; }
        public int Opmode { get; set; }
        public string ACAD_YEAR { get; set; }
        public int FYId { get; set; }
    }
    public class RequisitionDetail
    {
        public long Id { get; set; }
        public long ReqId { get; set; }
        public long ItemSumId { get; set; }
        public long ReqQuantity { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int Opmode { get; set; }
        // public int LanguageId { get; set; }
    }
    public class ReqRequest
    {
        public long Id { get; set; }
        public int Status { get; set; }
        public string ReqCode { get; set; }
        public string UdiseCode { get; set; }
        public DateTime? ReqDate { get; set; }
        public int CategoryId { get; set; }
        public int MediumId { get; set; }
        public int SubCategoryId { get; set; }
        public string Medium { get; set; }
        public string Category { get; set; }
        public string SubCategoryName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public IEnumerable<RequisitionDetail> lstDtl { get; set; }
    }


public class RequisitionDetail1
{
    public long Id { get; set; }
    public long ReqId { get; set; }
    public long ItemSumId { get; set; }
    public long ReqQuantity { get; set; }
   
}