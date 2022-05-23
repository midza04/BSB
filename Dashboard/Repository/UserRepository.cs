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
    public class UserRepository
    {
        private SqlConnection con;
        
        //To Handle connection related activities
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["conn"].ToString();
            con = new SqlConnection(constr);
        }

        //To view employee details with generic list 
        public List<UserModel> GetAllUsers()
        {
            connection();
            List<UserModel> userList = new List<UserModel>();
            SqlCommand com = new SqlCommand("spx_GetAllUsers", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            userList = (from DataRow dr in dt.Rows
                         select new UserModel()
                         {
                             ID = Convert.ToInt32(dr["ID"]),
                             Username = Convert.ToString(dr["Username"]),
                             IsEnabled= Convert.ToBoolean(dr["IsEnabled"]),                             
                         }).ToList();
            return userList;
        }

        public void UpdateUser(UserModel model)
        {
            try
            {
                connection();
                SqlCommand com = new SqlCommand("spx_UpdateUser", con);
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

                SqlParameter paramIsEnabled = new SqlParameter();
                paramIsEnabled.ParameterName = "@IsEnabled";
                paramIsEnabled.Value = model.IsEnabled;
                com.Parameters.Add(paramIsEnabled);

                com.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }

        }

        public void InsertUser(UserModel model)
        {
            connection();
            SqlCommand com = new SqlCommand("spx_InsertUser", con);
            com.CommandType = CommandType.StoredProcedure;
            con.Open();

            SqlParameter paramUsername = new SqlParameter();
            paramUsername.ParameterName = "@Username";
            paramUsername.Value = model.Username;
            com.Parameters.Add(paramUsername);

            SqlParameter paramIsEnabled = new SqlParameter();
            paramIsEnabled.ParameterName = "@IsEnabled";
            paramIsEnabled.Value = model.IsEnabled;
            com.Parameters.Add(paramIsEnabled);

            com.ExecuteNonQuery();

            con.Close();

        }

        public void DeleteEmailRecipient(int ID)
        {
            connection();
            SqlCommand com = new SqlCommand("spx_DeleteUser", con);
            com.CommandType = CommandType.StoredProcedure;
            con.Open();

            SqlParameter paramID = new SqlParameter();
            paramID.ParameterName = "@Id";
            paramID.Value = ID;
            com.Parameters.Add(paramID);

            com.ExecuteNonQuery();

            con.Close();
        }

        public UserModel GetUser(int Id) 
        {
            connection();
            UserModel userModel;
            SqlCommand com = new SqlCommand("spx_GetUser", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlParameter paramID = new SqlParameter();
            paramID.ParameterName = "@Id";
            paramID.Value = Id;

            com.Parameters.Add(paramID);
            con.Open();

            using (var reader = com.ExecuteReader())
            {
                reader.Read();

                userModel = new UserModel
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    Username = Convert.ToString(reader["Username"])              
                };
            }

            con.Close();

            return userModel;
        }

        public UserModel GetUserByName(string name)
        {
            connection();
            UserModel userModel = null;
            SqlCommand com = new SqlCommand("spx_GetUserByName", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlParameter paramID = new SqlParameter();
            paramID.ParameterName = "@Name";
            paramID.Value = name;

            com.Parameters.Add(paramID);
            con.Open();

            using (var reader = com.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();

                    userModel = new UserModel
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        Username = Convert.ToString(reader["Username"])
                    }; 
                }
            }

            con.Close();

            return userModel;
        }
    }
}