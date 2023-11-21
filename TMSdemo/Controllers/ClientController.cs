using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMSdemo.Models;
using TMSdemo.DAL;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TMSdemo.Controllers
{
    public class ClientController : Controller
    {
        // GET: Client
        Client_DAL client_DAL = new Client_DAL();
        public ActionResult AddClient()
        {
            return View();
        }
        public ActionResult InsertClient(Client client)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session["EmployeeDetails"] is DataRow dataRow)
                {
                    HttpCookie cookie2 = Request.Cookies["Id"];
                    bool retmsg = client_DAL.InsertClient(client, cookie2.Value);
                    string jsonMsg = retmsg ? $"Client '{client.clientName}' Added Successfully" : null;
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
        public ActionResult AddProject()
        {
            string conString = ConfigurationManager.ConnectionStrings["Defaultcon"].ToString();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            try
            {
                if (System.Web.HttpContext.Current.Session["EmployeeDetails"] is DataRow dataRow)
                {
                    using (SqlConnection connection = new SqlConnection(conString))
                    {
                        SqlCommand command = connection.CreateCommand();
                        command.CommandType = CommandType.Text;
                        command.CommandText = "select client_code as clcode,client_name as clname from ClientMaster";
                        SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                        connection.Open();
                        sqlDA.Fill(dt);
                        connection.Close();

                        command.CommandText = "select project_code as prjcode,project_name as prjname from ProjectMaster";
                        SqlDataAdapter sqlDA1 = new SqlDataAdapter(command);
                        connection.Open();
                        sqlDA1.Fill(dt1);
                        connection.Close();
                    }
                    ViewBag.clients = dt.AsEnumerable().Select(row => new SelectListItem
                    {
                        Value = row["clcode"].ToString(),
                        Text = row["clname"].ToString()
                    }).ToList();
                    ViewBag.projects = dt1.AsEnumerable().Select(row => new SelectListItem
                    {
                        Value = row["prjcode"].ToString(),
                        Text = row["prjname"].ToString()
                    }).ToList();
                    return View();
                }
                else
                {
                    TempData["exception"] = "Session timeout occured";
                    return RedirectToAction("Logout", "Dashboard");
                }
                
            }
            catch(Exception ex)
            {
                TempData["exception"] = "Session timeout occured";
                return RedirectToAction("Logout", "Dashboard");
            }
            
        }


        public ActionResult InsertProject(Client client)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session["EmployeeDetails"] is DataRow dataRow)
                {
                    HttpCookie cookie2 = Request.Cookies["Id"];
                    bool retmsg = client_DAL.InsertProject(client, cookie2.Value);
                    string jsonMsg = retmsg ? $"Project '{client.projecttName}' Added Successfully" : null;
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

        public ActionResult InsertModule(Client client)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session["EmployeeDetails"] is DataRow dataRow)
                {
                    HttpCookie cookie2 = Request.Cookies["Id"];
                    bool retmsg = client_DAL.InsertModule(client, cookie2.Value);
                    string jsonMsg = retmsg ? $"Module '{client.projecttName}' Added Successfully" : null;
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

        public ActionResult AddForms()
        {
            string conString = ConfigurationManager.ConnectionStrings["Defaultcon"].ToString();
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select client_code as clcode,client_name as clname from ClientMaster";
                    SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                    connection.Open();
                    sqlDA.Fill(dt);
                    connection.Close();
                }
                ViewBag.clients = dt.AsEnumerable().Select(row => new SelectListItem
                {
                    Value = row["clcode"].ToString(),
                    Text = row["clname"].ToString()
                }).ToList();
                return View();
            }
            catch(Exception ex)
            {
                TempData["exception"] = "Session timeout occured";
                return RedirectToAction("Logout", "Dashboard");
            }
            
        }

        public ActionResult InsertForm(Client client)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session["EmployeeDetails"] is DataRow dataRow)
                {
                    HttpCookie cookie2 = Request.Cookies["Id"];
                    bool retmsg = client_DAL.InsertForm(client, cookie2.Value);
                    string jsonMsg = retmsg ? $"Form '{client.formname}' Added Successfully" : null;
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
    }
}