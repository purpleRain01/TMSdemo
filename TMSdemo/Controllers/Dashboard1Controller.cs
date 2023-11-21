using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMSdemo.Models;
using TMSdemo.DAL;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json;

namespace TMSdemo.Controllers
{
    public class Dashboard1Controller : Controller
    {
        Performance_DAL performance_DAL = new Performance_DAL();
        Addtask_DAL addtask_DAL = new Addtask_DAL();
        // GET: Dashboard1
        public ActionResult Index()
        {
            try
            {
                if (System.Web.HttpContext.Current.Session["EmployeeDetails"] is DataRow dataRow)
                {
                    if (TempData["PerformanceList"] == null)
                    {
                        List<Performance> performanceList = new List<Performance>();
                        HttpCookie cookie2 = Request.Cookies["Id"];
                        performanceList = performance_DAL.GetPerformance(cookie2.Value);
                        return View(performanceList);
                    }
                    else
                    {
                        List<Performance> performanceList1 = TempData["PerformanceList"] as List<Performance>;
                        return View(performanceList1);
                    }
                }
                else
                {
                    TempData["exception"] = "Session timeout occured";
                    return RedirectToAction("Logout", "Dashboard");
                }
                
            }
            catch (Exception ex)
            {
                TempData["Exception"] = ex.Message.ToString();
                return RedirectToAction("Index", "Error");
            }


        }

        public ActionResult IndexFiltered(string id, string sD, string eD)
        {
            try
            {
                List<Performance> performanceList1 = new List<Performance>();
                HttpCookie cookie2 = Request.Cookies["Id"];
                performanceList1 = performance_DAL.GetPerformanceFiltered(cookie2.Value, id, sD, eD);
                TempData["PerformanceList"] = performanceList1;
                //return RedirectToAction("Index", "Dashboard1");
                return View(performanceList1);
            }
            catch (Exception ex)
            {
                TempData["Exception"] = ex.Message.ToString();
                return RedirectToAction("Index", "Error");
            }

        }


        //public JsonResult IndexFiltered(string id, string sD, string eD)
        //{
        //    try
        //    {
        //        List<Performance> performanceList1 = new List<Performance>();
        //        HttpCookie cookie2 = Request.Cookies["Id"];
        //        performanceList1 = performance_DAL.GetPerformanceFiltered(cookie2.Value, id, sD, eD);
        //        TempData["PerformanceList"] = performanceList1;

        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Exception"] = ex.Message.ToString();
        //        return RedirectToAction("Index", "Error");
        //    }

        //}

        public ActionResult AddTask()
        {
            string conString = ConfigurationManager.ConnectionStrings["Defaultcon"].ToString();
            DataTable dt1 = new DataTable();
            if (System.Web.HttpContext.Current.Session["EmployeeDetails"] is DataRow dataRow)
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select client_code as clcode,client_name as clname from ClientMaster";
                    SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                    connection.Open();
                    sqlDA.Fill(dt1);
                    connection.Close();
                }
                ViewBag.clients = dt1.AsEnumerable().Select(row => new SelectListItem
                {
                    Value = row["clcode"].ToString(),
                    Text = row["clname"].ToString()
                }).ToList();
                return View();
            }
            else
            {
                TempData["exception"] = "Session timeout occured";
                return RedirectToAction("Logout", "Dashboard");
            }
           
        }
        [HttpPost]
        public ActionResult SubmitTask(Task task)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session["EmployeeDetails"] is DataRow dataRow)
                {
                    HttpCookie cookie2 = Request.Cookies["Id"];
                    bool retmsg = addtask_DAL.InsertTask(task, cookie2.Value);
                    string jsonMsg = retmsg ? $"Task '{task.taskCode}' Added Successfully" : null;
                    return Json(jsonMsg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["exception"] = "Session timeout occured";
                    return RedirectToAction("Logout", "Dashboard");
                }
                
            }
            catch (Exception ex)
            {
                return Json("Error Occurred: " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetDropdowndetails(string id, string option, string prjid)
        {
            DataTable dt = new DataTable();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            try
            {
                if (System.Web.HttpContext.Current.Session["EmployeeDetails"] is DataRow dataRow)
                {
                    object responseData = null;
                    string conString = ConfigurationManager.ConnectionStrings["Defaultcon"].ToString();
                    if (option == "1")
                    {
                        using (SqlConnection connection = new SqlConnection(conString))
                        {
                            SqlCommand command = connection.CreateCommand();
                            command.CommandType = CommandType.Text;
                            command.CommandText = "SELECT DISTINCT project_code AS prjcode,project_name as prjname FROM dbo.ProjectMaster WHERE company_code='" + id + "'";
                            SqlDataAdapter sqlDA = new SqlDataAdapter(command);

                            connection.Open();
                            sqlDA.Fill(dt);
                        }

                        // Convert the DataTable to a list of dictionaries
                        foreach (DataRow dr in dt.Rows)
                        {
                            var row = dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => dr[col]);
                            rows.Add(row);
                        }
                    }
                    else if (option == "2")
                    {
                        using (SqlConnection connection = new SqlConnection(conString))
                        {
                            SqlCommand command = connection.CreateCommand();
                            command.CommandType = CommandType.Text;
                            command.CommandText = "SELECT DISTINCT module_name AS modname,module_code as modcode FROM dbo.ModulesTable WHERE project_code='" + id + "'";
                            SqlDataAdapter sqlDA = new SqlDataAdapter(command);

                            connection.Open();
                            sqlDA.Fill(dt);
                        }

                        // Convert the DataTable to a list of dictionaries
                        foreach (DataRow dr in dt.Rows)
                        {
                            var row = dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => dr[col]);
                            rows.Add(row);
                        }
                    }

                    else if (option == "3")
                    {
                        using (SqlConnection connection = new SqlConnection(conString))
                        {
                            SqlCommand command = connection.CreateCommand();
                            command.CommandType = CommandType.Text;
                            command.CommandText = "SELECT DISTINCT form_name AS formname,form_id as formid FROM dbo.FormTable WHERE project_code='" + prjid + "' and module_code = (select module_code from ModulesTable where project_code = '" + prjid + "' and module_code like '%" + id + "%')";
                            SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                            connection.Open();
                            sqlDA.Fill(dt);
                        }

                        // Convert the DataTable to a list of dictionaries
                        foreach (DataRow dr in dt.Rows)
                        {
                            var row = dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => dr[col]);
                            rows.Add(row);
                        }
                        DataTable dt1 = new DataTable();
                        DataTable dt2 = new DataTable();
                        using (SqlConnection connection = new SqlConnection(conString))
                        {
                            //TASK CODE CREATION CODE

                            SqlCommand command = connection.CreateCommand();
                            command.CommandType = CommandType.Text;
                            command.CommandText = "SELECT MAX(id) as id FROM dbo.TaskTable";
                            SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                            connection.Open();
                            sqlDA.Fill(dt1);
                            connection.Close();

                            command.CommandText = "select m.abbreviation as abr1,p.abbreviation as abr,c.website from ModulesTable m right join projectmaster p on m.project_code = p.project_code right join ClientMaster c on c.client_code = p.company_code where m.module_code ='" + id + "' ";
                            SqlDataAdapter sqlDA1 = new SqlDataAdapter(command);
                            connection.Open();
                            sqlDA1.Fill(dt2);
                            connection.Close();
                        }

                        string taskid = dt1.Rows[0]["id"].ToString();
                        if (taskid == "")
                        {
                            taskid = "1";
                        }
                        string taskname = dt2.Rows[0]["website"].ToString() + dt2.Rows[0]["abr"].ToString() + "-" + dt2.Rows[0]["abr1"].ToString();

                        responseData = new
                        {
                            Rows = rows,
                            AdditionalData = taskname + taskid
                        };
                        return Json(responseData, JsonRequestBehavior.AllowGet);

                    }

                    return Json(rows, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["exception"] = "Session timeout occured";
                    return RedirectToAction("Logout", "Dashboard");
                }
                
            }
            catch (Exception ex)
            {
                TempData["Exception"] = ex.Message.ToString();
                return Json(rows, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GetDropdowndetails1(string id, string option, string prjid)
        {
            DataTable dt = new DataTable();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            try
            {
                if (System.Web.HttpContext.Current.Session["EmployeeDetails"] is DataRow dataRow)
                {
                    //no code to be run ;
                }
                else
                {
                    TempData["exception"] = "Session timeout occured";
                    return RedirectToAction("Logout", "Dashboard");
                }
                string conString = ConfigurationManager.ConnectionStrings["Defaultcon"].ToString();
                if (option == "0")
                {
                    using (SqlConnection connection = new SqlConnection(conString))
                    {
                        SqlCommand command = connection.CreateCommand();
                        command.CommandType = CommandType.Text;
                        command.CommandText = "SELECT DISTINCT client_code AS clcode,client_name as clname FROM dbo.ClientMaster";
                        SqlDataAdapter sqlDA = new SqlDataAdapter(command);

                        connection.Open();
                        sqlDA.Fill(dt);
                    }

                    // Convert the DataTable to a list of dictionaries
                    foreach (DataRow dr in dt.Rows)
                    {
                        var row = dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => dr[col]);
                        rows.Add(row);
                    }
                }
                if (option == "1")
                {
                    using (SqlConnection connection = new SqlConnection(conString))
                    {
                        SqlCommand command = connection.CreateCommand();
                        command.CommandType = CommandType.Text;
                        command.CommandText = "SELECT DISTINCT project_code AS prjcode,project_name as prjname FROM dbo.ProjectMaster";
                        SqlDataAdapter sqlDA = new SqlDataAdapter(command);

                        connection.Open();
                        sqlDA.Fill(dt);
                    }

                    // Convert the DataTable to a list of dictionaries
                    foreach (DataRow dr in dt.Rows)
                    {
                        var row = dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => dr[col]);
                        rows.Add(row);
                    }
                }
                else if (option == "2")
                {
                    using (SqlConnection connection = new SqlConnection(conString))
                    {
                        SqlCommand command = connection.CreateCommand();
                        command.CommandType = CommandType.Text;
                        command.CommandText = "SELECT DISTINCT module_name AS modname,module_code as modcode FROM dbo.ModulesTable";

                        SqlDataAdapter sqlDA = new SqlDataAdapter(command);

                        connection.Open();
                        sqlDA.Fill(dt);
                    }

                    // Convert the DataTable to a list of dictionaries
                    foreach (DataRow dr in dt.Rows)
                    {
                        var row = dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => dr[col]);
                        rows.Add(row);
                    }
                }

                else if (option == "3")
                {
                    using (SqlConnection connection = new SqlConnection(conString))
                    {
                        SqlCommand command = connection.CreateCommand();
                        command.CommandType = CommandType.Text;
                        command.CommandText = "SELECT DISTINCT employee_code AS empcode,employee_name as empname FROM dbo.EmployeeMaster";
                        SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                        connection.Open();
                        sqlDA.Fill(dt);
                    }

                    // Convert the DataTable to a list of dictionaries
                    foreach (DataRow dr in dt.Rows)
                    {
                        var row = dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => dr[col]);
                        rows.Add(row);
                    }

                }

                return Json(rows, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["Exception"] = ex.Message.ToString();
                return Json(rows, JsonRequestBehavior.AllowGet);
            }
        }

    }
}