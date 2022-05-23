using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFFintech.goAML
{
    public class Datalayer : IDisposable
    {
        #region Dispose

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
            this.con.Dispose();
            this.cmdselect.Dispose();
            this.da.Dispose();
        }

        protected virtual void Dispose(Boolean freeManagedObjectsAlso)
        {
            this.con.Dispose();
            this.cmdselect.Dispose();
            this.da.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Connection objects

        readonly String strCon = ConfigurationSettings.AppSettings.Get("StrConn");

        SqlCommand cmdselect;
        SqlConnection con;
        SqlDataAdapter da;

        #endregion

        #region "Loans Charts and Graphs"

        public DataTable Report_GetCount_Loans_Between_Dates(DateTime dateEnteredFrom, DateTime dateEnteredTo)
        {
            this.con = new SqlConnection(this.strCon);
            this.cmdselect = new SqlCommand();
            this.cmdselect.CommandText = "sp_Report_GetCount_Loans_Between_Dates";
            cmdselect.Parameters.Add("@parDateEnteredFrom", SqlDbType.DateTime).Value = dateEnteredFrom;
            cmdselect.Parameters.Add("@parDateEnteredTo", SqlDbType.DateTime).Value = dateEnteredTo;

            this.cmdselect.CommandTimeout = 0;
            this.cmdselect.CommandType = CommandType.StoredProcedure;
            this.cmdselect.Connection = this.con;
            DataTable dt = new DataTable();

            this.da = new SqlDataAdapter();
            this.da.SelectCommand = this.cmdselect;

            try
            {
                this.con.Open();
                this.da.Fill(dt);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                this.con.Close();
            }
            return dt;
        }

        public DataTable GetDropdownListOfReports()
        {
            this.con = new SqlConnection(this.strCon);
            this.cmdselect = new SqlCommand();
            this.cmdselect.CommandText = "spx_GetDropdown_List_of_Reports";
            this.cmdselect.CommandTimeout = 0;
            this.cmdselect.CommandType = CommandType.StoredProcedure;
            this.cmdselect.Connection = this.con;
            DataTable dt = new DataTable();

            this.da = new SqlDataAdapter();
            this.da.SelectCommand = this.cmdselect;

            try
            {
                this.con.Open();
                this.da.Fill(dt);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                this.con.Close();
            }
            return dt;
        }

        #endregion
        #region Error Logging
        public void ErrorAddlogdata(string errorDescription, string errorModule, Guid errorUserID)
        {
            this.con = new SqlConnection(this.strCon);
            this.cmdselect = new SqlCommand();
            this.cmdselect.CommandText = "ERROR_ADDLOGDATA";
            this.cmdselect.CommandTimeout = 0;
            this.cmdselect.CommandType = CommandType.StoredProcedure;
            this.cmdselect.Parameters.Add("@ERROR_DESCRIPTION", SqlDbType.VarChar).Value = errorDescription;
            this.cmdselect.Parameters.Add("@ERROR_MODULE", SqlDbType.VarChar).Value = errorModule;
            this.cmdselect.Parameters.Add("@ERROR_USER_ID", SqlDbType.UniqueIdentifier).Value = errorUserID;
            this.cmdselect.Connection = this.con;

            try
            {
                this.con.Open();
                this.cmdselect.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {

                throw ex;

            }
            finally
            {
                this.con.Close();
            }
        }


        public DataTable ERROR_GET_ERROR_LOG(Guid ERROR_USER_ID)
        {
            this.con = new SqlConnection(this.strCon);
            this.cmdselect = new SqlCommand();
            this.cmdselect.CommandText = "ERROR_GET_ERROR_LOG";
            this.cmdselect.CommandTimeout = 0;
            this.cmdselect.CommandType = CommandType.StoredProcedure;
            this.cmdselect.Parameters.Add("@ERROR_USER_ID", SqlDbType.UniqueIdentifier).Value = ERROR_USER_ID;
            this.cmdselect.Connection = this.con;
            DataTable dt = new DataTable();

            this.da = new SqlDataAdapter();
            this.da.SelectCommand = this.cmdselect;

            try
            {
                this.con.Open();
                this.da.Fill(dt);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                this.con.Close();
            }
            return dt;
        }

        public void LogError(string STEP_DESCR, string LOGGER_DESCR)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings.Get("StrConn"));
            SqlCommand cmdselect = new SqlCommand();
            cmdselect.CommandText = "spx_logger";
            cmdselect.CommandType = CommandType.StoredProcedure;
            cmdselect.Connection = con;
            cmdselect.Parameters.Add("@STEP_DESCR", SqlDbType.VarChar).Value = STEP_DESCR;
            cmdselect.Parameters.Add("@LOGGER_DESCR", SqlDbType.VarChar).Value = LOGGER_DESCR;

            try
            {
                con.Open();
                cmdselect.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {

                throw ex;

            }
            finally
            {
                con.Close();
            }

        }

        public void LogStep(string varStepNumber, string varStepDescription)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings.Get("StrConn"));

            cmdselect = new SqlCommand();
            cmdselect.CommandText = "spx_logger";
            cmdselect.CommandTimeout = 0;
            cmdselect.CommandType = CommandType.StoredProcedure;
            cmdselect.Connection = con;

            cmdselect.Parameters.Add(new SqlParameter("@STEP_DESCR", varStepNumber));
            cmdselect.Parameters.Add(new SqlParameter("@LOGGER_DESCR", varStepDescription));


            try
            {
                con.Open();
                cmdselect.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {

                throw ex;

            }
            finally
            {
                con.Close();
            }
        }

        public void Log_GET_CASH_Deposit_Skipped_Records(string var_TRN_REF_NO, string var_Error_Why_Skipped)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings.Get("StrConn"));

            cmdselect = new SqlCommand();
            cmdselect.CommandText = "spx_GET_CASH_Deposit_Skipped_Records_Insert_One";
            cmdselect.CommandTimeout = 0;
            cmdselect.CommandType = CommandType.StoredProcedure;
            cmdselect.Connection = con;

            cmdselect.Parameters.Add(new SqlParameter("@TRN_REF_NO", var_TRN_REF_NO));
            cmdselect.Parameters.Add(new SqlParameter("@Error_Why_Skipped", var_Error_Why_Skipped));

            try
            {
                con.Open();
                cmdselect.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {

                throw ex;

            }
            finally
            {
                con.Close();
            }
        }


        #endregion


        #region #Verify File

        public void SettlementTransaction_Insert_One(Int64 batchNumber, string CheckNumber, string CustomerName, string DeductionCode, decimal InstallmentAmountTreasury, decimal OutstandingAmount, string IP_Hostname, string Username)
        {
            con = new SqlConnection(this.strCon);
            SqlCommand cmdselect = new SqlCommand();
            cmdselect.CommandType = System.Data.CommandType.StoredProcedure;
            cmdselect.CommandTimeout = 0;
            cmdselect.Connection = con;
            cmdselect.CommandText = "spx_SettlementTransaction_Insert_One";
            cmdselect.Parameters.Add("@BatchNumber", SqlDbType.BigInt).Value = batchNumber;
            cmdselect.Parameters.Add("@CheckNumber", SqlDbType.VarChar).Value = CheckNumber;
            cmdselect.Parameters.Add("@CustomerName", SqlDbType.VarChar).Value = CustomerName;
            cmdselect.Parameters.Add("@DeductionCode", SqlDbType.VarChar).Value = DeductionCode;
            cmdselect.Parameters.Add("@InstallmentAmountTreasury", SqlDbType.Decimal).Value = InstallmentAmountTreasury;
            cmdselect.Parameters.Add("@OutstandingAmount", SqlDbType.Decimal).Value = OutstandingAmount;
            cmdselect.Parameters.Add("@IP_Hostname", SqlDbType.VarChar).Value = IP_Hostname;
            cmdselect.Parameters.Add("@Username", SqlDbType.VarChar).Value = Username;

            try
            {
                con.Open();
                cmdselect.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        public DataTable GET_ReportHeader_Dynamic()
        {
            con = new SqlConnection(this.strCon);
            SqlCommand cmdselect = new SqlCommand();
            cmdselect.CommandType = System.Data.CommandType.StoredProcedure;
            cmdselect.CommandTimeout = 0;
            cmdselect.Connection = con;
            cmdselect.CommandText = "spx_Get_ReportHeader_Dynamic";
            DataTable dt = new DataTable();
            da = new SqlDataAdapter();
            da.SelectCommand = cmdselect;


            try
            {
                con.Open();
                da.Fill(dt);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return dt;

        }
        public DataTable GET_CASH_Deposit()
        {
            con = new SqlConnection(this.strCon);
            SqlCommand cmdselect = new SqlCommand();
            cmdselect.CommandType = System.Data.CommandType.StoredProcedure;
            cmdselect.CommandTimeout = 0;
            cmdselect.Connection = con;
            cmdselect.CommandText = "spx_GET_CASH_Deposit";
            DataTable dt = new DataTable();
            da = new SqlDataAdapter();
            da.SelectCommand = cmdselect;


            try
            {
                con.Open();
                da.Fill(dt);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return dt;

        }

        public void SettlementTransaction_Update_One_FCUBS_Post(Int64 parID, string parFCUBSProcessingStatus)
        {

            this.con = new SqlConnection(this.strCon);
            this.cmdselect = new SqlCommand();
            this.cmdselect.CommandText = "spx_SettlementTransaction_Update_One_FCUBS_Post";
            this.cmdselect.CommandTimeout = 0;
            this.cmdselect.CommandType = CommandType.StoredProcedure;
            cmdselect.Parameters.Add("@parID", SqlDbType.BigInt).Value = Convert.ToInt64(parID);
            cmdselect.Parameters.Add("@parFCUBSProcessingStatus", SqlDbType.VarChar).Value = "Picked for FCUBS Posting";
            this.cmdselect.Connection = this.con;

            try
            {

                con.Open();
                cmdselect.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {

                ErrorAddlogdata(ex.ToString(), " : SettlementTransaction_Update_One_FCUBS_Post", Guid.NewGuid());
                throw ex;
            }
            finally
            {
                con.Close();
            }

        }

        public void SettlementsTransaction_Header_Update_One(Int64 parBatchNumber, bool parProcessingStatus)
        {

            this.con = new SqlConnection(this.strCon);
            this.cmdselect = new SqlCommand();
            this.cmdselect.CommandText = "spx_Update_SettlementsTransaction_Header";
            this.cmdselect.CommandTimeout = 0;
            this.cmdselect.CommandType = CommandType.StoredProcedure;
            cmdselect.Parameters.Add("@parBatchNumber", SqlDbType.BigInt).Value = parBatchNumber;
            cmdselect.Parameters.Add("@parProcessingStatus", SqlDbType.Bit).Value = parProcessingStatus;
            this.cmdselect.Connection = this.con;

            try
            {

                con.Open();
                cmdselect.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {

                ErrorAddlogdata(ex.ToString(), " : SettlementTransaction_Update_One_FCUBS_Post", Guid.NewGuid());
                throw ex;
            }
            finally
            {
                con.Close();
            }

        }



        public void Settlement_Transactions_Update_Finished_Batch_Number_ProcessStatus_True(Int64 varBatchNumber)
        {

            this.con = new SqlConnection(this.strCon);
            this.cmdselect = new SqlCommand();
            this.cmdselect.CommandText = "spx_Settlement_Transactions_Update_Finished_Batch_Number_ProcessStatus_True";
            this.cmdselect.CommandTimeout = 0;
            this.cmdselect.CommandType = CommandType.StoredProcedure;
            cmdselect.Parameters.Add("@parBatchNumber", SqlDbType.BigInt).Value = varBatchNumber;

            this.cmdselect.Connection = this.con;

            try
            {

                con.Open();
                cmdselect.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                ErrorAddlogdata(ex.ToString(), " : Settlement_Transactions_Update_Finished_Batch_Number_ProcessStatus_True", Guid.NewGuid());
            }
            finally
            {
                con.Close();
            }

        }


        public void SettlementTransaction_Update_Whole_Batch_FCUBS_Post(Int64 parID, string parFCUBS_Reference_Number,
            string parFCUBSProcessingStatus, string parFCUBS_Feed_Back_Message, string parMaker_ID, string parPosting_Hostname,
            string IP_Hostname, string DebitAccountNumber)
        {

            this.con = new SqlConnection(this.strCon);
            this.cmdselect = new SqlCommand();
            this.cmdselect.CommandText = "spx_SettlementTransaction_Update_Whole_Batch_FCUBS_Post";
            this.cmdselect.CommandTimeout = 0;
            this.cmdselect.CommandType = CommandType.StoredProcedure;
            cmdselect.Parameters.Add("@parID", SqlDbType.BigInt).Value = parID;
            cmdselect.Parameters.Add("@parFCUBS_Reference_Number", SqlDbType.VarChar).Value = parFCUBS_Reference_Number;
            cmdselect.Parameters.Add("@parFCUBSProcessingStatus", SqlDbType.VarChar).Value = parFCUBSProcessingStatus;
            cmdselect.Parameters.Add("@parFCUBS_Feed_Back_Message", SqlDbType.VarChar).Value = parFCUBS_Feed_Back_Message;
            cmdselect.Parameters.Add("@parMaker_ID", SqlDbType.VarChar).Value = parMaker_ID;
            cmdselect.Parameters.Add("@parPosting_Hostname", SqlDbType.VarChar).Value = parPosting_Hostname;
            cmdselect.Parameters.Add("@parDebitAccountNumber", SqlDbType.VarChar).Value = DebitAccountNumber;

            this.cmdselect.Connection = this.con;

            try
            {

                con.Open();
                cmdselect.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }

        }

        public DataTable Get_Settlement_Verification_Failed_Records(Int64 batchNumber)
        {

            this.con = new SqlConnection(this.strCon);
            this.cmdselect = new SqlCommand();
            this.cmdselect.CommandText = "spx_Get_Settlement_Verification_Failed_Records";
            this.cmdselect.CommandTimeout = 0;
            this.cmdselect.CommandType = CommandType.StoredProcedure;
            cmdselect.Parameters.Add(new SqlParameter("@batchNumber", batchNumber));
            DataTable dt = new DataTable();
            this.cmdselect.Connection = con;
            da = new SqlDataAdapter();
            da.SelectCommand = cmdselect;


            try
            {
                this.con.Open();
                this.da.Fill(dt);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                this.con.Close();
            }
            return dt;
        }



        public DataTable GetFailedSettlements(Int64 batchNumber)
        {
            this.con = new SqlConnection(this.strCon);
            this.cmdselect = new SqlCommand();
            this.cmdselect.CommandText = "spx_Get_Settlement_Verification_Failed_Records";
            this.cmdselect.CommandTimeout = 0;
            this.cmdselect.CommandType = CommandType.StoredProcedure;
            this.cmdselect.Parameters.Add(new SqlParameter("@batchNumber", batchNumber));
            DataTable dt = new DataTable();
            this.cmdselect.Connection = con;
            da = new SqlDataAdapter();
            da.SelectCommand = cmdselect;


            try
            {
                this.con.Open();
                this.da.Fill(dt);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                this.con.Close();
            }
            return dt;
        }


        public DataTable GET_SettlementsTransaction_Header()
        {
            this.con = new SqlConnection(this.strCon);
            this.cmdselect = new SqlCommand();
            this.cmdselect.CommandText = "spx_GET_CASH_Deposit";
            this.cmdselect.CommandTimeout = 0;
            this.cmdselect.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            this.cmdselect.Connection = con;
            da = new SqlDataAdapter();
            da.SelectCommand = cmdselect;
            try
            {
                this.con.Open();
                this.da.Fill(dt);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                this.con.Close();
            }
            return dt;
        }





        public DataTable GetVerificationRecordWithErrors(Int64 batchNumber)
        {
            this.con = new SqlConnection(this.strCon);
            this.cmdselect = new SqlCommand();
            this.cmdselect.CommandText = "spx_Get_Settlement_Verification_Error_Records";
            this.cmdselect.CommandTimeout = 0;
            this.cmdselect.CommandType = CommandType.StoredProcedure;
            this.cmdselect.Parameters.Add(new SqlParameter("@BatchNumber", batchNumber));
            DataTable dt = new DataTable();
            this.cmdselect.Connection = con;
            da = new SqlDataAdapter();
            da.SelectCommand = cmdselect;

            try
            {
                this.con.Open();
                this.da.Fill(dt);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                this.con.Close();
            }
            return dt;
        }


        public DataTable Get_Max_Settlement_Transactions_Batch_Number()
        {
            this.con = new SqlConnection(this.strCon);
            this.cmdselect = new SqlCommand();
            this.cmdselect.CommandText = "spx_Get_Max_Settlement_Transactions_Batch_Number";
            this.cmdselect.CommandTimeout = 0;
            this.cmdselect.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            this.cmdselect.Connection = con;
            da = new SqlDataAdapter();
            da.SelectCommand = cmdselect;


            try
            {
                this.con.Open();
                this.da.Fill(dt);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                this.con.Close();
            }
            return dt;
        }


        public DataTable GetVerificationResults(Int64 batchNumber)
        {
            this.con = new SqlConnection(this.strCon);
            this.cmdselect = new SqlCommand();
            this.cmdselect.CommandText = "spx_Get_Settlement_Verification_Results";
            this.cmdselect.CommandTimeout = 0;
            this.cmdselect.CommandType = CommandType.StoredProcedure;
            cmdselect.Parameters.Add(new SqlParameter("@batchNumber", batchNumber));
            DataTable dt = new DataTable();
            this.cmdselect.Connection = con;
            da = new SqlDataAdapter();
            da.SelectCommand = cmdselect;

            try
            {
                this.con.Open();
                this.da.Fill(dt);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                this.con.Close();
            }
            return dt;
        }

        #endregion

    }

}
