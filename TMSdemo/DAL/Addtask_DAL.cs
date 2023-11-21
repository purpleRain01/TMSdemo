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
    public class Addtask_DAL
    {

        //CONNECTION STRING 
        string conString = ConfigurationManager.ConnectionStrings["Defaultcon"].ToString();

        public bool InsertTask(Task task,string empid)
        {
            int sqlop=0;
            string description = "";
            if (task.taskDetails.Length > 249)//need to set it as 500 in db too 
            {
                description= task.taskDetails.Substring(0, 249);
            }
            else
            {
                description = task.taskDetails;
            }

            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@taskcode", task.taskCode);
                command.Parameters.AddWithValue("@projectcode", task.projectCode);
                command.Parameters.AddWithValue("@modulecode", task.moduleCode);
                command.Parameters.AddWithValue("@formid", task.FormID);
                command.Parameters.AddWithValue("@priority", task.Priority);
                command.Parameters.AddWithValue("@enddate", task.endDate);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@clientid", task.clientID);
                command.Parameters.AddWithValue("@empid", empid);
                command.CommandText = "sp_InsertTask";

                connection.Open();
                sqlop = command.ExecuteNonQuery();
                connection.Close();

                if (sqlop != 0)
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