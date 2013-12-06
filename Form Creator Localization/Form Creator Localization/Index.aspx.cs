using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Form_Creator_Localization
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetLocalizationLanguages();
        }

        private void GetLocalizationLanguages()
        {
            DAL.Localization data = new DAL.Localization();
            DataSet localizationLaguages =  data.GetAllLanguages();

            if (localizationLaguages.Tables != null)
                SetLocalizationLanguages(localizationLaguages.Tables[0]);
        }

        private void SetLocalizationLanguages(DataTable languageTable)
        {
            foreach (DataRow row in languageTable.Rows)
            {
                if (!string.IsNullOrEmpty(row["Name"].ToString()))
                    ddlLanguage.Items.Add(new ListItem(row["Name"].ToString(), row["LanguageLookup_ID"].ToString()));
            }
        }
    }
}