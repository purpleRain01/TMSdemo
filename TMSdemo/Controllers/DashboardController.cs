using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMSdemo.DAL;
using TMSdemo.Models;



namespace TMSdemo.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        Task_DAL task_DAL = new Task_DAL();
        [HttpGet]
        public ActionResult Index()
        {
            Employee employee = new Employee();
            // Check if the session variable exists and if it contains a valid DataRow.
            try
            {
                

                if (System.Web.HttpContext.Current.Session["EmployeeDetails"] is DataRow dataRow)
                {

                    employee.EmpName = dataRow["employee_name"].ToString().ToUpper();
                    employee.EmpID = dataRow["employee_code"].ToString();
                    employee.Access = dataRow["access"].ToString().Trim();
                    employee.Designation = dataRow["designation"].ToString().Trim();
                    //adding cookies profile name for all pages
                    HttpCookie EmpNamecookie = new HttpCookie("Name", employee.EmpName);
                    HttpCookie EmpIdcookie = new HttpCookie("Id", employee.EmpID);
                    HttpCookie EmpDsgncookie = new HttpCookie("Dsgn", employee.Designation);
                    Response.Cookies.Add(EmpNamecookie);
                    Response.Cookies.Add(EmpIdcookie);
                    Response.Cookies.Add(EmpDsgncookie);

                    if (Convert.ToInt32(employee.Access) >= 9)
                    {
                        //adding session cookies admin type
                        HttpCookie Admincookie1 = new HttpCookie("Admin1", "False");
                        Response.Cookies.Add(Admincookie1);
                        HttpCookie Admincookie = new HttpCookie("Admin", "True");
                        Response.Cookies.Add(Admincookie);
                        //adding session to for changing cookie
                        Session["Admin1"] = "False";
                        Session["Admin"] = "True";
                    }
                    
                    else if (Convert.ToInt32(employee.Access) == 8)
                    {
                        HttpCookie Admincookie1 = new HttpCookie("Admin1", "True");
                        HttpCookie Admincookie = new HttpCookie("Admin", "false");
                        Response.Cookies.Add(Admincookie);
                        Response.Cookies.Add(Admincookie1);
                        //adding session to for changing cookie
                        Session["Admin1"] = "True";
                        Session["Admin"] = "False";
                    }
                    else if (Convert.ToInt32(employee.Access) == 6)
                    {
                        HttpCookie testercookie = new HttpCookie("tester", "True");
                        Response.Cookies.Add(testercookie);
                        //adding session to for changing cookie
                        Session["tester"] = "True";
                    }

                }
                else
                {
                    TempData["exception"] = "Session timeout occured";
                    return RedirectToAction("Logout", "Dashboard");
                }

                //taking all the pendinng tasks
                var allTasks = task_DAL.GetallTasks(employee.EmpID);

                foreach (var item in allTasks)
                {
                    if (item.status == "running")
                    {

                        TempData["msg"] = "running";
                    }
                    else if (item.status == "break")
                    {
                        TempData["msg"] = "break";
                    }

                }
                return View(allTasks);

            }
            catch(Exception ex)
            {
                TempData["exception"] = ex.Message.ToString();
                return RedirectToAction("Logout", "Dashboard");
            }
        }

        public ActionResult taskIncomplete(string taskcode)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session["EmployeeDetails"] is DataRow dataRow)
                {
                    bool isInserted = false;
                    HttpCookie cookie2 = Request.Cookies["Id"];
                    isInserted = task_DAL.incompleteTask(cookie2.Value, taskcode);
                    if (isInserted)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Error");
                    }
                }
                else
                {
                    TempData["exception"] = "Session timeout occured";
                    return RedirectToAction("Logout", "Dashboard");
                }
                
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index", "Error");
            }
        }
        public ActionResult Task(string taskcode, string actionType)
        {
            //both cases entry is inserted to the transaction table
            try
            {
                if (System.Web.HttpContext.Current.Session["EmployeeDetails"] is DataRow dataRow)
                {
                    if (actionType == "pending") { actionType = "True"; }
                    else if (actionType == "hold" || actionType == "eod") { actionType = "False"; }

                    bool isInserted = false;
                    HttpCookie cookie2 = Request.Cookies["Id"];
                    EmplyeeTask emplyeeTask = new EmplyeeTask()
                    {
                        empid = cookie2.Value,
                        taskcode = taskcode,
                        action = Convert.ToBoolean(actionType),
                        actioncode = "running"

                    };


                    isInserted = task_DAL.stopOrstart(emplyeeTask, this.HttpContext);

                    if (isInserted)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["exception"] = "Session timeout occured";
                    return RedirectToAction("Logout", "Dashboard");
                }
                
            }
            catch(Exception ex)
            {
                TempData["Exception"] = ex.Message.ToString();
                return RedirectToAction("Index", "Error");
            }
        }



        //from running task button controller
        public ActionResult runningTask(string actionType)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session["EmployeeDetails"] is DataRow dataRow)
                {
                    bool isInserted = false;
                    string[] parts = actionType.Split('?');
                    string currState = parts[0];
                    string reason1 = "NIL";
                    if (parts.Length > 1 && parts[1].StartsWith("reason="))
                    {
                        reason1 = parts[1].Substring("reason=".Length);
                    }
                    HttpCookie cookie2 = Request.Cookies["Id"];
                    EmplyeeTask emplyeeTask = new EmplyeeTask()
                    {
                        empid = cookie2.Value,
                        taskcode = "0",
                        action = false,
                        actioncode = currState,
                        reason = reason1

                    };


                    isInserted = task_DAL.stopOrstart1(emplyeeTask, this.HttpContext);
                    if (isInserted)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Exception"] = "DB insertion Failed";
                        return RedirectToAction("Index", "Error");
                    }
                }
                else
                {
                    TempData["exception"] = "Session timeout occured";
                    return RedirectToAction("Logout", "Dashboard");
                }
                
            }
            catch(Exception ex)
            {
                TempData["Exception"] = ex.Message.ToString();
                return RedirectToAction("Index", "Error");
            }
           
        }

        public ActionResult EOD(string currState, string action)
        {
            try
            {
                bool isInserted = false;
                string reason = "eod";
                if (System.Web.HttpContext.Current.Session["EmployeeDetails"] is DataRow dataRow)
                {
                    EmplyeeTask emplyeeTask = new EmplyeeTask()
                    {
                        empid = dataRow["employee_code"].ToString(),
                        taskcode = currState,
                        actioncode = "eod"

                    };
                    isInserted = task_DAL.holdTask(emplyeeTask, this.HttpContext, reason);
                    if (isInserted)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Exception"] = "DB insertion Failed";
                        return RedirectToAction("Index", "Error");
                    }
                }
                else
                {
                    return RedirectToAction("Logout");//later to 404
                }
            }
            catch(Exception ex)
            {
                TempData["Exception"] = ex.Message.ToString();
                return RedirectToAction("Index", "Error");
            }

        }

        public ActionResult HoldTask( string action , string currState)
        {
            try
            {
                bool isInserted = false;
                string[] parts = currState.Split('?');
                currState = parts[0];
                string reason = "";
                if (parts.Length > 1 && parts[1].StartsWith("reason="))
                {
                    reason = parts[1].Substring("reason=".Length);
                }
                if (System.Web.HttpContext.Current.Session["EmployeeDetails"] is DataRow dataRow)
                {
                    EmplyeeTask emplyeeTask = new EmplyeeTask()
                    {
                        empid = dataRow["employee_code"].ToString(),
                        taskcode = currState,
                        actioncode = action

                    };
                    isInserted = task_DAL.holdTask(emplyeeTask, this.HttpContext, reason);
                    if (isInserted)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Exception"] = "DB insertion Failed";
                        return RedirectToAction("Index", "Error");
                    }
                }
                else
                {
                    return RedirectToAction("Logout");
                }
            }
            catch(Exception ex)
            {
                TempData["Exception"] = ex.Message.ToString();
                return RedirectToAction("Index", "Error");
            }
            
          
        }

        public ActionResult GetTime()
        {
            try
            {
                bool isDone = false;
                HttpCookie cookie2 = Request.Cookies["Id"];
                EmplyeeTask emplyeeTask = new EmplyeeTask()
                {
                    empid = cookie2.Value,
                    taskcode = "0",
                    action = false,
                    actioncode = "get"

                };
                isDone = task_DAL.stopOrstart(emplyeeTask, this.HttpContext);
                if (isDone)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Exception"] = "DB access error occured";
                    return RedirectToAction("Index", "Error");//404 redierct later
                }
            }
            catch(Exception ex)
            {
                TempData["Exception"] = ex.Message.ToString();
                return RedirectToAction("Index", "Error");
            }
        }
           
        



        public ActionResult Logout()
        {
            // Delete the "EmployeeDetails" session
            try
            {
                Response.Cookies["Name"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["Id"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["Admin"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["Admin1"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["Dsgn"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["starttime"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["totalwork"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["totalbreak"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["tester"].Expires = DateTime.Now.AddDays(-1);

                Session.Remove("EmployeeDetails");
                return RedirectToAction("Login", "Home");
            }
            catch(Exception ex)
            {
                TempData["Exception"] = ex.Message.ToString();
                return RedirectToAction("Index", "Error");
            }
        }
    }
}