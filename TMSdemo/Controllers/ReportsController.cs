using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;



using TMSdemo.DAL;
using TMSdemo.Models;

namespace TMSdemo.Controllers
{
    public class ReportsController : Controller
    {
        Report_DAL report_DAL = new Report_DAL();
        // GET: Reports
        public ActionResult ReportIndex()
        {
            try
            {
                if (System.Web.HttpContext.Current.Session["EmployeeDetails"] is DataRow dataRow)
                {
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

        public ActionResult Dropdownchange(string filterstring)
        {
            DataTable dt = new DataTable();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            try
            {
                if (System.Web.HttpContext.Current.Session["EmployeeDetails"] is DataRow dataRow)
                {
                    string conString = ConfigurationManager.ConnectionStrings["Defaultcon"].ToString();
                    if (filterstring == "emp")
                    {
                        using (SqlConnection connection = new SqlConnection(conString))
                        {
                            SqlCommand command = connection.CreateCommand();
                            command.CommandType = CommandType.Text;
                            command.CommandText = "select employee_code as empid,employee_name as empname from EmployeeMaster";
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
                    else if (filterstring == "tsk")
                    {
                        using (SqlConnection connection = new SqlConnection(conString))
                        {
                            SqlCommand command = connection.CreateCommand();
                            command.CommandType = CommandType.Text;
                            command.CommandText = "SELECT DISTINCT taskcode AS empid FROM dbo.TaskTable";
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


        public ActionResult DownloadExcel(string rptype, string role, string startT,string endT)
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
                DataTable dt = new DataTable();
                List<Task> Data = new List<Task>();
                Data = report_DAL.GetrepoortData(rptype, role, startT, endT);
                dt.Columns.Add("taskcode", typeof(string));
                dt.Columns.Add("projectcode", typeof(string));
                dt.Columns.Add("modulecode", typeof(string));
                dt.Columns.Add("date", typeof(string));
                dt.Columns.Add("status", typeof(string));
                dt.Columns.Add("name", typeof(string));
                dt.Columns.Add("reason", typeof(string));
                dt.Columns.Add("Total break", typeof(string));
                dt.Columns.Add("Total Work", typeof(string));
                dt.Columns.Add("Alloted time", typeof(string));
                dt.Columns.Add("Start time", typeof(string));
                dt.Columns.Add("End time", typeof(string));
                dt.Columns.Add("Extra time(S -for extra time)", typeof(string));
                foreach (var item in Data)
                {
                    DataRow row = dt.NewRow();
                    row[0] = item.taskCode;
                    row[1] = item.projectCode;
                    row[2] = item.moduleCode;
                    row[3] = item.sysDate;
                    row[4] = item.status;
                    row[5] = item.empname;
                    row[6] = item.reason;
                    row[7] = item.Tbreak;
                    row[8] = item.Twork;
                    row[9] = item.allotedTime;
                    row[10] = item.starttime;
                    row[11] = item.endtime;
                    row[12] = item.extratime;
                    dt.Rows.Add(row);
                }
                string startDateString = startT.Split('G')[0]; // "Fri Aug 25 2023 00:00:00"
                string endDateString = endT.Split('G')[0];
                using (var excel = new ExcelPackage())
                {
                    var worksheet = excel.Workbook.Worksheets.Add("Sheet1");
                    if (rptype == "emp")
                    {
                        worksheet.Cells["A1"].Value = "Employee Report " + role.ToString() + " [" + startDateString.ToString() + "/" + endDateString.ToString() + "]";
                    }
                    else
                    {
                        worksheet.Cells["A1"].Value = "Task Report " + role.ToString() + " [" + startDateString.ToString() + "/" + endDateString.ToString() + "]";
                    }
                    worksheet.Cells["A1:L1"].Merge = true; // Merge cells for the heading
                    worksheet.Cells["A1:L1"].Style.Font.Size = 20;
                    worksheet.Cells["A1:L1"].Style.Font.Bold = true;
                    worksheet.Cells["A1:L1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    // Add the data from the DataTable starting from row 2
                    worksheet.Cells["A2"].LoadFromDataTable(dt, true);
                    worksheet.DefaultRowHeight = 25;
                    worksheet.DefaultColWidth = 17;
                    byte[] byteArray = excel.GetAsByteArray();
                    // Return the Excel file as a downloadable file
                    return File(byteArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "report.xlsx");
                }
            }
            catch(Exception ex)
            {
                TempData["exception"] = ex.Message.ToString();
                return RedirectToAction("Logout", "Dashboard");
            }
           
        }
    }
}