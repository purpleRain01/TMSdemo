using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMSdemo.Models
{
    public class Client
    {
        public string clientName { get; set; }
        public string clientid { get; set; }
        public string projecttName { get; set; }
        public string projectid { get; set; }

        public string moduleName { get; set; }
        public string moduleid { get; set; }

        public string formname { get; set; }

        public string CLAbbreviation { get; set; }
        public string prjAbbreviation { get; set; }
        public string modAbbreviation { get; set; }

        public string contactNo { get; set; }
        public string contactPerson { get; set; }
        public string Email { get; set; }
    }
}