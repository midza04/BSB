using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBFfintech_Interface_Portal
{
    public class CustomerModel
    {

        private int _BancABC_Automatic_SMS_Sending_Using_CRM_ID;
        private string _Phone_Number;
        private string _DESCRIPTION;
        private string _Subject;
        private string _Campaing_Names;
        private byte _SendYesNo;

        public int BancABC_Automatic_SMS_Sending_Using_CRM_ID
        {
            get
            {
                return _BancABC_Automatic_SMS_Sending_Using_CRM_ID;
            }
            set
            {
                _BancABC_Automatic_SMS_Sending_Using_CRM_ID = value;
            }
        }


        public string Phone_Number
        {
            get
            {
                return _Phone_Number;
            }
            set
            {
                _Phone_Number = value;
            }
        }


        public string DESCRIPTION
        {
            get
            {
                return _DESCRIPTION;
            }
            set
            {
                _DESCRIPTION = value;
            }
        }


        public string Subject
        {
            get
            {
                return _Subject;
            }
            set
            {
                _Subject = value;
            }
        }


        public string Campaing_Names
        {
            get
            {
                return _Campaing_Names;
            }
            set
            {
                _Campaing_Names = value;
            }
        }


        public byte  SendYesNo
        {
            get
            {
                return _SendYesNo;
            }
            set
            {
                _SendYesNo = value;
            }
        }
    }
}
