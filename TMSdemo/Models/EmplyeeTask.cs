using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMSdemo.Models
{
    public class EmplyeeTask
    {
        public string taskcode { get; set; }

        public string empid { get; set; }

        public bool action { get; set; }
        //start is true FOR the pending task and rest button actions from running task area is false 
        public string reason { get; set; }
        public string actioncode { get; set; }
        public string temp { get; set; }
        //running and break from running task area
    }
}