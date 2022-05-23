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
    public class ParameterRepository
    {
        private SqlConnection con;
        
        //To Handle connection related activities
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["conn"].ToString();
            con = new SqlConnection(constr);
        }

        public List<ParameterModel> GetAllParameters()
        {
            connection();
            List<ParameterModel> paramList = new List<ParameterModel>();
            SqlCommand com = new SqlCommand("spx_GetParameters", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            //Bind EmpModel generic list using LINQ 
            paramList = (from DataRow dr in dt.Rows
                       select new ParameterModel()
                       {
                           ID = Convert.ToInt32(dr["ID"]),
                           Name = Convert.ToString(dr["Name"]),
                           Value = Convert.ToString(dr["Value"]),
                           Type = Convert.ToString(dr["Type"]),
                           FriendlyName = Convert.ToString(dr["FriendlyName"]),
                           Description = Convert.ToString(dr["Description"]),
                           Options = (string.IsNullOrEmpty(Convert.ToString(dr["Options"])) ? null : Convert.ToString(dr["Options"]).Split(',')),
                           Values = (string.IsNullOrEmpty(Convert.ToString(dr["Values"])) ? null : Convert.ToString(dr["Values"]).Split(','))
                       }).ToList();
            return paramList;

        }

        public EmailParameterModel GetAllEmailParameters()
        {
            connection();
            List<ParameterModel> paramList = new List<ParameterModel>();
            SqlCommand com = new SqlCommand("spx_GetEmailParameters", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
                       
            paramList = (from DataRow dr in dt.Rows
                         select new ParameterModel()
                         {
                             ID = Convert.ToInt32(dr["ID"]),
                             Name = Convert.ToString(dr["Name"]),
                             Value = Convert.ToString(dr["Value"]),
                             Type = Convert.ToString(dr["Type"]),
                             FriendlyName = Convert.ToString(dr["FriendlyName"]),
                             Description = Convert.ToString(dr["Description"])
                         }).ToList();

            #region Map DataTable rows to model.
            EmailParameterModel emailModel = new EmailParameterModel();
            emailModel.EmailSignatureName1 = paramList.Where(x => x.Name == "Email_Signature_Name_1").FirstOrDefault().Value;
            emailModel.EmailSignatureEmail1 = paramList.Where(x => x.Name == "Email_Signature_Email_1").FirstOrDefault().Value;
            emailModel.EmailSignaturePhoneNumber1 = paramList.Where(x => x.Name == "Email_Signature_PhoneNumber_1").FirstOrDefault().Value;

            emailModel.EmailSignatureName2 = paramList.Where(x => x.Name == "Email_Signature_Name_2").FirstOrDefault().Value;
            emailModel.EmailSignatureEmail2 = paramList.Where(x => x.Name == "Email_Signature_Email_2").FirstOrDefault().Value;
            emailModel.EmailSignaturePhoneNumber2 = paramList.Where(x => x.Name == "Email_Signature_PhoneNumber_2").FirstOrDefault().Value;
            emailModel.EmailNoRecordsMessage = paramList.Where(x => x.Name == "Email_No_Records_Message").FirstOrDefault().Value; 
            #endregion

            return emailModel;
        }

        public void UpdateParameters(List<ParameterModel> parameters)
        {
            connection();
            SqlCommand com = new SqlCommand("spx_UpdateParameter", con);
            com.CommandType = CommandType.StoredProcedure;
            con.Open();

            foreach (ParameterModel parameter in parameters)
            {
                SqlParameter paramID = new SqlParameter();
                paramID.ParameterName = "@Id";
                paramID.Value = parameter.ID;

                SqlParameter paramValue = new SqlParameter();
                paramValue.ParameterName = "@Value";
                
                if (parameter.Value == "Yes")
                {
                    //paramValue.Value = parameter.Value;
                    paramValue.Value = "1";
                }
                else if (parameter.Value == "No")
                {
                    paramValue.Value = "0";
                }
                else
                {
                    paramValue.Value = parameter.Value;
                }

                com.Parameters.Add(paramID);
                com.Parameters.Add(paramValue);

                com.ExecuteNonQuery();

                com.Parameters.Clear();
            }
            
            con.Close();
        }

        public void UpdateEmailParameters(EmailParameterModel emailParameters)
        {
            connection();
            SqlCommand com = new SqlCommand("spx_UpdateEmailParameter", con);
            com.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "@Email_Signature_Name_1";
            param1.Value = emailParameters.EmailSignatureName1;
            com.Parameters.Add(param1);

            SqlParameter param2 = new SqlParameter();
            param2.ParameterName = "@Email_Signature_Email_1";
            param2.Value = emailParameters.EmailSignatureEmail1;
            com.Parameters.Add(param2);

            SqlParameter param3 = new SqlParameter();
            param3.ParameterName = "@Email_Signature_PhoneNumber_1";
            param3.Value = emailParameters.EmailSignaturePhoneNumber1;
            com.Parameters.Add(param3);

            SqlParameter param4 = new SqlParameter();
            param4.ParameterName = "@Email_Signature_Name_2";
            param4.Value = emailParameters.EmailSignatureName2;
            com.Parameters.Add(param4);

            SqlParameter param5 = new SqlParameter();
            param5.ParameterName = "@Email_Signature_Email_2";
            param5.Value = emailParameters.EmailSignatureEmail2;
            com.Parameters.Add(param5);

            SqlParameter param6 = new SqlParameter();
            param6.ParameterName = "@Email_Signature_PhoneNumber_2";
            param6.Value = emailParameters.EmailSignaturePhoneNumber2;
            com.Parameters.Add(param6);


            SqlParameter param7 = new SqlParameter();
            param7.ParameterName = "@Email_No_Records_Message";
            param7.Value = emailParameters.EmailNoRecordsMessage;
            com.Parameters.Add(param7);

            com.ExecuteNonQuery();

            //com.Parameters.Clear();
            

            con.Close();
        }

    }
}