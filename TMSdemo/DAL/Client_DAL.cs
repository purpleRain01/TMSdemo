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
    public class Client_DAL
    {
        string conString = ConfigurationManager.ConnectionStrings["Defaultcon"].ToString();
        public bool InsertForm(Client client, string empid)
        {
            int sqlop = 0;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@prjcode", client.projectid);
                command.Parameters.AddWithValue("@modulecode", client.moduleid);
                command.Parameters.AddWithValue("@formane", client.formname);

                command.CommandText = "sp_InsertForm";
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
        public bool InsertModule(Client client, string empid)
        {
            int sqlop = 0;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@modname", client.moduleName);
                command.Parameters.AddWithValue("@abbr", client.modAbbreviation);
                command.Parameters.AddWithValue("@prjid", client.projectid);

                command.CommandText = "sp_InsertModule";
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
        public bool InsertProject(Client client, string empid)
        {
            int sqlop = 0;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@prjname", client.projecttName);
                command.Parameters.AddWithValue("@abbr", client.prjAbbreviation);
                command.Parameters.AddWithValue("@clid", client.clientid);

                command.CommandText = "sp_InsertProject";

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
        public bool InsertClient(Client client, string empid)
        {
            int sqlop = 0;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@clientname", client.clientName);
                command.Parameters.AddWithValue("@abbr", client.CLAbbreviation);
                command.Parameters.AddWithValue("@contact", client.contactNo);
                command.Parameters.AddWithValue("@contperson", client.contactPerson);
                command.Parameters.AddWithValue("@email", client.Email);
               
                command.CommandText = "sp_Insertclient";

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