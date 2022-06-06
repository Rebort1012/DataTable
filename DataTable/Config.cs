using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataTable
{
    internal class Config
    {
        private static Config instance;
        public static Config I => instance ?? (instance = new Config());

        private Config()
        {
            LoadConfig();
        }

        public enum ExportType
        {
            Json,
            Protobuf,
            Xml,
            Bytes,
        }

        private string configPath;
        public string excelPath;
        public string dataPath;
        public string classPath;
        public ExportType exportType;
        public bool isExportServer;

        public readonly List<string> enumTypeList = new List<string>();

        private void LoadConfig()
        {
            configPath = "./config.txt";
            string temp = FileTool.ReadString(configPath);
            string[] strs = temp.Split(';');

            for (int i = 0; i < strs.Length; ++i)
            {
                if (strs[i].StartsWith("#"))
                    continue;

                string[] tempStrs = strs[i].Split(':');

                string name = Regex.Replace(tempStrs[0], "[^A-Za-z0-9]", "");

                switch (name)
                {
                    case "excelPath": excelPath = tempStrs[1]; break;
                    case "dataPath": dataPath = tempStrs[1]; break;
                    case "classPath": classPath = tempStrs[1]; break;
                    case "exportType": exportType = (ExportType)Enum.Parse(typeof(ExportType), tempStrs[1]); break;
                    case "isExportServer": isExportServer = tempStrs[1].ToLower() != "false"; break;
                }
            }

            FileTool.WriteString($"{classPath}EnumType.cs", "");
        }
    }
}
