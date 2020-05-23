using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectorV2
{
    class WebServisURL
    {
        public static void Al (int FirmaID, int ServisTipi, int SablonTipi)
        {
            sqltestEntities data = new sqltestEntities();

            var list = data.S_WebServisTanimlari(FirmaID).Where(c => c.ServisTipi_Tip1 == ServisTipi && c.SablonTipi_Tip2 == SablonTipi).ToList();
            string url = list.FirstOrDefault().URL;

            var tiplist = data.S_Tip(ServisTipi, -1).ToList();
            string parametre = tiplist.FirstOrDefault().Parametre;


            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            ConfigurationSectionGroup applicationSectionGroup = config.GetSectionGroup("applicationSettings");
            ConfigurationSection applicationConfigSection = applicationSectionGroup.Sections["ConnectorV2.Properties.Settings"];
            ClientSettingsSection clientSection = (ClientSettingsSection)applicationConfigSection;

            //Database Configuration Setting
            SettingElement applicationSetting = clientSection.Settings.Get(parametre);
            applicationSetting.Value.ValueXml.InnerXml = url;

            applicationConfigSection.SectionInformation.ForceSave = true;
            config.Save();



        }

        public static void MutabakatAl(int FirmaID, int ServisTipi, int SablonTipi)
        {
            sqltestEntities data = new sqltestEntities();

            var list = data.S_WebServisTanimlari(FirmaID).Where(c => c.ServisTipi_Tip1 == ServisTipi && c.ServisYonu == true).ToList();
            string url = list.FirstOrDefault().URL;

            var tiplist = data.S_Tip(ServisTipi, -1).ToList();
            string parametre = tiplist.FirstOrDefault().Parametre;

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            ConfigurationSectionGroup applicationSectionGroup = config.GetSectionGroup("applicationSettings");
            ConfigurationSection applicationConfigSection = applicationSectionGroup.Sections["ConnectorV2.Properties.Settings"];
            ClientSettingsSection clientSection = (ClientSettingsSection)applicationConfigSection;

            //Database Configuration Setting
            SettingElement applicationSetting = clientSection.Settings.Get(parametre);
            applicationSetting.Value.ValueXml.InnerXml = url;

            applicationConfigSection.SectionInformation.ForceSave = true;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("applicationSettings");

            string degisiklik = applicationSetting.Value.ValueXml.InnerXml;


        }



    }
}
