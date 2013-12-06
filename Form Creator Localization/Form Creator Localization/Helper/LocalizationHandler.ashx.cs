using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Form_Creator_Localization.Classes;
using Form_Creator_Localization.DAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Form_Creator_Localization.Helper
{
    public class LocalizationHandler : IHttpHandler
    {
        private Localization localization { get; set; }
        private FormFields formFields { get; set; }
        private DataSet dataResults { get; set; }

        public void ProcessRequest(HttpContext context)
        {
            //Check for Type of Query String
            if (context.Request.QueryString.ToString().Contains("language"))
            {
                //Process for language specific
                int currentLangaugeIndex = int.Parse(context.Request.QueryString[0]);

                //Validation Check for language
                if (currentLangaugeIndex > 0)
                {
                    this.localization = new Localization();
                    dataResults = this.localization.GetLanguageByID(currentLangaugeIndex);

                    //Seralize the object for a return respone..
                    string jsonResults = SeralizeResults(dataResults.Tables[0]);
                    context.Response.Write(jsonResults);
                }
                else{
                    context.Response.Write("No Data");
                }
            }
            if (!string.IsNullOrEmpty(context.Request.Form["Results"])){
                //Process for updates
                string results = context.Request.Form["Results"];
                if (results != null){
                    int resultsCount = LangaugeResults(results);
                    context.Response.Write("Rows updated: \t" + resultsCount.ToString());
                }
            }
        }

        private int LangaugeResults(string results)
        {
            DAL.Localization data = new Localization();
            return data.UpdateFieldsByLangauge(results);
        }

        private string SeralizeResults(DataTable dataTable)
        {
            JavaScriptSerializer seralizer = new JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn dataColumn in dataTable.Columns)
                {
                    row.Add(dataColumn.ColumnName, dataRow[dataColumn]);
                }
                rows.Add(row);
            }
            //return serializer 
            return seralizer.Serialize(rows);
        }

        //private int Mapper(string language)
        //{
        //    int currentLanguage = 0;
        //    switch (language.ToLower())
        //    {
        //        case "english":
        //            return currentLanguage = 1;
        //        case "german":
        //            return currentLanguage = 2;
        //        case "japanese":
        //            return currentLanguage = 3;
        //        default:
        //            break;
        //    }
        //    return currentLanguage;
        //}

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public class FormFields
        {
            //Language
            public string CurrentLanguage { get; set; }

            //Fields
            public string City { get; set; }
            public string Comments { get; set; }
            public string Company { get; set; }
            public string CompanyDescription { get; set; }
            public string ContentBlock { get; set; }
            public string Country { get; set; }
            public string CustomDropDown { get; set; }
            public string Date { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string GenericText { get; set; }
            public string GroupCheckboxes { get; set; }
            public string Image { get; set; }
            public string IndustryDescription { get; set; }
            public string LastName { get; set; }
            public string NavigationTop { get; set; }
            public string Number { get; set; }
            public string OrganizationRole { get; set; }
            public string Phone { get; set; }
            public string PrimaryRelationWithEsri { get; set; }
            public string State { get; set; }
            public string StreetAddress { get; set; }
            public string Submit { get; set; }
            public string Suite { get; set; }
            public string URL { get; set; }
            public string Users { get; set; }
            public string ZipCode { get; set; }
        }
    }
}