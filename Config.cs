using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;

namespace FileManager
{
    class Config
    {
        
        public string SavedPath;

        //Чтение параметров конфигурации
        public void Read()
        {
            SavedPath = ConfigurationManager.AppSettings.Get("Path");
        }

        //Сохранение параметров конфигурации
        public void Save(string APath)
        {
            Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            conf.AppSettings.Settings["Path"].Value = APath;
            conf.Save(ConfigurationSaveMode.Modified);
            //ConfigurationManager.RefreshSection("appSettings");
        }

    }
}
