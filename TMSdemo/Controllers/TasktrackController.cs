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
    public class TasktrackController : Controller
    {
        // GET: Tasktrack
        Role_DAL role_DAL = new Role_DAL();
        public ActionResult Home(string id)
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
                List<Task> taskHistory = new List<Task>();
                taskHistory = role_DAL.GettaskHistory(id);
                TempData["bug"] = "0";
                TempData["reopen"] = "0";
                return View(taskHistory);
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index", "Error");
            }
            
        }
    }
}