using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TMSdemo.Models
{
    public class Task
    {
        [DisplayName("Serial No.")]
        public string id { get; set; }

        [DisplayName("Task code")]
        public string taskCode { get; set; }
        public string clientID { get; set; }

        [DisplayName("Project code")]
        public string projectCode { get; set; }

        [DisplayName("Module Code")]
        public string moduleCode { get; set; }

        [DisplayName("Module Code")]
        public string FormID { get; set; }


        [DisplayName("Priority")]
        public string Priority { get; set; }

        [DisplayName("End date")]
        public string endDate { get; set; }

        [DisplayName("Time alloted")]
        public string allotedTime { get; set; }

        [DisplayName("Assigned Date")]
        public string dateAssigned { get; set; }

        [DisplayName("Task details")]
        public string taskDetails { get; set; }

        

        [DisplayName("Status")]
        public string status { get; set; }

        [DisplayName("Start time")]
        public string starttime { get; set; }

        [DisplayName("End time")]
        public string endtime { get; set; }

        [DisplayName("reason")]
        public string reason { get; set; }

        [DisplayName("Twork")]
        public string Twork { get; set; }
        [DisplayName("Tbreak")]
        public string Tbreak { get; set; }

        public string extratime { get; set; }

        public string count { get; set; }
        public string count1 { get; set; }

        public string tableid { get; set; }

        public int entryBy { get; set; }

        [DisplayName("Server time")]
        public string sysDate { get; set; }
        [DisplayName("Employee")]
        public string empname { get; set; }




    }
}