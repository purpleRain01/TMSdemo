using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TMSdemo.Models;

namespace TMSdemo.DAL
{
    public class Performance_DAL
    {

        //CONNECTION STRING 
        string conString = ConfigurationManager.ConnectionStrings["Defaultcon"].ToString();

        public List<Performance> GetPerformance(string empid)
        {
            List<Performance> performanceList = new List<Performance>();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                command.CommandText = "sp_GetallPerformance";
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dt = new DataTable();

                connection.Open();
                sqlDA.Fill(dt);
                connection.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    string pi1 = "";
                    double percntg1 = (Convert.ToInt32(dr["CompletedOnTimeCount"]) / (Convert.ToInt32(dr["CompletedOnTimeCount"]) + Convert.ToInt32(dr["NCompletedOnTimeCount"]))) / 2;
                    double percntg2 = Convert.ToDouble(dr["TotalWorkTimeInSeconds"]) / Convert.ToDouble(dr["TotalAllotedTimeInSeconds"]) * 100;
                    percntg2 = 100 - percntg2;
                    pi1 = percntg2.ToString("F2");
                    
                    object totalbrtime = dr["TotalBreakTimeInSeconds"];
                    if (totalbrtime == DBNull.Value)
                    {
                        performanceList.Add(new Performance
                        {
                            empname = dr["EmployeeName"].ToString(),
                            TBreaktime = "00:00:00",
                            TWorktime = TimeSpan.FromSeconds(Convert.ToInt32(dr["TotalWorkTimeInSeconds"])).ToString(),
                            ConTimecount = Convert.ToInt32(dr["CompletedOnTimeCount"]),
                            NConTimecount = Convert.ToInt32(dr["NCompletedOnTimeCount"]),
                            empid = dr["EmployeeCode"].ToString(),
                            TAlloted = TimeSpan.FromSeconds(Convert.ToInt32(dr["TotalAllotedTimeInSeconds"])).ToString(),
                            
                            pi = pi1,

                        });
                    }
                    else
                    {
                        performanceList.Add(new Performance
                        {
                            empname = dr["EmployeeName"].ToString(),
                            TBreaktime = TimeSpan.FromSeconds(Convert.ToInt32(dr["TotalBreakTimeInSeconds"])).ToString(),
                            TWorktime = TimeSpan.FromSeconds(Convert.ToInt32(dr["TotalWorkTimeInSeconds"])).ToString(),
                            ConTimecount = Convert.ToInt32(dr["CompletedOnTimeCount"]),
                            NConTimecount = Convert.ToInt32(dr["NCompletedOnTimeCount"]),
                            empid = dr["EmployeeCode"].ToString(),
                            TAlloted = TimeSpan.FromSeconds(Convert.ToInt32(dr["TotalAllotedTimeInSeconds"])).ToString(),
                            
                            pi = pi1,

                        });
                    }


                    

                }
            }

            return performanceList;
        }


        public List<Performance> GetPerformanceFiltered(string empid, string id, string sD, string eD)
        {
            List<Performance> performanceList = new List<Performance>();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                command.CommandText = "sp_GetPerformanceFiltered";
                if (id == "1")
                {
                    string s = DateTime.Now.AddDays(-30).ToString();
                    command.Parameters.AddWithValue("@startDt", DateTime.Now.AddDays(-30).ToString());
                    command.Parameters.AddWithValue("@endDt", DateTime.Now.AddDays(-60).ToString());
                }
                else if (id == "2")
                {
                    command.Parameters.AddWithValue("@startDt", DateTime.Now.ToString());
                    command.Parameters.AddWithValue("@endDt", DateTime.Now.AddDays(-365).ToString());
                }
                else if (id == "3")
                {
                    command.Parameters.AddWithValue("@startDt", sD);
                    command.Parameters.AddWithValue("@endDt", eD);
                }
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dt = new DataTable();

                connection.Open();
                sqlDA.Fill(dt);
                connection.Close();
                double percntg1 = 0.0;
                double percntg2 = 0.0;

                foreach (DataRow dr in dt.Rows)
                {
                    string pi1 = "";
                    if((Convert.ToInt32(dr["CompletedOnTimeCount"]) + Convert.ToInt32(dr["NCompletedOnTimeCount"]) != 0))
                    {
                         percntg1 = (Convert.ToInt32(dr["CompletedOnTimeCount"]) / (Convert.ToInt32(dr["CompletedOnTimeCount"]) + Convert.ToInt32(dr["NCompletedOnTimeCount"]))) / 2;
                         percntg2 = Convert.ToDouble(dr["TotalWorkTimeInSeconds"]) / Convert.ToDouble(dr["TotalAllotedTimeInSeconds"]) * 100;

                    }
                    else
                    {
                         percntg1 = 0.0;
                        percntg2 = 0.0;
                    }
                    object toatalsaved = dr["TotalBreakTimeInSeconds"];
                    if (toatalsaved != DBNull.Value)
                    {
                        if (Convert.ToInt32(dr["TotalSavedTimeInSeconds"]) < 0)
                        {
                            pi1 = "-" + percntg2.ToString("F2");
                        }
                        else
                        {
                            pi1 = "+" + percntg2.ToString("F2");
                        }
                    }
                    else
                    {
                        pi1 = "0.0";
                    }
                       

                    performanceList.Add(new Performance
                    {
                        empname = dr["EmployeeName"].ToString(),
                        TBreaktime = TimeSpan.FromSeconds(Convert.ToInt32(dr["TotalBreakTimeInSeconds"])).ToString(),
                        TWorktime = TimeSpan.FromSeconds(Convert.ToInt32(dr["TotalWorkTimeInSeconds"])).ToString(),
                        ConTimecount = Convert.ToInt32(dr["CompletedOnTimeCount"]),
                        NConTimecount = Convert.ToInt32(dr["NCompletedOnTimeCount"]),
                        empid = dr["EmployeeCode"].ToString(),
                        TAlloted = TimeSpan.FromSeconds(Convert.ToInt32(dr["TotalAllotedTimeInSeconds"])).ToString(),
                        TSaved = TimeSpan.FromSeconds(Convert.ToInt32(dr["TotalSavedTimeInSeconds"])).ToString(),
                        pi = pi1,

                    });

                }
            }

            return performanceList;


        }
    


    public DataTable getDropDownBind(string clientid)
    {
        using (SqlConnection connection = new SqlConnection(conString))
        {
            DataTable dt = new DataTable();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@clientid", clientid);
            command.CommandText = "DropdownFiller";
            SqlDataAdapter sqlDA = new SqlDataAdapter(command);
            connection.Open();
            sqlDA.Fill(dt);
            connection.Close();
            return dt;
        }

    }
}
}