using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VSRemoteDebugger
{
    // Cannot nest these classes because XAML has bugs resolving them.
    public class ConfigData
    {
        public string CurrentlySelected { get; set; }
        public Dictionary<string, ConfigFileDataRow> data { get; set; } = new Dictionary<string, ConfigFileDataRow>();
    }

    public class ConfigFileDataRow
    {
        public string Hostname { get; set; } = "localhost";
        public string DotnetLocation { get; set; } = "~/.dotnet/dotnet";
        public string GroupName { get; set; } = "group";
        public string OutputDirectory { get; set; } = "~/project";
        public string Username { get; set; } = "username";
        public string VsdbgLocation { get; set; } = "~/.vsdbg/vsdbg";
        public bool? DontDebug { get; set; } = false;
        public bool? Publish { get; set; } = false;
        public bool? UseCommandLineFromProject { get; set; } = false;
    }

    public static class ConfigFile
    {
        static string filename {
            get { return Environment.GetFolderPath(folder: Environment.SpecialFolder.UserProfile) + "/.VSRemoteDebugger.conf"; }
        }



        public volatile static ConfigData Data = new ConfigData();

        public static ConfigFileDataRow Current { get {
                ConfigFileDataRow temp = null;
                Data.data.TryGetValue(Data.CurrentlySelected, out temp);
                return temp;
            }
            set => Data.data[Data.CurrentlySelected] = value;
        }

        static ConfigFile()
        {
            if(!File.Exists(filename)){
                Save();
            } else {
                Data = JsonConvert.DeserializeObject<ConfigData>(File.ReadAllText(filename));
            }
        }

        public static void Save()
        {
            string content = JsonConvert.SerializeObject(Data, Formatting.Indented);
            File.WriteAllText(filename, content);
        }
    }
}
