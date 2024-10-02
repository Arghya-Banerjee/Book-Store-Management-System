using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


    public class ItemSummary
    {
        public long Id { get; set; }
        public string Item { get; set; }
        public string Class { get; set; }
        public int ClassInt { get; set; }
        //public int IsKhata { get; set; }
       // public int KhataQtyPerStd { get; set; }
      //  public long RefSummaryId { get; set; }
        public int CategoryId { get; set; }
        public int SubcategoryId { get; set; }
        public int MediumId { get; set; }
        public int Opmode { get; set; }
    }
