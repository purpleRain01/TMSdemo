using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TMSdemo.Models;
using System.Web.Mvc;

namespace TMSdemo.DAL
{
    public class Task_DAL
    {

        //CONNECTION STRING 
        string conString = ConfigurationManager.ConnectionStrings["Defaultcon"].ToString();

        public bool incompleteTask(string empid, string taskcode)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@empid", empid);
                command.Parameters.AddWithValue("@taskcode", taskcode);
                command.CommandText = "sp_incompleteTask";
                connection.Open();
                result = command.ExecuteNonQuery();
                connection.Close();
                if (result != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool holdTask(EmplyeeTask emplyeeTask, HttpContextBase httpcontext,string reason)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@empid", emplyeeTask.empid);
                command.Parameters.AddWithValue("@currState", emplyeeTask.taskcode);
                if (emplyeeTask.actioncode == "HoldTask")
                {
                    command.Parameters.AddWithValue("@flag", "0");
                }
                else if (emplyeeTask.actioncode == "eod")
                {
                    command.Parameters.AddWithValue("@flag", "1");
                }
                command.Parameters.AddWithValue("@action", emplyeeTask.actioncode);
                command.Parameters.AddWithValue("@reason", reason);
                command.CommandText = "sp_holdTask";
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);

                connection.Open();
                sqlDA.Fill(dt);
                connection.Close();

                if (dt.Rows.Count > 0)
                {
                    //adding times form db to cookies
                    object totalBreakTimeObj = dt.Rows[0]["lastbreaktime"];
                    if (totalBreakTimeObj == DBNull.Value)
                    {
                        HttpCookie starttime = new HttpCookie("starttime", "00:00:00");
                        httpcontext.Response.Cookies.Add(starttime);
                    }
                    else
                    {


                        DateTime totalworktime = (DateTime)dt.Rows[0]["totalworktime"];
                        TimeSpan worktime = totalworktime.TimeOfDay;
                        HttpCookie totalwork = new HttpCookie("totalwork", worktime.ToString(@"hh\:mm\:ss"));
                        httpcontext.Response.Cookies.Add(totalwork);

                        totalBreakTimeObj = dt.Rows[0]["totalbreaktime"];
                        if (totalBreakTimeObj != DBNull.Value)
                        {
                            DateTime totalBreakTime = (DateTime)dt.Rows[0]["totalbreaktime"];
                            TimeSpan breakTime = totalBreakTime.TimeOfDay;
                            HttpCookie totalbreak = new HttpCookie("totalbreak", breakTime.ToString(@"hh\:mm\:ss"));
                            httpcontext.Response.Cookies.Add(totalbreak);
                        }


                    }

                    return true;
                }
                else
                {
                    return false;
                }

            }

        }


        public bool stopOrstart1(EmplyeeTask emplyeeTask, HttpContextBase httpcontext)
        {

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@empid", emplyeeTask.empid);
                command.Parameters.AddWithValue("@taskcode", emplyeeTask.taskcode);
                command.Parameters.AddWithValue("@flag", emplyeeTask.action);
                command.Parameters.AddWithValue("@actioncode", emplyeeTask.actioncode);
                command.Parameters.AddWithValue("@reason", emplyeeTask.reason);
                command.CommandText = "sp_stopOrstartTask1";
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);


                connection.Open();
                sqlDA.Fill(dt);
                connection.Close();

                if (dt.Rows.Count > 0)
                {
                    //adding times form db to cookies
                    object totalBreakTimeObj = dt.Rows[0]["lastbreaktime"];
                    if (totalBreakTimeObj == DBNull.Value)
                    {
                        HttpCookie starttime = new HttpCookie("starttime", "00:00:00");
                        httpcontext.Response.Cookies.Add(starttime);
                    }
                    else
                    {


                        DateTime totalworktime = (DateTime)dt.Rows[0]["totalworktime"];
                        TimeSpan worktime = totalworktime.TimeOfDay;
                        HttpCookie totalwork = new HttpCookie("totalwork", worktime.ToString(@"hh\:mm\:ss"));
                        httpcontext.Response.Cookies.Add(totalwork);

                        totalBreakTimeObj = dt.Rows[0]["totalbreaktime"];
                        if (totalBreakTimeObj != DBNull.Value)
                        {
                            DateTime totalBreakTime = (DateTime)dt.Rows[0]["totalbreaktime"];
                            TimeSpan breakTime = totalBreakTime.TimeOfDay;
                            HttpCookie totalbreak = new HttpCookie("totalbreak", breakTime.ToString(@"hh\:mm\:ss"));
                            httpcontext.Response.Cookies.Add(totalbreak);
                        }


                    }

                    return true;
                }
                else
                {
                    return false;
                }

            }


        }
        public bool stopOrstart(EmplyeeTask emplyeeTask, HttpContextBase httpcontext)
        {

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@empid", emplyeeTask.empid);
                command.Parameters.AddWithValue("@taskcode", emplyeeTask.taskcode);
                command.Parameters.AddWithValue("@flag", emplyeeTask.action);
                command.Parameters.AddWithValue("@actioncode", emplyeeTask.actioncode);
                command.CommandText = "sp_stopOrstartTask";
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);


                connection.Open();
                sqlDA.Fill(dt);
                connection.Close();

                if (dt.Rows.Count > 0)
                {
                    //adding times form db to cookies
                    object totalBreakTimeObj = dt.Rows[0]["lastbreaktime"];
                    if (totalBreakTimeObj == DBNull.Value)
                    {
                        HttpCookie starttime = new HttpCookie("starttime", "00:00:00");
                        httpcontext.Response.Cookies.Add(starttime);
                    }
                    else
                    {


                        DateTime totalworktime = (DateTime)dt.Rows[0]["totalworktime"];
                        TimeSpan worktime = totalworktime.TimeOfDay;
                        HttpCookie totalwork = new HttpCookie("totalwork", worktime.ToString(@"hh\:mm\:ss"));
                        httpcontext.Response.Cookies.Add(totalwork);

                        totalBreakTimeObj = dt.Rows[0]["totalbreaktime"];
                        if (totalBreakTimeObj != DBNull.Value)
                        {
                            DateTime totalBreakTime = (DateTime)dt.Rows[0]["totalbreaktime"];
                            TimeSpan breakTime = totalBreakTime.TimeOfDay;
                            HttpCookie totalbreak = new HttpCookie("totalbreak", breakTime.ToString(@"hh\:mm\:ss"));
                            httpcontext.Response.Cookies.Add(totalbreak);
                        }


                    }

                    return true;
                }
                else
                {
                    return false;
                }

            }


        }

        public List<Task> GetallTasksforAssign()
        {
            List<Task> assigntasks = new List<Task>();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetallTasksforassign";
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dt = new DataTable();

                connection.Open();
                sqlDA.Fill(dt);
                connection.Close();

                foreach (DataRow dr in dt.Rows)
                {
                  
                    if (dr["status"].ToString() == "reopen")
                    {
                        assigntasks.Add(new Task
                        {

                            taskCode = dr["taskcode"].ToString(),
                            projectCode = dr["projectcode"].ToString(),
                            moduleCode = dr["modulecode"].ToString(),
                            clientID = dr["clientid"].ToString(),
                            Priority = dr["priority"].ToString(),
                            endDate = dr["enddate"].ToString(),
                            taskDetails = dr["description"].ToString(),
                            empname= dr["employee_name"].ToString(),
                            reason = dr["reason"].ToString(),
                            status = dr["status"].ToString(),
                            count=dr["recount"].ToString()

                        }); 
                    }
                    else if(dr["status"].ToString() == "cwb")
                    {
                        assigntasks.Add(new Task
                        {

                            taskCode = dr["taskcode"].ToString(),
                            projectCode = dr["projectcode"].ToString(),
                            moduleCode = dr["modulecode"].ToString(),
                            clientID = dr["clientid"].ToString(),
                            Priority = dr["priority"].ToString(),
                            endDate = dr["enddate"].ToString(),
                            taskDetails = dr["description"].ToString(),
                            empname = dr["employee_name"].ToString(),
                            reason = dr["reason"].ToString(),
                            status = dr["status"].ToString(),
                            count = dr["bgcount"].ToString()

                        });
                    }
                    // add logic for changing alloted time in each condition for accessing it in front
                    else
                    {
                        assigntasks.Add(new Task
                        {

                            taskCode = dr["taskcode"].ToString(),
                            projectCode = dr["projectcode"].ToString(),
                            moduleCode = dr["modulecode"].ToString(),
                            clientID = dr["clientid"].ToString(),
                            Priority = dr["priority"].ToString(),
                            endDate = dr["enddate"].ToString(),
                            taskDetails = dr["description"].ToString(),
                            empname = dr["employee_name"].ToString(),
                            reason = dr["reason"].ToString(),
                            status = dr["status"].ToString()

                        });
                    }
                }
            }
            return assigntasks;
        }
        public List<Task> GetallTasks(string empid)
        {
            List<Task> pendingtasks = new List<Task>();

            using (SqlConnection connection = new SqlConnection(conString))
            { 
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@empid", empid);
                command.CommandText = "sp_GetallTasks";
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                
                connection.Open();
                sqlDA.Fill(dt);
                connection.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    pendingtasks.Add(new Task
                    {

                        taskCode = dr["taskcode"].ToString(),
                        clientID = dr["clientid"].ToString(),
                        projectCode = dr["projectcode"].ToString(),
                        moduleCode = dr["modulecode"].ToString(),
                        allotedTime = dr["timealloted1"].ToString(),
                        Priority = dr["priority"].ToString(),
                        endDate = dr["enddate"].ToString(),
                        dateAssigned = dr["assigneddate"].ToString(),
                        taskDetails = dr["description"].ToString(),
                        reason = dr["reason"].ToString(),

                        starttime = dr["starttime"].ToString(),
                        endtime = dr["endtime"].ToString(),
                        status = dr["status"].ToString(),
                        tableid="task"

                    });; ;
                }
            }

            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@empid", empid);
                command.CommandText = "sp_GetallCmpltdTasks";
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dt1 = new DataTable();

                connection.Open();
                sqlDA.Fill(dt1);
                connection.Close();
                foreach (DataRow dr in dt1.Rows)
                {
                    pendingtasks.Add(new Task
                    {

                        taskCode = dr["taskcode"].ToString(),
                        clientID = dr["clientid"].ToString(),
                        projectCode = dr["projectcode"].ToString(),
                        moduleCode = dr["modulecode"].ToString(),
                        allotedTime = dr["allotedtime"].ToString(),
                        Priority = dr["priority"].ToString(),
                     
                        tableid="trn",

                        starttime = dr["starttime"].ToString(),
                        endtime = dr["endtime"].ToString(),
                        status = dr["status"].ToString(),

                    }); ;
                }

            }
                return pendingtasks;
        }

        public bool AssignTask(string empid,string taskcode,string timealloted)
        {
            int sqlreturn = 0;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@empid", empid);
                command.Parameters.AddWithValue("@taskcode", taskcode);
                command.Parameters.AddWithValue("@timealloted", timealloted);
                command.CommandText = "sp_AssignTask";
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                connection.Open();
                sqlreturn = command.ExecuteNonQuery();
                connection.Close();
            }
            if (sqlreturn != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ChangeAllotedtime(string taskcode,string newtime)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@taskcode", taskcode);
                command.Parameters.AddWithValue("@time", newtime);
                command.CommandText = "sp_ChangeAllotedtime";
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);

                connection.Open();
                int msg = command.ExecuteNonQuery();
                connection.Close();
                if (msg != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
                
        }
    }
}