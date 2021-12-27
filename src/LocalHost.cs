﻿using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace VSRemoteDebugger
{
    internal class LocalHost
    {
        internal LocalHost(string remoteUserName, string remoteIP, string remoteVsDbgPath, string remoteDotnetPath, string remoteDebugFolderPath)
        {
            _remoteUserName = remoteUserName;
            _remoteIP = remoteIP;
            _remoteVsDbgPath = remoteVsDbgPath;
            _remoteDotnetPath = remoteDotnetPath;
            _remoteDebugFolderPath = remoteDebugFolderPath;
        }

        private readonly string _remoteUserName;
        private readonly string _remoteIP;
        private readonly string _remoteVsDbgPath;
        private readonly string _remoteDotnetPath;
        private readonly string _remoteDebugFolderPath;

        internal static string DEBUG_ADAPTER_HOST_FILENAME => "launch.json";
        internal static string HOME_DIR_PATH => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        internal static string SSH_KEY_PATH => Path.Combine(HOME_DIR_PATH, ".ssh\\id_rsa");

        internal string DebugAdapterHostFilePath { get; set; }
        internal string ProjectName { get; set; }
        internal string Assemblyname { get; set; }
        internal string ProjectFullName { get; set; }
        internal string SolutionFullName { get; set; }
        internal string SolutionDirPath { get; set; }
        internal string ProjectConfigName { get; set; }
        internal string OutputDirName { get; set; }
        internal string OutputDirFullName { get; set; }

        /// <summary>
        /// Space delimited string containing command line arguments
        /// </summary>
        internal string CommandLineArguments { get; set; } = String.Empty;

        /// <summary>
        /// Generates a temporary json file and returns its path
        /// </summary>
        /// <returns>Full path to the generated json file</returns>
        internal string GenJson()
        {
            dynamic json = new JObject();
            json.version = "0.2.0";
            json.adapter = "ssh.exe";
            json.adapterArgs = $"-i {SSH_KEY_PATH} {_remoteUserName}@{_remoteIP} {_remoteVsDbgPath} --interpreter=vscode";

            json.configurations = new JArray() as dynamic;
            dynamic config = new JObject();
            config.project = "default";
            config.type = "coreclr";
            config.request = "launch";
            config.program = _remoteDotnetPath;
            var jarrObj = new JArray($"./{Assemblyname}.dll");
            if (CommandLineArguments.Length > 0)
            {
                foreach (var arg in CommandLineArguments.Split(' '))
                {
                    jarrObj.Add(arg);
                }
            }
			config.args = jarrObj;
            config.cwd = _remoteDebugFolderPath;
            config.stopAtEntry = "false";
            config.console = "internalConsole";
            json.configurations.Add(config);

            string tempJsonPath = Path.Combine(Path.GetTempPath(), DEBUG_ADAPTER_HOST_FILENAME);
            File.WriteAllText(tempJsonPath, Convert.ToString(json));

            return tempJsonPath;
        }
    }
}
