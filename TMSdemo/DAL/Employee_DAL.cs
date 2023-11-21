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
    public class Employee_DAL
    {

        //CONNECTION STRING 
        string conString = ConfigurationManager.ConnectionStrings["Defaultcon"].ToString();
        //getting all employees
        public List<Employee> GetallEmplpoyees()
        {
            List<Employee> employeeList = new List<Employee>();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetallEmmployees";
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dt = new DataTable();

                connection.Open();
                sqlDA.Fill(dt);
                connection.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    employeeList.Add(new Employee
                    {
                        
                        EmpID = dr["employee_code"].ToString(),
                        EmpName = dr["employee_name"].ToString(),
                       
                    }); ;
                }
            }
            return employeeList;
             
        }

            //checking user fn
            public bool CheckUser(Employee employee)
        {
            int retmsg = 0;
            using (SqlConnection connection = new SqlConnection(conString))
            {

                SqlCommand command = new SqlCommand("sp_Checkuser", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@empcode", employee.EmpID);
                command.Parameters.AddWithValue("@password", employee.Password);

                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dt = new DataTable();


                connection.Open();
                sqlDA.Fill(dt);
                connection.Close();
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    HttpContext.Current.Session["EmployeeDetails"] = row;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        //inserting emp fn
        public bool InsertEmployee(Employee employee)
        {
            int retmsg = 0;


            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command1 = connection.CreateCommand();
                command1.CommandType = CommandType.Text;
                command1.CommandText = "SELECT * FROM dbo.AccessTable";
                DataTable dtAccess = new DataTable();

                SqlDataAdapter sqlDA = new SqlDataAdapter(command1);

                connection.Open();
                sqlDA.Fill(dtAccess);
                //getting access code from the access table
                for (int i = 0; i < dtAccess.Rows.Count; i++)
                {
                    string s = dtAccess.Rows[i]["designation"].ToString().Trim();
                    if (s== employee.Designation)
                    {
                        employee.Access = dtAccess.Rows[i]["access"].ToString();
                    }
                }
            }
            using (SqlConnection connection = new SqlConnection(conString))
            {

                SqlCommand command = new SqlCommand("sp_InsertEmployees", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@empname", employee.EmpName);
                command.Parameters.AddWithValue("@designation", employee.Designation);
                command.Parameters.AddWithValue("@empcode", employee.EmpID);
                command.Parameters.AddWithValue("@access", employee.Access);

                command.Parameters.AddWithValue("@address", employee.Address);
                command.Parameters.AddWithValue("@contact", employee.Contact);
                command.Parameters.AddWithValue("@password", employee.Password);
                
                command.Parameters.AddWithValue("@status", "Active");


                connection.Open();
                retmsg = command.ExecuteNonQuery();
                connection.Close();

            }
            if (retmsg > 0)
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