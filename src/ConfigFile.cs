using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VSRemoteDebugger
{
    public static class ConfigFile
    {
        static string filename {
            get { return Environment.GetFolderPath(folder: Environment.SpecialFolder.UserProfile) + "/.VSRemoteDebugger.conf"; }
        }

        public static Dictionary<string, ConfigFileDataRow> data = new Dictionary<string, ConfigFileDataRow>();

        public class ConfigFileDataRow
        {
            public string Hostname { get; set; } = "localhost";
            public string DotnetLocation { get; set; } = "~/.dotnet/dotnet";
            public string GroupName { get; set; } = "group";
            public string OutputDirectory { get; set; } = "~/project";
            public string Username { get; set; } = "username";
            public string VsdbgLocation { get; set; } = "~/.vsdbg/vsdbg";
            public bool DontDebug { get; set; } = false;
            public bool Publish { get; set; } = false;
            public bool UseCommandLineFromProject { get; set; } = false;
        }

        static ConfigFile()
        {
            if(!File.Exists(filename)){
                Save();
            } else {
                data = JsonConvert.DeserializeObject<Dictionary<string, ConfigFileDataRow>>(File.ReadAllText(filename));
            }
        }

        public static void Save()
        {
            string content = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filename, content);
        }
    }
}
