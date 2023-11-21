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
    public class Report_DAL
    {
        //CONNECTION STRING 
        string conString = ConfigurationManager.ConnectionStrings["Defaultcon"].ToString();

        public List<Task> GetrepoortData(string type, string id, string start,string end)
        {
            DataTable dt = new DataTable();
            List<Task> data = new List<Task>();

            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_ReportData";
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@type", type);
                command.Parameters.AddWithValue("@sdt", start);
                command.Parameters.AddWithValue("@edt", end);
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);


                connection.Open();
                sqlDA.Fill(dt);
                connection.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    data.Add(new Task
                    {

                        taskCode = dr["taskcode"].ToString(),
                        projectCode = dr["projectcode"].ToString(),
                        moduleCode = dr["modulecode"].ToString(),
                        sysDate = dr["datetime"].ToString(),
                        status = dr["status"].ToString(),
                        empname= dr["employeename"].ToString(),
                        reason = dr["reason"].ToString(),
                        Tbreak = dr["toalbrkT"].ToString(),
                        
                        Twork= dr["totalwrkT"].ToString(),
                        allotedTime= dr["altT"].ToString(),
                        starttime= dr["startT"].ToString(),
                        endtime= dr["endT"].ToString(),

                        extratime= dr["extT"].ToString(),
                    }); ;
                }

            }
         return data;
        }
    }
}