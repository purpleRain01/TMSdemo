using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMSdemo.DAL;

namespace TMSdemo.Controllers
{
    public class TaskassignController : Controller
    {
        // GET: Taskassign
        Task_DAL task_DAL = new Task_DAL();
        public ActionResult Tassign(string id,string val)
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
                var allTasks = task_DAL.GetallTasksforAssign();

                string conString = ConfigurationManager.ConnectionStrings["Defaultcon"].ToString();
                DataTable dt = new DataTable();
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select employee_code as empid,employee_name as empname from EmployeeMaster";

                    SqlDataAdapter sqlDA = new SqlDataAdapter(command);

                    connection.Open();
                    sqlDA.Fill(dt);
                }
                ViewBag.employees = dt.AsEnumerable().Select(row => new SelectListItem
                {
                    Value = row["empid"].ToString(),
                    Text = row["empname"].ToString()
                }).ToList();


                return View(allTasks);
            }
            catch(Exception ex)
            {
                TempData["Exception"] = ex.Message.ToString();
                return RedirectToAction("Index", "Error");
            }
            
        }
        //Js code in rolechange to avoid confusion

        public ActionResult TassignToemployee(string empid,string taskcode,string DateT)
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
                bool retmsg = false;
                string[] dateTimeParts = DateT.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
                DateTime startDate = DateTime.ParseExact(dateTimeParts[0], "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                DateTime endDate = DateTime.ParseExact(dateTimeParts[1], "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                TimeSpan timeDifference = endDate - startDate;
                string formattedTimeDifference = $"{(int)timeDifference.TotalHours:D2}:{timeDifference.Minutes:D2}:{timeDifference.Seconds:D2}";

                retmsg = task_DAL.AssignTask(empid, taskcode, formattedTimeDifference);
                if (retmsg)
                {
                    return RedirectToAction("Tassign");
                }
                else
                {
                    TempData["Exception"] = "Db insertion failed";
                    return RedirectToAction("Index","Error");
                }
            }
            catch(Exception ex)
            {
                TempData["Exception"] = ex.Message.ToString();
                return RedirectToAction("Index", "Error");
            }
            
        }

        public ActionResult TassignToTester(string empid, string taskcode, string DateT)
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
                bool retmsg = false;
                string[] dateTimeParts = DateT.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
                DateTime startDate = DateTime.ParseExact(dateTimeParts[0], "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                DateTime endDate = DateTime.ParseExact(dateTimeParts[1], "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                TimeSpan timeDifference = endDate - startDate;
                string formattedTimeDifference = $"{(int)timeDifference.TotalHours:D2}:{timeDifference.Minutes:D2}:{timeDifference.Seconds:D2}";

                retmsg = task_DAL.AssignTask(empid, taskcode, formattedTimeDifference);
                if (retmsg)
                {
                    return RedirectToAction("Tassign");
                }
                else
                {
                    TempData["Exception"] = "Db insertion failed";
                    return RedirectToAction("Index","Error");
                }
            }
            catch(Exception ex)
            {
                TempData["Exception"] = ex.Message.ToString();
                return RedirectToAction("Index", "Error");
            }
           
        }
        public ActionResult TaskchangeTime(string taskcode, string DateT)
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

                string[] dateTimeParts = DateT.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
                DateTime startDate = DateTime.ParseExact(dateTimeParts[0], "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                DateTime endDate = DateTime.ParseExact(dateTimeParts[1], "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                TimeSpan timeDifference = endDate - startDate;
                string formattedTimeDifference = $"{(int)timeDifference.TotalHours:D2}:{timeDifference.Minutes:D2}:{timeDifference.Seconds:D2}";

                bool retmsg = false;
                retmsg = task_DAL.ChangeAllotedtime(taskcode, formattedTimeDifference);
                if (retmsg)
                {
                    
                    return Json("Time changed Successfully", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Error Occured process not completed", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                TempData["Exception"] = ex.Message.ToString();
                return RedirectToAction("Index", "Error");
            }
        }
    }

}