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
    public class LoggerRepository
    {

        private SqlConnection con;

        //To Handle connection related activities
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["conn"].ToString();
            con = new SqlConnection(constr);
        }

        //To view employee details with generic list 
        public List<LoggerModel> GetAllLoggerEntries()
        {
            connection();
            List<LoggerModel> loggerList = new List<LoggerModel>();
            SqlCommand com = new SqlCommand("spx_GetLogger", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            //Bind EmpModel generic list using LINQ 
            loggerList = (from DataRow dr in dt.Rows
                         select new LoggerModel()
                         {
                             ID = Convert.ToInt32(dr["ID"]),
                             StepDescription = Convert.ToString(dr["StepDescription"]),
                             LoggerDescription = Convert.ToString(dr["LoggerDescription"]),
                             DateLogged = Convert.ToString(dr["DateLogged"])
                         }).ToList();
            return loggerList;


        }

        
        public void LogError(string description, string error){
            
           // connection();
            
           //if (con != null && con.State == ConnectionState.Closed)
           // { 
           // con.Open();
           // }
           // SqlCommand com = new SqlCommand("spx_logerror", con);

           // SqlParameter paramID = new SqlParameter();
           // paramID.ParameterName = "@Error_decription";
           // paramID.Value = description;
           // com.Parameters.Add(paramID);

           // SqlParameter paramError = new SqlParameter();
           // paramError.ParameterName = "@STEP_DESCR";
           // paramError.Value = error;
           // com.Parameters.Add(paramError);

           // com.ExecuteNonQuery();

           // con.Close();            

        }
    }
}