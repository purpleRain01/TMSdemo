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
    public class Role_DAL
    {
        string conString = ConfigurationManager.ConnectionStrings["Defaultcon"].ToString();

        public List<Task> GettaskHistory(string id)
        {
            List<Task> history = new List<Task>();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@taskcode", id);
                command.CommandText = "sp_Tasktrack";
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dt = new DataTable();

                connection.Open();
                sqlDA.Fill(dt);
                connection.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    history.Add(new Task
                    {
                        taskCode = dr["taskcode"].ToString(),
                        projectCode = dr["projectcode"].ToString(),
                        moduleCode = dr["modulecode"].ToString(),
                        FormID = dr["formid"].ToString(),
                        sysDate = dr["datetime"].ToString(),
                        empname = dr["employeename"].ToString(),
                        status = dr["status"].ToString(),
                        reason=dr["reason"].ToString(),
                        count= dr["bgcount"].ToString(),
                        count1= dr["recount"].ToString(),

                    });

                }
            }
            return history;
        }
        public bool changeEmpRole(string empid, string role)
        {
            int updtdClmns = 0;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                command.CommandText = "sp_ChangeRole";
                command.Parameters.AddWithValue("@empid", empid);
                command.Parameters.AddWithValue("@role", role);
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                
                connection.Open();
                updtdClmns = command.ExecuteNonQuery();
                connection.Close();
            }
            if (updtdClmns != 0) { return true; }
            else { return false; }
        }
        public List<Employee> GetRoles()
        {
            List<Employee> RoleList = new List<Employee>();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                command.CommandText = "sp_GetRoles";
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dt = new DataTable();

                connection.Open();
                sqlDA.Fill(dt);
                connection.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    RoleList.Add(new Employee
                    {
                        EmpName = dr["empname"].ToString(),
                        EmpID = dr["empid"].ToString(),
                        Designation = dr["dsgn"].ToString(),
                        Access = dr["access"].ToString(),
                        Status = dr["status"].ToString(),
                    });

                }
            }
            return RoleList;
        }
    }
}