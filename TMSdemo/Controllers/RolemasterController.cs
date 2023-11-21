using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMSdemo.Models;
using TMSdemo.DAL;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace TMSdemo.Controllers
{
    public class RolemasterController : Controller
    {
        // GET: Rolemaster
        Employee employee = new Employee();
        Role_DAL role_DAL = new Role_DAL();
        public ActionResult Index()
        {
            try
            {
                if (System.Web.HttpContext.Current.Session["EmployeeDetails"] is DataRow dataRow)
                {

                }
                else
                {
                    TempData["exception"] = "Session timeout occured";
                    return RedirectToAction("Logout", "Dashboard");
                }
                List<Employee> Rolelist = new List<Employee>();
                HttpCookie cookie2 = Request.Cookies["Id"];
                Rolelist = role_DAL.GetRoles();
                //getting all the designations
                string conString = ConfigurationManager.ConnectionStrings["Defaultcon"].ToString();
                DataTable dt = new DataTable();
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select * from dbo.AccessTable";
                    SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                    connection.Open();
                    sqlDA.Fill(dt);
                }
                ViewBag.designation = dt.AsEnumerable().Select(row => new SelectListItem
                {
                    Value = row["access"].ToString(),
                    Text = row["designation"].ToString()
                }).ToList();

                return View(Rolelist);
            }
            catch(Exception ex)
            {
                TempData["Exception"] = ex.Message.ToString();
                return RedirectToAction("Index", "Error");
            }  
        }
        public ActionResult Rolechange(string empid,string role)
        {
            bool retmsg;
            try
            {
                if (System.Web.HttpContext.Current.Session["EmployeeDetails"] is DataRow dataRow)
                {

                }
                else
                {
                    TempData["exception"] = "Session timeout occured";
                    return RedirectToAction("Logout", "Dashboard");
                }
                retmsg = role_DAL.changeEmpRole(empid, role);
                if (retmsg)
                    return RedirectToAction("Index");
                else
                {
                    TempData["Exception"] = "Rolechange in db resulted into an issue";
                    return RedirectToAction("Index","Error");
                }     
            }
            catch(Exception ex)
            {
                TempData["Exception"] = ex.Message.ToString();
                return RedirectToAction("Index", "Error");
            }  
        }
    }
}