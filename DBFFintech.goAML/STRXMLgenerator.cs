using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFFintech.goAML
{
     public class STRXMLgenerator
    {

        public static string GenerateXML(XmlGeneratorModel settings, DataTable dt)
        {
            int i = 0;
            string Trn_XML_Transaction = string.Empty;
            Datalayer dataLayer = new Datalayer();

            StringBuilder reportXML = new StringBuilder();

            string Trn_XML_String = string.Empty;

            try
            {
                reportXML.Append("<report>");

                // You get Error Reporting Person does not match reporting entinty
                // <rentity_id>{0}</rentity_id>
                // 8 = Production   https://www.fia.org.bw/goAMLcln https://168.167.134.90/goAMLcln
                // 3 = UAT = https://www.fia.org.bw/goamltrn
                // Org ID from 8 to 3
                // Check whats there now and update this line -- UAT = 3 and PROD = 8
                reportXML.AppendFormat(" <rentity_id>{0}</rentity_id>", settings.EntityId); // Pick from tblSystem Parameter To be Automated later : Sequnce created - auto - Maintain sequence number - CASH - ETF - International

                reportXML.Append("       <rentity_branch>Head Office BSB</rentity_branch>");
                reportXML.Append("       <submission_code>E</submission_code>"); // Type of submission (E - electronically/M - manually)
                reportXML.Append("       <report_code>STR</report_code>"); // Type of transaction (STR/PEP STR/CTR)
                reportXML.Append("       <entity_reference>Botswana Savings Bank</entity_reference>");
                reportXML.Append("       <fiu_ref_number>3</fiu_ref_number>");

                DateTime varSYSTEMDATE = Convert.ToDateTime(dt.Rows[i]["SYSTEMDATE"].ToString());
                string varSYSTEMDATEFormated = varSYSTEMDATE.Date.ToString("dd-MM-yyy HH:mm:ss");

                reportXML.Append("       <submission_date>" + SqlHelper.varFormatDate_To_YYYY_MM_DDTHH_MM_SS(varSYSTEMDATEFormated) + ".7205893+02:00</submission_date>");
                reportXML.Append("       <currency_code_local>BWP</currency_code_local>"); //Hard Coded

                #region Reporting Person
                reportXML.Append("        <reporting_person>");

                

                    reportXML.Append("          <gender>F</gender>");
                    reportXML.Append("          <title>MISS</title>");
                    reportXML.Append("          <first_name>MPHO</first_name>");
                    reportXML.Append("          <last_name>MATHIBA</last_name>");
                    reportXML.Append("          <birthdate>1982-02-26T00:00:00</birthdate>");
                    reportXML.Append("          <id_number>007520119</id_number>");
                    reportXML.Append("          <nationality1>BW</nationality1>");
                    reportXML.Append("          <phones>");
                    reportXML.Append("            <phone>");
                    reportXML.Append("              <tph_contact_type>2</tph_contact_type>" +
                    "              <tph_communication_type>M</tph_communication_type>" +
                    "              <tph_country_prefix>BW</tph_country_prefix>" +
                    "              <tph_number>71455068</tph_number>" +
                    "              <tph_extension>2222</tph_extension>" +
                    "              <comments>ALTERNATE # 2673670045</comments>" +
                    "             </phone>" +
                    "          </phones>");

                    reportXML.Append("          <email>mmathiba@bsb.bw</email>");


                    /*
                    reportXML.Append("          <gender>F</gender>");
                    reportXML.Append("          <title>MS</title>");
                    reportXML.Append("          <first_name>MAMOLEFI</first_name>");
                    reportXML.Append("          <last_name>QOBOSE</last_name>");
                    reportXML.Append("          <birthdate>1990-04-16T00:00:00</birthdate>");
                    reportXML.Append("          <id_number>484129218</id_number>");
                    reportXML.Append("          <nationality1>BW</nationality1>");
                    reportXML.Append("          <phones>");
                    reportXML.Append("            <phone>");
                    reportXML.Append("              <tph_contact_type>2</tph_contact_type>" +
                                   "              <tph_communication_type>M</tph_communication_type>" +
                                   "              <tph_country_prefix>BW</tph_country_prefix>" +
                                   "              <tph_number>+26771685576</tph_number>" +
                                   "              <tph_extension>2222</tph_extension>" +
                                   "              <comments>ALTERNATE # 71835544</comments>" +
                                   "             </phone>" +
                                   "          </phones>");

                    reportXML.Append("          <email>mqobose90@gmail.com</email>");
                    */

               

                reportXML.Append("        </reporting_person>");

                #endregion

                reportXML.Append("        <location>" +
                               "            <address_type>2</address_type>" +
                               "            <address>PLOT 1150</address> " +
                               "            <city>GABORONE</city> " +
                               "            <country_code>BW</country_code> " +
                               "        </location>");
                
                reportXML.Append("       <reason>"+ dt.Rows[i]["USERCOMMENTS"].ToString()+"</reason>"); //Optional Can be null - Hard Coded
                reportXML.Append("       <action>n/a</action>");

                if (settings.LoggingAll
                    )
                {
                    dataLayer.LogStep("step 3", "inside processxml </reporting_person> end");
                }

                int total = dt.Rows.Count;

                foreach (DataRow row in dt.Rows)
                {

                    try
                    {
                        if (settings.LoggingAll)
                        {
                            dataLayer.LogStep("Step 3", "Inside ProcessXML Transaction  number =  " + i.ToString());
                        }

                        StringBuilder transactionXML = new StringBuilder();

                        transactionXML.AppendLine("<transaction>");

                        transactionXML.AppendLine("       <transactionnumber>" + dt.Rows[i]["TRN_REF_NO"].ToString() + "</transactionnumber>"); //  Data from actb_history
                        transactionXML.AppendLine("       <internal_ref_number>" + dt.Rows[i]["TRN_REF_NO"].ToString() + "</internal_ref_number>"); //  Data from actb_history
                        transactionXML.AppendLine("       <transaction_location>" + dt.Rows[i]["BRANCH_NAME"].ToString() + "</transaction_location>");
                        transactionXML.AppendLine("       <transaction_description>" + dt.Rows[i]["trn_desc"].ToString() + "</transaction_description>");

                        DateTime varTRN_DT = Convert.ToDateTime(dt.Rows[i]["TRN_DT"].ToString());
                        string varTRN_DTFormated = varTRN_DT.Date.ToString("dd-MM-yyy HH:mm:ss");

                        transactionXML.AppendLine("       <date_transaction>" + SqlHelper.varFormatDate_To_YYYY_MM_DDTHH_MM_SS(varTRN_DTFormated) + "</date_transaction>");          //  Data from actb_history
                        transactionXML.AppendLine("       <teller>" + dt.Rows[i]["USER_ID"].ToString() + "</teller>");
                        transactionXML.AppendLine("       <authorized>" + dt.Rows[i]["AUTH_ID"].ToString() + "</authorized>");
                        transactionXML.AppendLine("       <transmode_code>S</transmode_code>"); // Refer to page 45, there are several possible options and its mandatory
                        transactionXML.AppendLine("       <transmode_comment>Cash Deposit</transmode_comment>"); // Refer to page 45, there are several possible options and its mandatory

                        string var_LCY_AMOUNT = System.Text.RegularExpressions.Regex.Replace(dt.Rows[i]["LCY_AMOUNT"].ToString(), @",", ".");

                        transactionXML.AppendLine("       <amount_local>" + System.Text.RegularExpressions.Regex.Replace(dt.Rows[i]["LCY_AMOUNT"].ToString(), @",", ".") + "</amount_local>");

                        transactionXML.AppendLine("       <t_from_my_client>");

                        transactionXML.AppendLine("       <from_funds_code>K</from_funds_code>"); // K for Cash on 5.2 in Documente

                        transactionXML.AppendLine("       <t_conductor>");

                        //transactionXML.AppendLine("       <gender>M</gender>");

                        string identification_type = dt.Rows[i]["IDType"].ToString(); // Change was implemneted from BSB Core banking system
                        int MaleOrFame = 0;

                        if (identification_type == "B") // OMANG - National Identity Card                                  
                        {
                            MaleOrFame = Convert.ToInt16(dt.Rows[i]["P_NATIONAL_ID"].ToString().Substring(4, 1));

                            if (MaleOrFame == 1)
                            {
                                transactionXML.AppendLine("       <gender>M</gender>"); // Og to add the correct Gender field
                            }
                            else
                            {
                                transactionXML.AppendLine("       <gender>F</gender>"); // Og to add the correct Gender field
                            }
                        }
                        else
                        {
                            transactionXML.AppendLine("       <gender>" + dt.Rows[i]["SEX"].ToString() + "</gender>");
                        }

                        transactionXML.AppendLine("       <first_name>" + dt.Rows[i]["FIRST_NAME"].ToString() + "</first_name>");
                        transactionXML.AppendLine("       <last_name>" + dt.Rows[i]["LAST_NAME"].ToString() + "</last_name>");

                        DateTime vardate_of_birth = Convert.ToDateTime(dt.Rows[i]["date_of_birth"].ToString());
                        string vardate_of_birthFormated = vardate_of_birth.Date.ToString("dd-MM-yyy HH:mm:ss");

                        transactionXML.AppendLine("       <birthdate>" + SqlHelper.varFormatDate_To_YYYY_MM_DDTHH_MM_SS(vardate_of_birthFormated) + "</birthdate>");          //  Data from actb_history

                        transactionXML.AppendLine("       <birth_place>Botswana</birth_place>");
                        //25 June 2019 Militant 
                        // transactionXML.AppendLine("       <id_number>" + dt.Rows[i]["P_NATIONAL_ID"].ToString() + "</id_number>");                        
                        transactionXML.AppendLine("       <nationality1>BW</nationality1>");
                        transactionXML.AppendLine("       <residence>BW</residence>");

                        transactionXML.AppendLine("          <phones>");
                        transactionXML.AppendLine("            <phone>");
                        transactionXML.AppendLine("              <tph_contact_type>2</tph_contact_type>");
                        transactionXML.AppendLine("              <tph_communication_type>M</tph_communication_type>");

                        //Trn_XML.Append("              <tph_number>" + dt.Rows[i]["NARRATIVE"].ToString() + "</tph_number>");
                        transactionXML.AppendLine("       <tph_number>" + dt.Rows[i]["Mobile"].ToString() + "</tph_number>");
                        transactionXML.AppendLine("            </phone>");
                        transactionXML.AppendLine("          </phones>");
                        transactionXML.AppendLine("          <addresses>");
                        transactionXML.AppendLine("            <address>");
                        transactionXML.AppendLine("              <address_type>2</address_type>");
                        transactionXML.AppendLine("              <address>Botswana</address>");
                        transactionXML.AppendLine("              <city>Gaborone</city>");
                        transactionXML.AppendLine("              <country_code>BW</country_code>");
                        transactionXML.AppendLine("            </address>");
                        transactionXML.AppendLine("          </addresses>");

                        transactionXML.AppendLine("              <occupation>None</occupation>");

                        transactionXML.AppendLine("            <identification>");

                        // string identification_type = "N"; // National ID - Hard coded N for now until i get the fields from BSB
                        // 18 June 2019 - Emmanuel Musabayana and Militant and Wanted on site 
                        //  dt.Rows[i]["IDType"].ToString()


                        // string identification_type = "N"; // National ID - Hard coded N for now until i get the fields from BSB

                        // This is what was working before until 18 June 2019 Sesfont Trip On site
                        /*
                        if (identification_type == "N")
                        {
                            transactionXML.AppendLine("              <type>B</type>");
                            transactionXML.AppendLine("       <number>71232334</number>");
                        }
                        else if (identification_type == "P") // Passport
                        {
                            transactionXML.AppendLine("              <type>C</type>");
                            transactionXML.AppendLine("       <number>71232334</number>");
                        }
                        else // Other for Birth certificate
                        {
                            transactionXML.AppendLine("              <type>O</type>");
                            transactionXML.AppendLine("       <number>71232334</number>");
                        }
                        */

                        //Update CashTransactions
                        //Set IDType = (Case When(IdentificationType = '101') Then 'B'

                        //                   When(IdentificationType = '102') Then 'C'

                        //                   When(IdentificationType = '103') Then 'O'

                        //                   Else('Unspecified') END)
                        //From BSBTEST_DB.t_UserCodeDetail



                        if (identification_type == "A") // Drivers License                                    
                        {
                            transactionXML.AppendLine("              <type>A</type>");
                        }
                        else if (identification_type == "B") // National Identity Card
                        {
                            transactionXML.AppendLine("              <type>B</type>");
                        }
                        else if (identification_type == "C") // Passport
                        {
                            transactionXML.AppendLine("              <type>C</type>");
                        }
                        else if (identification_type == "D") // Identification Issued by reporting Entity
                        {
                            transactionXML.AppendLine("              <type>D</type>");
                        }
                        else if (identification_type == "E") // Proof of Address
                        {
                            transactionXML.AppendLine("              <type>E</type>");
                        }
                        else if (identification_type == "O") // Other
                        {
                            transactionXML.AppendLine("              <type>O</type>");
                        }
                        else if (identification_type == "Unspecified") // Other
                        {
                            transactionXML.AppendLine("              <type>O</type>");
                        }
                        else // Other 
                        {
                            transactionXML.AppendLine("              <type>O</type>");
                        }

                        transactionXML.AppendLine("       <number>" + dt.Rows[i]["P_NATIONAL_ID"].ToString() + "</number>");

                        DateTime varppt_exp_date = Convert.ToDateTime(dt.Rows[i]["ppt_exp_date"].ToString());
                        string varppt_exp_dateFormated = varppt_exp_date.Date.ToString("dd-MM-yyy HH:mm:ss");

                        transactionXML.AppendLine("       <expiry_date>" + SqlHelper.varFormatDate_To_YYYY_MM_DDTHH_MM_SS(varppt_exp_dateFormated) + "</expiry_date>"); //Og to add the correct Gender field

                        transactionXML.AppendLine("              <issue_country>BW</issue_country>");
                        transactionXML.AppendLine("            </identification>");

                        transactionXML.AppendLine("       </t_conductor>");

                        transactionXML.AppendLine("       <from_person>");


                        if (identification_type == "B") // OMANG - National Identity Card                                  
                        {
                            MaleOrFame = Convert.ToInt16(dt.Rows[i]["P_NATIONAL_ID"].ToString().Substring(4, 1));

                            if (MaleOrFame == 1)
                            {
                                transactionXML.AppendLine("       <gender>M</gender>"); // Og to add the correct Gender field
                            }
                            else
                            {
                                transactionXML.AppendLine("       <gender>F</gender>"); // Og to add the correct Gender field
                            }
                        }
                        else
                        {
                            transactionXML.AppendLine("       <gender>" + dt.Rows[i]["SEX"].ToString() + "</gender>");
                        }


                        transactionXML.AppendLine("       <first_name>" + dt.Rows[i]["FIRST_NAME"].ToString() + "</first_name>");
                        transactionXML.AppendLine("       <last_name>" + dt.Rows[i]["LAST_NAME"].ToString() + "</last_name>");

                        transactionXML.AppendLine("       <birthdate>" + SqlHelper.varFormatDate_To_YYYY_MM_DDTHH_MM_SS(vardate_of_birthFormated) + "</birthdate>");          //  Data from actb_history

                        transactionXML.AppendLine("       <birth_place>Botswana</birth_place>");
                        //25 June 2019  Militant
                        //transactionXML.AppendLine("       <id_number>" + dt.Rows[i]["P_NATIONAL_ID"].ToString() + "</id_number>");  // Og to add the correct Gender field
                        transactionXML.AppendLine("       <nationality1>BW</nationality1>");
                        transactionXML.AppendLine("       <residence>BW</residence>");

                        transactionXML.AppendLine("          <phones>");
                        transactionXML.AppendLine("            <phone>");
                        transactionXML.AppendLine("              <tph_contact_type>2</tph_contact_type>");
                        transactionXML.AppendLine("              <tph_communication_type>M</tph_communication_type>");
                        //            Trn_XML.Append("              <tph_number>" + dt.Rows[i]["NARRATIVE"].ToString() + "</tph_number>");                        
                        transactionXML.AppendLine("       <tph_number>" + dt.Rows[i]["Mobile"].ToString() + "</tph_number>");
                        transactionXML.AppendLine("            </phone>");
                        transactionXML.AppendLine("          </phones>");
                        transactionXML.AppendLine("          <addresses>");
                        transactionXML.AppendLine("            <address>");
                        transactionXML.AppendLine("              <address_type>2</address_type>");
                        transactionXML.AppendLine("              <address>Botswana</address>");
                        transactionXML.AppendLine("              <city>Gaborone</city>");
                        transactionXML.AppendLine("              <country_code>BW</country_code>");
                        transactionXML.AppendLine("            </address>");
                        transactionXML.AppendLine("          </addresses>");

                        transactionXML.AppendLine("              <occupation>None</occupation>");

                        transactionXML.AppendLine("            <identification>");

                        string identification_type_Receiver = dt.Rows[i]["IDType"].ToString(); // Change was implemneted from BSB Core banking system

                        if (identification_type_Receiver == "A") // Drivers License                                    
                        {
                            transactionXML.AppendLine("              <type>A</type>");
                        }
                        else if (identification_type_Receiver == "B") // National Identity Card
                        {
                            transactionXML.AppendLine("              <type>B</type>");
                        }
                        else if (identification_type_Receiver == "C") // Other
                        {
                            transactionXML.AppendLine("              <type>C</type>");
                        }
                        else if (identification_type_Receiver == "D") // Identification Issued by reporting Entity
                        {
                            transactionXML.AppendLine("              <type>D</type>");
                        }
                        else if (identification_type_Receiver == "E") // Proof of Address
                        {
                            transactionXML.AppendLine("              <type>E</type>");
                        }
                        else if (identification_type_Receiver == "O") // Other
                        {
                            transactionXML.AppendLine("              <type>O</type>");
                        }
                        else if (identification_type_Receiver == "Unspecified") // Other
                        {
                            transactionXML.AppendLine("              <type>O</type>");
                        }
                        else // Other 
                        {

                            transactionXML.AppendLine("              <type>O</type>");
                        }
                        //Militant 25 2019 June 
                        //transactionXML.AppendLine("       <number> " + dt.Rows[i]["P_NATIONAL_ID"].ToString() + "</number>");
                        if (identification_type == "B") // OMANG - National Identity Card                                  
                        {
                            transactionXML.AppendLine("       <number>" + dt.Rows[i]["P_NATIONAL_ID"].ToString() + "</number>");
                        }
                        else // All other Identification types
                        {
                            transactionXML.AppendLine("       <number>" + dt.Rows[i]["P_NATIONAL_ID"].ToString() + "</number>");
                        }

                        DateTime varppt_exp_date_second = Convert.ToDateTime(dt.Rows[i]["ppt_exp_date"].ToString());
                        string varppt_exp_date_secondFormated = varppt_exp_date_second.Date.ToString("dd-MM-yyy HH:mm:ss");

                        transactionXML.AppendLine("       <expiry_date>" + SqlHelper.varFormatDate_To_YYYY_MM_DDTHH_MM_SS(varppt_exp_date_secondFormated) + "</expiry_date>"); //Og to add the correct Gender field
                        transactionXML.AppendLine("              <issue_country>BW</issue_country>");
                        transactionXML.AppendLine("            </identification>");

                        transactionXML.AppendLine("       </from_person>"); // Optional

                        transactionXML.AppendLine("       <from_country>BW</from_country>"); // Optional

                        transactionXML.AppendLine("       </t_from_my_client>");

                        transactionXML.AppendLine("       <t_to_my_client>");

                        transactionXML.AppendLine("       <to_funds_code>A</to_funds_code>");

                        transactionXML.AppendLine("       <to_account>");

                        transactionXML.AppendLine("       <institution_name>BSB</institution_name>");
                        transactionXML.AppendLine("       <swift>None</swift>");
                        transactionXML.AppendLine("       <branch>None</branch>");
                        transactionXML.AppendLine("       <account>" + dt.Rows[i]["AC_NO"].ToString() + "</account>");
                        transactionXML.AppendLine("       <currency_code>BWP</currency_code>");
                        transactionXML.AppendLine("       <account_name>" + dt.Rows[i]["AC_DESC"].ToString() + "</account_name>");
                        //Trn_XML.Append("       <personal_account_type>" + dt.Rows[i]["ACCOUNT_TYPE"].ToString() + "</personal_account_type>");
                        transactionXML.AppendLine("       <personal_account_type>C</personal_account_type>");

                        transactionXML.AppendLine("          <signatory>");

                        transactionXML.AppendLine("          <t_person>");

                        if (dt.Rows[i]["CUSTOMER_TYPE"].ToString().Trim() == "I") // STTM_CUST_PERSONAL
                        {

                            if (identification_type == "B") // OMANG - National Identity Card                                  
                            {
                                MaleOrFame = Convert.ToInt16(dt.Rows[i]["P_NATIONAL_ID"].ToString().Substring(4, 1));

                                if (MaleOrFame == 1)
                                {
                                    transactionXML.AppendLine("       <gender>M</gender>"); // Og to add the correct Gender field
                                }
                                else
                                {
                                    transactionXML.AppendLine("       <gender>F</gender>"); // Og to add the correct Gender field
                                }
                            }
                            else
                            {
                                transactionXML.AppendLine("       <gender>" + dt.Rows[i]["SEX"].ToString() + "</gender>");
                            }
                            transactionXML.AppendLine("       <first_name>" + dt.Rows[i]["FIRST_NAME"].ToString() + "</first_name>");
                            transactionXML.AppendLine("       <last_name>" + dt.Rows[i]["LAST_NAME"].ToString() + "</last_name>");
                            //Trn_XML.Append("       <birthdate>1957-09-04T00:00:00</birthdate>"); //Og to add the correct Gender field

                            DateTime varDATE_OF_BIRTH = Convert.ToDateTime(dt.Rows[i]["DATE_OF_BIRTH"].ToString());
                            string varDATE_OF_BIRTHFormated = varDATE_OF_BIRTH.Date.ToString("dd-MM-yyy HH:mm:ss");

                            transactionXML.AppendLine("       <birthdate>" + SqlHelper.varFormatDate_To_YYYY_MM_DDTHH_MM_SS(varDATE_OF_BIRTHFormated) + "</birthdate>");
                            transactionXML.AppendLine("       <birth_place>Botswana</birth_place>");

                            //25 June 2019 Militant
                            /*
                            if (dt.Rows[i]["P_NATIONAL_ID"].ToString().Trim().Length == 0)
                            {
                                //Trn_XML.Append("       <number>" + dt.Rows[i]["PASSPORT_NO"].ToString() + "</number>");
                                transactionXML.AppendLine("       <id_number>" + dt.Rows[i]["P_NATIONAL_ID"].ToString() + "</id_number>");
                            }
                            else
                            {
                                //Trn_XML.Append("       <number>" + dt.Rows[i]["P_NATIONAL_ID"].ToString() + "</number>");
                                transactionXML.AppendLine("       <id_number>" + dt.Rows[i]["P_NATIONAL_ID"].ToString() + "</id_number>");

                            }
                            */
                        }
                        else // STTM_CUST_CORPORATE
                        {

                            if (identification_type == "B") // OMANG - National Identity Card                                  
                            {
                                MaleOrFame = Convert.ToInt16(dt.Rows[i]["P_NATIONAL_ID"].ToString().Substring(4, 1));

                                if (MaleOrFame == 1)
                                {
                                    transactionXML.AppendLine("       <gender>M</gender>"); // Og to add the correct Gender field
                                }
                                else
                                {
                                    transactionXML.AppendLine("       <gender>F</gender>"); // Og to add the correct Gender field
                                }
                            }
                            else
                            {
                                transactionXML.AppendLine("       <gender>" + dt.Rows[i]["SEX"].ToString() + "</gender>");
                            }

                            transactionXML.AppendLine("       <first_name>" + dt.Rows[i]["AC_DESC"].ToString() + "</first_name>");
                            transactionXML.AppendLine("       <last_name>" + dt.Rows[i]["AC_DESC"].ToString() + "</last_name>");
                            //Trn_XML.Append("       <birthdate>1957-09-04T00:00:00</birthdate>"); //Og to add the correct Gender field

                            DateTime varDATE_OF_BIRTH_New = Convert.ToDateTime(dt.Rows[i]["DATE_OF_BIRTH"].ToString());
                            string varvarDATE_OF_BIRTH_NewFormated = varDATE_OF_BIRTH_New.Date.ToString("dd-MM-yyy HH:mm:ss");

                            transactionXML.AppendLine("       <birthdate>" + SqlHelper.varFormatDate_To_YYYY_MM_DDTHH_MM_SS(varvarDATE_OF_BIRTH_NewFormated) + "</birthdate>"); //Og to add the correct Gender field
                            transactionXML.AppendLine("       <birth_place>Botswana</birth_place>");

                            //Trn_XML.Append("       <number>" + dt.Rows[i]["C_NATIONAL_ID"].ToString() + "</number>");
                            //transactionXML.AppendLine("       <id_number>71232334</id_number>");
                            //25 Jne 2019 Militant
                            //transactionXML.AppendLine("       <id_number>" + dt.Rows[i]["P_NATIONAL_ID"].ToString() + "</id_number>");
                        }

                        transactionXML.AppendLine("       <nationality1>BW</nationality1>");
                        transactionXML.AppendLine("       <residence>BW</residence>");

                        transactionXML.AppendLine("          <phones>");
                        transactionXML.AppendLine("            <phone>");
                        transactionXML.AppendLine("              <tph_contact_type>2</tph_contact_type>");
                        transactionXML.AppendLine("              <tph_communication_type>M</tph_communication_type>");
                        //Trn_XML.Append("              <tph_number>" + dt.Rows[i]["TELEPHONE"].ToString() + "</tph_number>");
                        //transactionXML.AppendLine("              <tph_number>7234345</tph_number>");
                        transactionXML.AppendLine("       <tph_number>" + dt.Rows[i]["Mobile"].ToString() + "</tph_number>");
                        transactionXML.AppendLine("            </phone>");
                        transactionXML.AppendLine("          </phones>");
                        transactionXML.AppendLine("          <addresses>");
                        transactionXML.AppendLine("            <address>");
                        transactionXML.AppendLine("              <address_type>2</address_type>");
                        transactionXML.AppendLine("              <address>" + dt.Rows[i]["Address_1"].ToString() + "</address>");
                        transactionXML.AppendLine("              <city>" + dt.Rows[i]["Address_2"].ToString() + "</city>");
                        transactionXML.AppendLine("              <country_code>BW</country_code>");
                        transactionXML.AppendLine("            </address>");
                        transactionXML.AppendLine("          </addresses>");

                        if (dt.Rows[i]["designation"].ToString().Trim().Length == 0)
                        {
                            transactionXML.AppendLine("              <occupation>None</occupation>");
                        }
                        else
                        {
                            transactionXML.AppendLine("              <occupation>" + dt.Rows[i]["designation"].ToString() + "</occupation>");
                        }

                        transactionXML.AppendLine("            <identification>");

                        transactionXML.AppendLine("              <type>" + identification_type + "</type>");


                        if (identification_type == "B") // OMANG - National Identity Card                                  
                        {
                            transactionXML.AppendLine("       <number>" + dt.Rows[i]["P_NATIONAL_ID"].ToString() + "</number>");
                        }
                        else // All other Identification types
                        {
                            transactionXML.AppendLine("       <number>" + dt.Rows[i]["P_NATIONAL_ID"].ToString() + "</number>");
                        }
                        // 25 June 2019 Militant.
                        //if (dt.Rows[i]["CUSTOMER_TYPE"].ToString().Trim() == "I")
                        //{
                        //    if (dt.Rows[i]["P_NATIONAL_ID"].ToString().Trim().Length == 0)
                        //    {
                        //        //Trn_XML.Append("       <number>" + dt.Rows[i]["PASSPORT_NO"].ToString() + "</number>");
                        //        transactionXML.AppendLine("       <number>71232334</number>");
                        //    }
                        //    else
                        //    {
                        //        // Trn_XML.Append("       <number>" + dt.Rows[i]["P_NATIONAL_ID"].ToString() + "</number>");
                        //        transactionXML.AppendLine("       <number>" + dt.Rows[i]["P_NATIONAL_ID"].ToString().Trim() + "</number>");

                        //    }
                        //}
                        //else
                        //{
                        //    //Trn_XML.Append("       <number>" + dt.Rows[i]["C_NATIONAL_ID"].ToString() + "</number>");
                        //    transactionXML.AppendLine("       <number>71232334</number>");
                        //}

                        //transactionXML.AppendLine("              <expiry_date>2017-09-07T00:00:00</expiry_date>");
                        DateTime varVarppt_exp_date = Convert.ToDateTime(dt.Rows[i]["ppt_exp_date"].ToString());
                        string varvarppt_exp_dateFormated = varVarppt_exp_date.Date.ToString("dd-MM-yyy HH:mm:ss");

                        transactionXML.AppendLine("       <expiry_date>" + SqlHelper.varFormatDate_To_YYYY_MM_DDTHH_MM_SS(varvarppt_exp_dateFormated) + "</expiry_date>"); //Og to add the correct Gender field

                        transactionXML.AppendLine("              <issue_country>BW</issue_country>");
                        transactionXML.AppendLine("            </identification>");

                        transactionXML.AppendLine("          </t_person>");

                        transactionXML.AppendLine("          </signatory>");

                        //Trn_XML.Append("       <opened>1957-09-04T00:00:00</opened>"); //Og to add the correct Gender field

                        DateTime varAC_OPEN_DATE = Convert.ToDateTime(dt.Rows[i]["AC_OPEN_DATE"].ToString());
                        string varAC_OPEN_DATE_Formated = varAC_OPEN_DATE.Date.ToString("dd-MM-yyy HH:mm:ss");

                        transactionXML.AppendLine("              <opened>" + SqlHelper.varFormatDate_To_YYYY_MM_DDTHH_MM_SS(varAC_OPEN_DATE_Formated) + "</opened>");
                        //Trn_XML.Append("              <status_code>" + dt.Rows[i]["RECORD_STAT"].ToString() + "</status_code>");
                        transactionXML.AppendLine("              <status_code>A</status_code>");

                        transactionXML.AppendLine("       </to_account>");

                        transactionXML.AppendLine("       <to_country>BW</to_country>");

                        transactionXML.AppendLine("       </t_to_my_client>");

                        transactionXML.AppendLine("       </transaction>");

                        reportXML.Append(transactionXML);
                    }
                    catch (Exception ex)
                    {
                        dataLayer.LogStep(ex.Message.ToString(), dt.Rows[i]["TRN_REF_NO"].ToString() + " : Error on reference number : Inside foreach DataRow row in dt.Rows for transaction Tag");
                        // Create a table which will have the fault skipped records TRN_REF_NO get logged seperately into that table
                        // I should create a Transaction Table for landing the daily records from the Core banking system
                        // Then also create and a Master table for Archiving the processed records
                        // This will increase effeciency
                        dataLayer.Log_GET_CASH_Deposit_Skipped_Records(dt.Rows[i]["TRN_REF_NO"].ToString(), ex.Message.ToString());
                        continue; // ignore the error as it has been logged then continue to the next record
                        //throw;
                    }
                    i++; // This makes the i counter increament for the i in the dt.Rows[i] foreach (DataRow row in dt.Rows)
                }

                if (settings.LoggingAll)
                {
                    dataLayer.LogStep("Step 4", "Inside ProcessXML just before </report> end ");
                }

                reportXML.Append("</report>");

                Trn_XML_String = reportXML.ToString();


                Trn_XML_String = System.Text.RegularExpressions.Regex.Replace(Trn_XML_String, @"\s{2,}", " ");

                Trn_XML_String = System.Text.RegularExpressions.Regex.Replace(Trn_XML_String, @"\s{2,}", " ");

                Trn_XML_String = System.Text.RegularExpressions.Regex.Replace(Trn_XML_String, @"&", " ");
                Trn_XML_String = System.Text.RegularExpressions.Regex.Replace(Trn_XML_String, @" />", "/>");
                //Trn_XML_String = System.Text.RegularExpressions.Regex.Replace(Trn_XML_String, @"-", " ");
                Trn_XML_String = System.Text.RegularExpressions.Regex.Replace(Trn_XML_String, @",", " ");
                Trn_XML_String = System.Text.RegularExpressions.Regex.Replace(Trn_XML_String, @" <", "<");
                Trn_XML_String = System.Text.RegularExpressions.Regex.Replace(Trn_XML_String, @"< ", "<");

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Trn_XML_String;
        }


    }
}
