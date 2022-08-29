using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Windows.Forms;

namespace DBFfintech_Interface_Portal
{


    public static class GenericMethods
    {



        public static int UserID;
        //This will store the Ratings to the Catche when there is no Network Connections
        //public static List<DataLayer.RatingModel> CatchedRatings = new List<DataLayer.RatingModel>(); 
        #region "Common Functions"
        public static bool BooleanConveter(string val)
        {
            if (val.TrimStart().TrimEnd() == "Active")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool BooleanConveterNumber(string val)
        {
            if (val.TrimStart().TrimEnd() == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static object Isnull(object value)
        {
            if (value.ToString() == string.Empty || value == null)
            {
                return value = 0;
            }
            else
            {
                return value;
            }
        }

        public static object IsnullString(string value)
        {
            if (value == string.Empty || value == null || value == "&nbsp;")
            {
                return value = string.Empty;
            }
            else
            {
                return value;
            }
        }

        public static object IsnullString(object value)
        {
            if (value.ToString() == string.Empty || value == null)
            {
                return value = string.Empty;
            }
            else
            {
                return value;
            }
        }

        public static string IsnullString_Return_int(object value)
        {
            if (value.ToString() == string.Empty || value == null)
            {
                return "0";
            }
            else
            {
                return value.ToString();
            }
        }


        public static object IsnullDate(object value)
        {
            if (value.ToString() == string.Empty || value == null)
            {
                return value = DateTime.Now;
            }
            else
            {
                return value;
            }
        }
        public static int Isnull_Integer(object value)
        {
            if (value.ToString() == string.Empty || value == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(value);
            }
        }
        public static decimal Isnull_Decimal(object value)
        {
            if (value.ToString() == string.Empty || value == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(value);
            }
        }

        public static bool IsNumber(string value)
        {
            double num;
            bool isNum = double.TryParse(value.Replace("-", string.Empty).TrimStart(), out num);

            if (isNum)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public static object CheckForSpaces(string value)
        {
            return value.TrimStart().TrimEnd().Trim();
        }



        public static object CheckNegative(object value)
        {
            if (value.ToString().Substring(0, 1) == "-")
            {
                decimal val = Convert.ToDecimal(value.ToString().Replace("-", "").Trim());

                return value = val - val;
            }
            else
            {
                return value;
            }
        }
        #endregion
        #region "General"
        public static bool CheckForInternetConnection()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                // no network connection
                return false;
            }
            else
            {
                return true;
            }




        }
        public static string GetServerNameAndIPAddress()
        {
            //Provides information and the means that are used to manipulate the current environment and platform
            string MachineName = System.Environment.MachineName;
            //Returns the IPAddress of the computer
            string MachineIP = GetComputer_LanIP();
            //Formats the IPAddress and Machinename 
            string HostNameIP = String.Format("{0} - {1}", MachineIP, MachineName);

            return HostNameIP;
        }
        public static string GetComputer_LanIP()
        {
            //Set the hostname by using the built in DNS library
            string strHostName = System.Net.Dns.GetHostName();
            //Set the IPHostEntry of the HostEntry by parsing in the the hostname
            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

            //For each IPAddress in the ipEntryAddressList
            foreach (IPAddress ipAddress in ipEntry.AddressList)
            {
                //If the IPAddress Address family == to InterNetWork - then return the IPAdress as a String
                if (ipAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    return ipAddress.ToString();
                }
            }
            return "-";
        }

        public static string Isverified(string status)
        {

            if (status == "1")
            {
                return "Green";
            }
            else
            {
                return "Red";
            }




        }

        public static string ValidateMobilePhone(string Cellphone)
        {
            //1.1 Mobile Phone number  
            //1.1.1 All Mobile Phone number should be 12 Digits long

            if (Cellphone.Length == 12)
            {
                //1.1.2 First 3 Digits should be prefixed with 255
                if (Cellphone.Substring(0, 3) == "255")
                {
                    //1 2 3 4 5 6 7 8 9 10 11 12
                    //1.1.3 From 4th Digit onwards it should have 9 digits only
                    if (Cellphone.Substring(3, 9).Length == 9)
                    {
                        return "Valid";
                    }
                    else
                    {
                        return "From 4th Digit onwards it should have 9 digits only";
                    }
                }
                else
                {
                    return "First 3 Digits should be prefixed with 255";
                }
            }
            else
            {
                return "All Mobile Phone number should be 12 Digits long";

            }
        }

        public static string ValidateCheckNumber(string CheckNumber, string SystemSettingsMinVal, string SystemSettingsMaxVal)
        {
            //1.2 Employee Check number for Goverment Employee number 
            //1.2.1 Minimum to date is 7 digits and Maximum to date is 11 digits
            if (CheckNumber.Length >= Convert.ToInt32(SystemSettingsMinVal))
            {
                if (CheckNumber.Length <= Convert.ToInt32(SystemSettingsMaxVal))
                {
                    return "Valid";
                }
                else
                {
                    return "CheckNumber maximum should be 11";
                }
            }
            else
            {
                return "CheckNumber Minimum digits should be 7";
            }


        }



        #endregion

        #region Dates

        public static int GetNumberofMonths(DateTime dtstart, DateTime dtend)
        {

            // Simply subtracting Month won't work as Jan 2014 will be less than Dec 2013.
            // So first multiply the month with year to get absolute month 
            int months = (dtend.Year - dtstart.Year) * 12 + dtend.Month - dtstart.Month;
            return months;
        }


        public static bool IsDate(string Date)
        {
            DateTime dDate;

            if (DateTime.TryParse(Date, out dDate))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string SwapDateParts(string Date)
        {
            DateTime dDate;

            if (DateTime.TryParse(Date, out dDate))
            {
                return String.Format("{0:d/MM/yyyy}", dDate).ToString();
            }
            else
            {
                return "Invalid date"; // <-- Control flow goes here
            }
        }
        #endregion
        #region Error Log


        public static void LogError(string ERROR_DESCRIPTION, string ERROR_MODULE, Guid ERROR_USER_ID)
        {

            Datalayer obj = new Datalayer();

            try
            {

                obj.ErrorAddlogdata(ERROR_DESCRIPTION, ERROR_MODULE, ERROR_USER_ID);

            }
            catch (Exception ex)
            {
                obj.ErrorAddlogdata(ex.Message, ERROR_MODULE, ERROR_USER_ID);

            }
            finally
            {
                obj = null;

            }

        }
        #endregion

    }
}

