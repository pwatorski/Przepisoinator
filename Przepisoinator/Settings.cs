using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Przepisoinator
{

    public class SettingsCarrier
    {
        public string StoragePath { get; set; }

        public SettingsCarrier() 
        {
            StoragePath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Przepisoinator");
        }
    }
    public class Settings
    {
        public static string MainStoragePath { get => instance.StoragePath; set
            {
                instance.StoragePath = value;
                UpdateStoragePaths();
            }
        }

        public static string RecepyStoragePath { get; private set; }
        public static string UnitsStoragePath { get; private set; }

        private static SettingsCarrier instance;

        static Settings()
        {
            Load();
            UpdateStoragePaths();
        }

        static void UpdateStoragePaths()
        {
            RecepyStoragePath = Path.Join(MainStoragePath, "recepies");
            UnitsStoragePath = Path.Join(MainStoragePath, "units");
        }

        public static void Save()
        {
            var appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string settingsPath = Path.Join(appdataPath, "przepisoinator");
            if (!Directory.Exists(settingsPath))
            {
                Directory.CreateDirectory(settingsPath);
            }
            settingsPath = Path.Join(settingsPath, "options.json");
            using (StreamWriter sw = new StreamWriter(settingsPath, false, encoding:Encoding.UTF8))
            {
                sw.Write(ToJson());
            }
        }
        public static SettingsCarrier Load() 
        {
            var appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string settingsPath = Path.Join(appdataPath, "przepisoinator", "options.json");
            if (!Path.Exists(settingsPath))
            {
                instance = new SettingsCarrier();
            }
            else
            {
                instance = FromJson(settingsPath);
            }
            return instance;
        }

        public static string ToJson()
        {
            return JsonSerializer.Serialize(instance, Misc.JsonOptions);
        }

        public static SettingsCarrier FromJson(string settingsPath)
        {
            string json = "";
            using (StreamReader sr = new StreamReader(settingsPath, encoding: Encoding.UTF8))
            {
                json = sr.ReadToEnd();
            }
            return JsonSerializer.Deserialize<SettingsCarrier>(json) ?? new SettingsCarrier();
        }
    }
}
