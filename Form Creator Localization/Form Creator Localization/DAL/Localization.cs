using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace Form_Creator_Localization.DAL
{
    public class Localization
    {
        
        #region Properties
        
        //----------
        //Properties
        //----------

        private static string ConnectionString = "Data Source=ESQUIBEL2;Database=FormGenerator;user id=sa;password=localhost;"; //ConfigurationManager.ConnectionStrings["FormGenerator"].ToString();
        private static string SprocGetAllLanguage = "[dbo].[spr_GetLanguages]";
        private static string SprocGetLanguageByID = "[dbo].[spr_GetControlTextByLanguageLookup_ID ]";
        private static string SprocUpdateLanguageByID = "[dbo].[spr_SaveControlTextByJson]";
        private DataSet languageFields {get; set;}

        #endregion

        #region Methods

        //-------
        //Methods
        //-------

        public DataSet GetAllLanguages()
        {
            this.languageFields = new DataSet();
            using (SqlConnection sCon = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sCmd = new SqlCommand(SprocGetAllLanguage, sCon))
                {
                    sCmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter sAdapt = new SqlDataAdapter(sCmd))
                    {
                        sCon.Open();
                        sAdapt.Fill(languageFields);
                        sCon.Close();
                    }
                }
            }

            return this.languageFields;
        }

        public DataSet GetLanguageByID(int language)
        {
            this.languageFields = new DataSet();
            using (SqlConnection sCon = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sCmd = new SqlCommand(SprocGetLanguageByID, sCon))
                {
                    sCmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter sAdapt = new SqlDataAdapter(sCmd))
                    {
                        sCon.Open();
                        sCmd.Parameters.AddWithValue("@LanguageLookup_ID", language);
                        sAdapt.Fill(languageFields);
                        sCon.Close();
                    }
                }
            }

            return this.languageFields;
        }

        
        public int UpdateFieldsByLangauge(string results)
        {
            int recordsUpdated = 0;
            using (SqlConnection sCon = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sCmd = new SqlCommand(SprocUpdateLanguageByID, sCon))
                {
                    //Command Type..
                    sCmd.CommandType = CommandType.StoredProcedure;
                    
                    //Parameters Set
                    sCmd.Parameters.AddWithValue("@Json", results);
                    //Open Connection
                    sCon.Open();
                    
                    /*Update table*/
                    try{
                        using (SqlDataReader reader = sCmd.ExecuteReader())
                        {
                            reader.Read();
                            recordsUpdated = int.Parse(reader[0].ToString());
                            reader.Close();
                        }
                    }
                    catch (SqlException sqlError){
                        Console.Write(sqlError.Message.ToString());
                        return recordsUpdated;
                    }
                }
            }
            return recordsUpdated;
        }

        #endregion      

    }
}