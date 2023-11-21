using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMSdemo.Models
{
    public class Performance
    {
        public string empname { get; set; }
        public string empid { get; set; }

        public string designation { get; set; }

        public string TAlloted { get; set; }

        public int ConTimecount { get; set; }
        public int NConTimecount { get; set; }

        public string TBreaktime { get; set; }

        public string TWorktime { get; set; }

        public string TSaved { get; set; }

        public string pi { get; set; }
        public string Leavetaken { get; set; }
    }
}