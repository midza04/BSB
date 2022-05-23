using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dashboard.Models;

namespace Dashboard.Repository
{
    public class EmailRecipientRepository
    {
        private SqlConnection con;
        
        //To Handle connection related activities
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["conn"].ToString();
            con = new SqlConnection(constr);
        }

        //To view employee details with generic list 
        public List<EmailRecipientModel> GetAllEmailRecipients()
        {
            connection();
            List<EmailRecipientModel> emailList = new List<EmailRecipientModel>();
            SqlCommand com = new SqlCommand("subGet_Emails", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            emailList = (from DataRow dr in dt.Rows
                         select new EmailRecipientModel()
                         {
                             ID = Convert.ToInt32(dr["ID"]),
                             Username = Convert.ToString(dr["Username"]),
                             EmailAddress = Convert.ToString(dr["EmailAddress"]),
                             PrimaryRecipient = Convert.ToBoolean(dr["PrimaryRecipient"])
                         }).ToList();
            return emailList;

        }

        public void UpdateEmailRecipient(EmailRecipientModel model)
        {
            connection();
            SqlCommand com = new SqlCommand("spx_UpdateEmailRecipient", con);
            com.CommandType = CommandType.StoredProcedure;
            con.Open();

            SqlParameter paramID = new SqlParameter();
            paramID.ParameterName = "@Id";
            paramID.Value = model.ID;
            com.Parameters.Add(paramID);

            SqlParameter paramUsername = new SqlParameter();
            paramUsername.ParameterName = "@Username";
            paramUsername.Value = model.Username;
            com.Parameters.Add(paramUsername);


            SqlParameter paramEmailAddress = new SqlParameter();
            paramEmailAddress.ParameterName = "@EmailAddress";
            paramEmailAddress.Value = model.EmailAddress;
            com.Parameters.Add(paramEmailAddress);

            SqlParameter paramPrimary = new SqlParameter();
            paramPrimary.ParameterName = "@PrimaryRecipient";
            paramPrimary.Value = model.PrimaryRecipient;
            com.Parameters.Add(paramPrimary);

            com.ExecuteNonQuery();            
            
            con.Close();

        }

        public void InsertEmailRecipient(EmailRecipientModel model)
        {
            connection();
            SqlCommand com = new SqlCommand("spx_InsertEmailRecipient", con);
            com.CommandType = CommandType.StoredProcedure;
            con.Open();

            SqlParameter paramUsername = new SqlParameter();
            paramUsername.ParameterName = "@Username";
            paramUsername.Value = model.Username;
            com.Parameters.Add(paramUsername);


            SqlParameter paramEmailAddress = new SqlParameter();
            paramEmailAddress.ParameterName = "@EmailAddress";
            paramEmailAddress.Value = model.EmailAddress;
            com.Parameters.Add(paramEmailAddress);

            SqlParameter paramPrimary = new SqlParameter();
            paramPrimary.ParameterName = "@PrimaryRecipient";
            paramPrimary.Value = model.PrimaryRecipient;
            com.Parameters.Add(paramPrimary);

            com.ExecuteNonQuery();

            con.Close();

        }

        public void DeleteEmailRecipient(int ID)
        {
            connection();
            SqlCommand com = new SqlCommand("spx_DeleteEmailRecipient", con);
            com.CommandType = CommandType.StoredProcedure;
            con.Open();

            SqlParameter paramID = new SqlParameter();
            paramID.ParameterName = "@Id";
            paramID.Value = ID;
            com.Parameters.Add(paramID);

            com.ExecuteNonQuery();

            con.Close();
        }

        public EmailRecipientModel GetEmailRecipient(int Id) 
        {
            connection();
            EmailRecipientModel emailModel;
            SqlCommand com = new SqlCommand("subGet_Email", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlParameter paramID = new SqlParameter();
            paramID.ParameterName = "@Id";
            paramID.Value = Id;

            com.Parameters.Add(paramID);
            con.Open();

            using (var reader = com.ExecuteReader())
            {
                reader.Read();

                emailModel = new EmailRecipientModel
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    Username = Convert.ToString(reader["Username"]),
                    EmailAddress = Convert.ToString(reader["EmailAddress"]),
                    PrimaryRecipient = Convert.ToBoolean(reader["PrimaryRecipient"])                    
                };
            }

            con.Close();
            
            return emailModel;
        }
    }
}