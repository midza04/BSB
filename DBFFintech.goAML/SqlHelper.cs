using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Configuration;
using System.Globalization; 
using Microsoft.VisualBasic;

namespace DBFFintech.goAML
{
    public static class SqlHelper
    {
        public static string subLoanStatusUpdate(string varLoanStatus, string varLoanID, string parLastUpdatedBy, string varComments = null)
        {

            SqlConnection con = new SqlConnection();
            Int64 varLoanIDInt = Convert.ToInt64(varLoanID);
            string parLastUpdatedByNew = parLastUpdatedBy;

            try
            {

                con.ConnectionString = ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString;

                SqlCommand cd = new SqlCommand();
                cd.CommandType = System.Data.CommandType.StoredProcedure;
                cd.Connection = con;

                cd.CommandType = System.Data.CommandType.StoredProcedure;
                cd.Connection = con;

                cd.CommandText = "procLoanUpdateStatusOne";

                cd.Parameters.Add(new SqlParameter("@parLoansID", varLoanIDInt));
                cd.Parameters.Add(new SqlParameter("@parLoanStatus", varLoanStatus));
                cd.Parameters.Add(new SqlParameter("@parLastUpdatedBy", parLastUpdatedByNew));
                cd.Parameters.Add(new SqlParameter("@parReason", "StatusUpdate"));
                cd.Parameters.Add(new SqlParameter("@parComments", varComments));


                if (con != null && con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cd.ExecuteNonQuery();
                return null;
            }
            catch (Exception ex)
            { 
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
        }


        public static string Get_Today_Date_Back_To_CSharp()
        {
            string varToday_Date = ""; // Get ToDay's Date
            SqlConnection conE = new SqlConnection();
            try
            {

                conE.ConnectionString = ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString;

                SqlCommand cd = new SqlCommand();
                cd.CommandType = System.Data.CommandType.StoredProcedure;
                cd.Connection = conE;

                cd.CommandText = "proc_Get_Today_Date_Back_To_CSharp";

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cd;
                DataTable dt = new DataTable();
                conE.Open();

                da.Fill(dt);

                varToday_Date = dt.Rows[0]["TodayDate"].ToString();

                return varToday_Date;

            }

            catch (SqlException ex)
            {
                throw ex;

            }
            finally
            {
                conE.Close();
            }
        }
        public static Int64 Get_Parameter_One_value(string parParameter_One_Name)
        {
            Int64 varParameterOne = 0;
            SqlConnection conE = new SqlConnection();
         
                conE.ConnectionString = ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString;

                SqlCommand cd = new SqlCommand();
                cd.CommandType = System.Data.CommandType.StoredProcedure;
                cd.Connection = conE;

                cd.CommandText = "procGetParameter_One_value";

                cd.Parameters.Add(new SqlParameter("@parParameter_One_Name", parParameter_One_Name));

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cd;
                DataTable dt = new DataTable();
                try
                {


                conE.Open();

                da.Fill(dt);

                varParameterOne = Convert.ToInt64(dt.Rows[0]["Parameter_One_value"]);

                return varParameterOne;

            }

            catch (SqlException ex)
            {

                throw;

            }
            finally
            {
                conE.Close();
            }
        }
        public static string funcTitleCase(string varTextFromControls)
        {

            if (varTextFromControls.Trim().Length != 0)
            {
                varTextFromControls = CultureInfo.CurrentCulture.TextInfo.ToUpper(varTextFromControls.ToUpper());
            }
            else
            {
                varTextFromControls = string.Empty;
            }

            return varTextFromControls;

        }
         
        public static void ExecuteTable(string storedProcedureName, StoredProcedureAction storedProcedureAction, DataTable dataTable, SqlParameter[] parameters, int batchSize)
        {
            string result = string.Empty;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 1000;
                    command.Parameters.AddRange(parameters);
                    command.UpdatedRowSource = UpdateRowSource.None;

                    using (SqlDataAdapter adapter = new SqlDataAdapter())
                    {
                        adapter.UpdateBatchSize = batchSize;
                        switch (storedProcedureAction)
                        {
                            case StoredProcedureAction.Delete: adapter.DeleteCommand = command; break;
                            case StoredProcedureAction.Insert: adapter.InsertCommand = command; break;
                            case StoredProcedureAction.Select: adapter.SelectCommand = command; break;
                            case StoredProcedureAction.Update: adapter.UpdateCommand = command; break;
                            default: break;
                        }

                        connection.Open();
                        int rowsAffected = adapter.Update(dataTable);
                    }
                }
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }


        public static string ExecuteScalar(string storedProcedure, SqlParameter[] parameters)
        {
            object result = null;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    command.CommandTimeout = 1000;
                    if (connection != null && connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    result = command.ExecuteScalar();
                }

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

            }
            return result == null ? null : result.ToString();
        }

        public static DataTable GetTable(string storedProcedure, SqlParameter[] parameters)
        {
            DataTable result = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    command.CommandTimeout = 1000;

                    if (connection != null && connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    SqlDataReader reader = command.ExecuteReader();
                    result.Load(reader, LoadOption.OverwriteChanges); //, LoadOption.OverwriteChanges, output);
                }

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            // Enable column edits in all columns of a data table
            foreach (DataColumn dc in result.Columns)
            {
                dc.ReadOnly = false;
                dc.AllowDBNull = true;
            }
            return result;
        }

        public static DataTable GetTable_Without_Paramteres(string storedProcedure)
        {
            DataTable result = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 1000;
                    if (connection != null && connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlDataReader reader = command.ExecuteReader();
                    result.Load(reader, LoadOption.OverwriteChanges); //, LoadOption.OverwriteChanges, output);
                }
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            // Enable column edits in all columns of a data table
            foreach (DataColumn dc in result.Columns)
            {
                dc.ReadOnly = false;
                dc.AllowDBNull = true;
            }
            return result;
        }



        public static DataSet GetDataSet(string storedProcedure, SqlParameter[] parameters, string[] output)
        {
            DataSet result = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    command.CommandTimeout = 1000;
                    if (connection != null && connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlDataReader reader = command.ExecuteReader();
                    result.Load(reader, LoadOption.OverwriteChanges, output);
                }

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            // Enable column edits in all columns of a data table
            foreach (DataTable dt in result.Tables)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    dc.ReadOnly = false;
                    dc.AllowDBNull = true;
                }
            }

            return result;
        }


        public static DataSet GetResultset(string storedProcedure, SqlParameter[] parameters, string[] output)
        {
            DataSet result = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    command.CommandTimeout = 1000;

                    if (connection != null && connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlDataReader reader = command.ExecuteReader();
                    result.Load(reader, LoadOption.OverwriteChanges, output);
                }
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            // Enable column edits in all columns of a data table
            foreach (DataTable dt in result.Tables)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    dc.ReadOnly = false;
                    dc.AllowDBNull = true;
                }
            }

            return result;
        }

        public static string GetCurrentBranche()
        {
             return "201"; // PROD Branch In TZ
           // return "203"; // UAT Branch In TZ
        }

        public static int RunSql(string sqlText)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sqlText, connection))
                {
                    cmd.CommandTimeout = 1000;


                    if (connection != null && connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    
                    rowsAffected = cmd.ExecuteNonQuery();
                }
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return rowsAffected;
        }

        public static int RunSql_Check_TOP_One(string sqlText)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sqlText, connection))
                {
                    cmd.CommandTimeout = 1000;

                    if (connection != null && connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        rowsAffected = dt.Rows.Count;

                    }
                    else
                    {
                        rowsAffected = 0;
                    }
                }
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return rowsAffected;
        }

        public static DataTable RunSql_Check_For_Insert_And_For_Update(string sqlText)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sqlText, connection))
                {
                    cmd.CommandTimeout = 1000;

                    if (connection != null && connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    return dt;
                }

            }

        }



        public static int RunSql(string connectionString, string sqlText)
        {
            int rowsAffected = 0;

            string dbConnection = string.IsNullOrEmpty(connectionString) ? ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString : connectionString;

            using (SqlConnection connection = new SqlConnection(dbConnection))
            {
                using (SqlCommand cmd = new SqlCommand(sqlText, connection))
                {
                    cmd.CommandTimeout = 1000;
                    if (connection != null && connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    rowsAffected = cmd.ExecuteNonQuery();
                }
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return rowsAffected;
        }


        public static string RunSqlScalar(string sqlText)
        {
            string result = "";

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sqlText, connection))
                {
                    cmd.CommandTimeout = 1000;
                    if (connection != null && connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    result = cmd.ExecuteScalar().ToString();
                }
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return result;
        }


        public static string CheckDBConnection(string sqlText)
        {
            object result = string.Empty;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sqlText, connection))
                {
                    cmd.CommandTimeout = 250;
                    if (connection != null && connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    result = cmd.ExecuteScalar();
                }
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return result == null ? string.Empty : result.ToString();
        }


        public static string ExecuteScalar(string sqlText)
        {
            object result = string.Empty;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sqlText, connection))
                {
                    cmd.CommandTimeout = 1000;
                    if (connection != null && connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    result = cmd.ExecuteScalar();
                }
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return result == null ? string.Empty : result.ToString();
        }


        public static string RunSqlScalar(string connectionString, string sqlText)
        {
            string result = "";

            string dbConnection = string.IsNullOrEmpty(connectionString) ? ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString : connectionString;

            using (SqlConnection connection = new SqlConnection(dbConnection))
            {
                using (SqlCommand cmd = new SqlCommand(sqlText, connection))
                {
                    cmd.CommandTimeout = 1000;
                    if (connection != null && connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    object o = cmd.ExecuteScalar();
                    if (o != null) result = o.ToString();
                }
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return result;
        }



        public static DataTable GetTable(string sqlText)
        {
            DataTable result = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlText, connection))
                {
                    command.CommandTimeout = 1000;
                    if (connection != null && connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlDataReader reader = command.ExecuteReader();
                    result.Load(reader, LoadOption.OverwriteChanges); //, LoadOption.OverwriteChanges, output);
                }
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            // Enable column edits in all columns of a data table
            foreach (DataColumn dc in result.Columns)
            {
                dc.ReadOnly = false;
                dc.AllowDBNull = true;
            }
            return result;
        }


        public static DataTable GetTable(string connectionString, string sqlText)
        {
            DataTable dt = new DataTable();

            string dbConnection = string.IsNullOrEmpty(connectionString) ? ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString : connectionString;
            using (SqlConnection connection = new SqlConnection(dbConnection))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlText, connection))
                {
                    if (connection != null && connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    adapter.FillSchema(dt, SchemaType.Source);
                    adapter.Fill(dt);
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    return dt;
                }
            }
        }


        public static SqlDataReader ExecuteReader(string sqlText)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sqlText, connection);
            cmd.CommandTimeout = 1000;
            if (connection != null && connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        public static void LogError_For_Valiation(string error, string varStepDesc = null)
        {
            SqlConnection conErr = new SqlConnection();
            try
            {

                conErr.ConnectionString = ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString;
                if (conErr != null && conErr.State == ConnectionState.Closed)
                {
                    conErr.Open();
                }

                SqlCommand cd = new SqlCommand();
                cd.CommandType = System.Data.CommandType.StoredProcedure;
                cd.Connection = conErr;

                cd.CommandText = "spx_logerror";

                //cd.Parameters.Add(new SqlParameter("@Error_decription", Session["FullName"].ToString().Trim() + " : " + error));

                cd.Parameters.Add(new SqlParameter("@Error_decription", error));
                cd.Parameters.Add(new SqlParameter("@STEP_DESCR", varStepDesc));

                int recordCount = cd.ExecuteNonQuery();


            }
            catch
            {
            }
            finally
            {
                try
                {
                    if (conErr.State == ConnectionState.Open)
                    {
                        conErr.Close();
                    }
                }
                catch { }
            }
        }
         

    
     
        public static string varFormatDate_To_DD_MM_YYYY(string varDateFromTextBox)
        {

            //  YYYY_MM_DD from SQL Server function
            // 1971-02-02 00:00:00.000
            string varDateYear = varDateFromTextBox.Substring(0, 4);
            string varDateMonth = varDateFromTextBox.Substring(5, 2);
            string varDateDay = varDateFromTextBox.Substring(8, 2);

            return varDateDay + "-" + varDateMonth + "-" + varDateYear;

        }

        public static string varFormatDate_To_YYYY_MM_DD(string varDateFromTextBox)
        {

            string varDateYear = varDateFromTextBox.Substring(6, 4);
            string varDateMonth = varDateFromTextBox.Substring(3, 2);
            string varDateDay = varDateFromTextBox.Substring(0, 2);

            return varDateYear + "-" + varDateMonth + "-" + varDateDay;

        }

        public static string varFormatDate_To_YYYY_MM_DDTHH_MM_SS(string varDateFromTextBox)
        {
            // 06-09-2015 00:00:00.0000000
            //1983-02-23T00:00:00
            // 0123456789012345678
            //15/08/2016 7:15:57 PM

            string varDateOK;
            if (varDateFromTextBox.Length == 0)
            {
                //varDateOK = "06-09-1970 00:00:00.0000000";
                varDateOK = "1970-02-23T00:00:00";
            }
            else
            { 
            string varTime = varDateFromTextBox.Substring(11, 8);
            string varDateYear = varDateFromTextBox.Substring(6, 4);
            string varDateMonth = varDateFromTextBox.Substring(3, 2);
            string varDateDay = varDateFromTextBox.Substring(0, 2);


            if(varTime.Trim().Length != 8) varTime = "0" + varTime;
            if (varDateMonth.Trim().Length != 2) varDateMonth = "0" + varDateMonth;
            if (varDateDay.Trim().Length != 2) varDateDay = "0" + varDateDay;
            
                varDateOK = varDateYear.Trim() + "-" + varDateMonth.Trim() + "-" + varDateDay.Trim() + "T" + varTime.Trim();
 
            }
            
            return varDateOK;

        }


        public static string varFormatDate_To_Month_Period_MM(string varDateFromTextBox)
        {

            string varDateMonth = "M" + varDateFromTextBox.Substring(3, 2);

            return varDateMonth;

        }

        public static string varFormatDate_To_Year_YYYY(string varDateFromTextBox)
        {

            string varDateYear = "FY" + varDateFromTextBox.Substring(6, 4);

            return varDateYear;

        }


        public static void LogError_No_Pop_Up_Msg(string varError_decription, string varstep_descrption)
        {
            SqlConnection conErr = new SqlConnection();
            try
            {

                conErr.ConnectionString = ConfigurationManager.ConnectionStrings["StrConn"].ConnectionString;


                SqlCommand cd = new SqlCommand();
                cd.CommandType = System.Data.CommandType.StoredProcedure;
                cd.Connection = conErr;

                cd.CommandText = "spx_logerror";

                cd.Parameters.Add(new SqlParameter("@Error_decription", varError_decription));
                cd.Parameters.Add(new SqlParameter("@STEP_DESCR", varstep_descrption));

                if (conErr != null && conErr.State == ConnectionState.Closed)
                {
                    conErr.Open();
                }
                int recordCount = cd.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                //LogError_No_Pop_Up_Msg(ex.Message.ToString(), "LogError_No_Pop_Up_Msg sub routine in LoanTracker");
            }
            finally
            {
                try
                {
                    if (conErr.State == ConnectionState.Open)
                    {
                        conErr.Close();
                    }
                }
                catch { }
            }
        }




        //public static  SqlDataReader GetReader(string connectionString, string sqlText)
        //{
        //    SqlDataReader reader = null;

        //    string dbConnection = string.IsNullOrEmpty(connectionString) ? ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString : connectionString;

        //    SqlConnection connection = new SqlConnection(dbConnection);

        //    SqlCommand cmd = new SqlCommand(sqlText, connection);

        //    cmd.CommandTimeout = 1000;
        //    connection.Open();
        //    reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        //    return reader;
        //}


        //public static  DataSet GetMultipleResults(string sqlText, string[] tableNames)
        //{
        //    DataSet ds = new DataSet();
        //    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand(sqlText, connection))
        //        {
        //            connection.Open();
        //            cmd.CommandTimeout = 1000;
        //            SqlDataReader reader = cmd.ExecuteReader();
        //            ds.Load(reader, LoadOption.OverwriteChanges, tableNames);
        //        }
        //        connection.Close();
        //    }
        //    return ds;
        //}


        //public static  DataSet GetMultipleResults(string connectionString, string sqlText, string[] tableNames)
        //{
        //    DataSet ds = new DataSet();
        //    string dbConnection = String.IsNullOrEmpty(connectionString) ? ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString : connectionString;
        //    using (SqlConnection connection = new SqlConnection(dbConnection))
        //    {
        //        using (SqlCommand cmd = new SqlCommand(sqlText, connection))
        //        {
        //            connection.Open();
        //            cmd.CommandTimeout = 1000;
        //            SqlDataReader reader = cmd.ExecuteReader();
        //            ds.Load(reader, LoadOption.OverwriteChanges, tableNames);
        //        }
        //        connection.Close();
        //    }
        //    return ds;
        //}


        //public static  SqlDataReader DataReader(string sqlText)
        //{
        //    SqlDataReader reader = null;

        //    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand(sqlText, connection))
        //        {
        //            connection.Open();
        //            cmd.CommandTimeout = 1000;
        //            reader = cmd.ExecuteReader();
        //        }
        //        connection.Close();
        //    }
        //    return reader;
        //}


        //public static  DataSet GetDataSet(string sqlText, string tableName)
        //{
        //    DataSet ds = new DataSet();
        //    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString))
        //    {
        //        using (SqlDataAdapter adapter = new SqlDataAdapter(sqlText, connection))
        //        {
        //            connection.Open();
        //            adapter.Fill(ds, tableName);
        //        }
        //        connection.Close();
        //    }
        //    return ds;
        //}


        //public static  DataSet GetDataSet(string connectionString, string sqlText, string tableName)
        //{
        //    DataSet ds = new DataSet();
        //    string dbConnection = String.IsNullOrEmpty(connectionString) ? ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString : connectionString;

        //    using (SqlConnection connection = new SqlConnection(dbConnection))
        //    {
        //        using (SqlDataAdapter adapter = new SqlDataAdapter(sqlText, connection))
        //        {
        //            connection.Open();
        //            adapter.Fill(ds, tableName);
        //        }
        //        connection.Close();
        //    }
        //    return ds;
        //}


        //public static  string ExecuteStoredProcedure(string storedProcedureName, SqlParameter[] parameters)
        //{
        //    string result = string.Empty;
        //    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString))
        //    {
        //        using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.CommandTimeout = 1000;
        //            command.Parameters.AddRange(parameters);
        //            connection.Open();
        //            result = command.ExecuteScalar().ToString();
        //        }
        //        connection.Close();
        //    }
        //    return result;
        //}


        //public static  void ExecuteStoredProcedure(string storedProcedureName, StoredProcedureAction storedProcedureAction, DataTable dataTable, SqlParameter[] parameters, int batchSize)
        //{
        //    string result = string.Empty;
        //    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LoanTrackerDBConnection"].ConnectionString))
        //    {
        //        using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.CommandTimeout = 1000;
        //            command.Parameters.AddRange(parameters);
        //            command.UpdatedRowSource = UpdateRowSource.None;

        //            using (SqlDataAdapter adapter = new SqlDataAdapter())
        //            {
        //                adapter.UpdateBatchSize = batchSize;
        //                switch (storedProcedureAction)
        //                {
        //                    case StoredProcedureAction.Delete: adapter.DeleteCommand = command; break;
        //                    case StoredProcedureAction.Insert: adapter.InsertCommand = command; break;
        //                    case StoredProcedureAction.Select: adapter.SelectCommand = command; break;
        //                    case StoredProcedureAction.Update: adapter.UpdateCommand = command; break;
        //                    default: break;
        //                }

        //                connection.Open();
        //                int rowsAffected = adapter.Update(dataTable);
        //            }
        //        }
        //        connection.Close();
        //    }
        //}

        public enum StoredProcedureAction
        {
            Delete = 1,
            Insert = 2,
            Select = 3,
            Update = 4
        }
    }
}