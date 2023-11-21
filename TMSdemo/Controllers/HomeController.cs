using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMSdemo.Models;
using TMSdemo.DAL;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace TMSdemo.Controllers
{
    public class HomeController : Controller
    {
        Employee_DAL employee_DAL = new Employee_DAL();
        // GET: Home
        public ActionResult Login()
        {
            Employee employee = new Employee();
            return View(employee);
        }
        [HttpPost]
        public ActionResult Login(Employee employee)
        {
            bool isEmployee = false;
            Response.Cookies["Name"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["Id"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["Admin"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["Admin1"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["Dsgn"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["starttime"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["totalwork"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["totalbreak"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["tester"].Expires = DateTime.Now.AddDays(-1);
            try
            {
                isEmployee = employee_DAL.CheckUser(employee);
                if (isEmployee)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    TempData["succmsg"] = "Please check id and password";
                    return RedirectToAction("Login");
                }

            }
            catch (Exception ex)
            {
                TempData["exception"] = ex.Message;
                return RedirectToAction("Logout", "Dashboard");
            }
        }

        public ActionResult Signup()
        {
            
            int Id = 0;
            string Empid = "";
            DataTable dtDesignation = new DataTable();
            DataTable dtEmployee = new DataTable();


            try
            {
                string conString = ConfigurationManager.ConnectionStrings["Defaultcon"].ToString();
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM dbo.OptionsTable";

                    SqlDataAdapter sqlDA = new SqlDataAdapter(command);

                    connection.Open();
                    sqlDA.Fill(dtDesignation);



                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT MAX(Id) as Id FROM dbo.EmployeeMaster";
                    sqlDA.Fill(dtEmployee);

                }
                if (dtEmployee.Rows[0]["Id"].ToString() != "")
                {
                    Id = Convert.ToInt32(dtEmployee.Rows[0]["Id"]);
                    Id += 1;
                }
                Empid = "EMP00" + Id.ToString();
                ViewBag.EmployeeId = Empid;
                ViewBag.Designation = dtDesignation.AsEnumerable()
                                        .SelectMany(row => row["Designation"].ToString().Split(':'))
                                        .Select(value => new SelectListItem
                                        {
                                            Value = value.Trim(),
                                            Text = value.Trim()
                                        })
                                        .ToList();

                Employee employee = new Employee();
                return View(employee);
            }
            catch(Exception ex)
            {
                TempData["exception"] = ex.Message.ToString();
                return RedirectToAction("Logout", "Dashboard");
            }
        }

        [HttpPost]
        public ActionResult Signup(Employee employee)
        {
            bool isInserted = false;
            try
            {

                isInserted = employee_DAL.InsertEmployee(employee);
                if (isInserted)
                {
                    TempData["succmsg"] = "Hi " + employee.EmpName + " Account created login with " + employee.EmpID;
                }
                else
                {
                    TempData["succmsg"] = "Employee data insertion failed";
                }

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                TempData["exception"] = ex.Message;
                return RedirectToAction("Logout", "Dashboard");
            }
        }
    }
}