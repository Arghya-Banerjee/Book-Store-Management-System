using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


    public class BlockMaster
    {
        public int Id { get; set; }
        public int DistrictId { get; set; }
        public string BlockName { get; set; }
        public int Opmode { get; set; }
        // added 16.10.20
        public string District  { get; set; }
    }
