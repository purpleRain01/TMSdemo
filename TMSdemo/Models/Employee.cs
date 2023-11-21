using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TMSdemo.Models
{
    public class Employee
    {
      


            [DisplayName("Employee ID")]
            public string EmpID { get; set; }

            [Required]
            [DisplayName("Employee Name")]
            public string EmpName { get; set; }

            [Required]
            [DisplayName("Date of Join")]
            public string Doj { get; set; }




            [Required]
            [DisplayName("Designation")]
            public string Designation { get; set; }

            [Required]
            [DisplayName("Address")]
            public string Address { get; set; }

            [Required]
            [DisplayName("Access")]
            public string Access { get; set; }

            [Required]
            [DisplayName("Contact")]
            public string  Contact { get; set; }

            [Required]
            [DisplayName("Password")]
            public string Password { get; set; }

        [Required]
        [DisplayName("Password")]
        public string ConfirmPassword { get; set; }

        [DisplayName("Status")]
            public string Status { get; set; }

        
    }
}