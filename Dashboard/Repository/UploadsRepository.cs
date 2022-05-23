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
    public class UploadsRepository
    {
        private SqlConnection con;
        
        //To Handle connection related activities
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["conn"].ToString();
            con = new SqlConnection(constr);
        }

        //To view employee details with generic list 
        public List<UploadModel> 
            GetAllUploads()
        {
            connection();
            List<UploadModel> uploadList = new List<UploadModel>();
            SqlCommand com = new SqlCommand("spx_Email_Files_SelectAll", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            //Bind uploadList generic list using LINQ 
            uploadList = (from DataRow dr in dt.Rows
                       select new UploadModel()
                       {
                           ID = Convert.ToInt32(dr["ID"]),
                           FileNameXML = Convert.ToString(dr["FileNameXML"]),
                           RecordCount = Convert.ToInt32(dr["RecordCount"]),
                           FileSizeMB = Convert.ToString(dr["FileSizeMB"]),
                           //BatchNumber = Convert.ToInt32(dr["BatchNumber"]),
                           DateCreated = Convert.ToString(dr["DateCreated"]),
                           //DateEmailSend = Convert.ToDateTime(dr["DateEmailSend"]),
                           SendYesNo = Convert.ToBoolean(dr["SendYesNo"])
                       }).ToList();
            return uploadList;

        }

        public Dictionary<string, int> GetUploadCount(int dateRange = 6)
        {
            connection();
            SqlCommand com = new SqlCommand("spx_GetRecordCountForRange", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            
            SqlParameter paramCount = new SqlParameter();
            paramCount.ParameterName = "@Days";
            paramCount.Value = dateRange;
            com.Parameters.Add(paramCount);


            con.Open();
            da.Fill(dt);

            Dictionary<string, int> recordCount = new Dictionary<string, int>();
            DateTime currentDate = DateTime.Now;

            string dateString = string.Empty;

            while(dateRange > 0)
            {
                dateString = currentDate.AddDays(dateRange * -1).ToString("yyyy-MM-dd");
                recordCount.Add(dateString, 0);
                dateRange--;
            }            
            dateString = currentDate.ToString("yyyy-MM-dd");
            recordCount.Add(dateString, 0);

            var d = dt.AsEnumerable();

            
            foreach (string key in recordCount.Keys.ToList())
            {
                var rows = dt.Select("Date = '" + key + "'");

                if (rows.Count() > 0)
                {
                    recordCount[key] = Convert.ToInt32(rows[0]["RecordCount"]);
                }
            }
            con.Close();

            return recordCount;
        }
    }
}