using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using Excel;


namespace DataTable
{
    internal class ExcelTool
    {
        public ExcelTool()
        {
            InitClassStr();
        }

        private string classStr;
        private string propertyStr;
        private string parseStr;
        private string enumStr;

        private string dataStr;

        private int dicIndex = 0;

        public void CreateDataTable(string path)
        {
            string classPath = Config.I.classPath;
            DataSet result = OpenExcel(path);

            int tableNum = result.Tables.Count;
            for (int index = 0; index < tableNum; index++)
            {
                string className = "";
                if (tableNum > 1)
                {
                    if (result.Tables[index].TableName.ToLower().Contains("sheet"))
                        continue;
                    className = result.Tables[index].TableName;
                }
                else
                {
                    string[] tempS11 = path.Split('/');
                    className = tempS11[tempS11.Length - 1].Split('.')[0];
                }
                int columns = result.Tables[index].Columns.Count;
                int rows = result.Tables[index].Rows.Count;

                //TODO:单个sheet====0
                //sheet名 = 类名
                string tempVal = classStr;
                tempVal = tempVal.Replace("className", className);

                //正式数据起点行
                int startIndex = 0;

                dataStr = "";
                //List<string> addDic = new List<string>();

                Dictionary<int, string> dataNameDic = new Dictionary<int, string>();
                Dictionary<int, string> dataTypeDic = new Dictionary<int, string>();
                //枚举类型记录
                Dictionary<string, Dictionary<string, int>> enumDic = new Dictionary<string, Dictionary<string, int>>();
                //数组类型是不是字符串
                Dictionary<string, bool> arrayDic = new Dictionary<string, bool>();

                Dictionary<string, List<string>> dicDic = new Dictionary<string, List<string>>();
                Dictionary<string, bool> dicTypeDic = new Dictionary<string, bool>();

                //第一行类型
                for (int i = 1; i < columns; ++i)
                {
                    string dataType = "";
                    string dataName = "";
                    string add = "";
                    string kv_key = "";


                    for (int j = 0; j < 3; ++j)
                    {
                        string tempRow0 = result.Tables[index].Rows[j][0].ToString();

                        if (tempRow0.StartsWith("#"))
                            continue;
                        else if (tempRow0.ToLower() == "type")
                        {
                            dataType = result.Tables[index].Rows[j][i].ToString();
                            if (dataType.EndsWith("[]"))
                            {
                                add = dataType.Split('[')[0];
                                dataType = "array";
                            }
                            else if (dataType.StartsWith("dic"))
                            {
                                dataType = dataType.Substring(0, dataType.Length - 1);
                                add = dataType.Split('<')[1];
                                dataType = "dic";
                            }
                            else if (dataType.StartsWith("enum"))
                            {
                                add = "";
                                int enumCount = 1;
                                for (int k = 4; k < rows; ++k)
                                {
                                    string strenum = result.Tables[index].Rows[k][i].ToString();
                                    if (!add.Contains(strenum))
                                    {
                                        add += $"{strenum}:{enumCount},";
                                        enumCount++;
                                    }
                                }
                                add = add.Substring(0, add.Length - 1);
                            }
                        }
                        else if (tempRow0.ToLower() == "name")
                        {
                            dataName = UpperFirstLetter(result.Tables[index].Rows[j][i].ToString());
                            if (dataType == "dic")
                            {
                                string[] tempStr3 = dataName.Split(':');
                                dataName = tempStr3[0];
                                kv_key = tempStr3[1];
                            }
                        }
                        else if (tempRow0 == "")
                        {
                            if (startIndex == 0)
                                startIndex = j;
                            continue;
                        }
                    }

                    if (dataType == "enum")
                    {
                        Dictionary<string, int> tempStrs = new Dictionary<string, int>();

                        string[] strstemp1 = add.Split(',');
                        foreach (string tempStr in strstemp1)
                        {
                            string[] strstemp2 = tempStr.Split(':');
                            tempStrs.Add(strstemp2[0], int.Parse(strstemp2[1]));
                        }
                        enumDic.Add(dataName, tempStrs);

                        string tempEnumStr = enumStr.Replace("Type", dataName);

                        foreach (var it in tempStrs)
                        {
                            tempEnumStr += $@"            {UpperFirstLetter(it.Key)},
";
                        }
                        tempEnumStr += @"        }
";
                        //tempVal += tempEnumStr;

                        if (!Config.I.enumTypeList.Contains(dataName))
                        {
                            Config.I.enumTypeList.Add(dataName);
                            tempEnumStr = FileTool.ReadString($"{classPath}EnumType.cs") + tempEnumStr;
                            FileTool.WriteString($"{classPath}EnumType.cs", tempEnumStr);
                        }

                        string enumStr2 = @"
        public dataType dataName{ get; private set; }
";
                        enumStr2 = enumStr2.Replace("dataType", "Enum" + dataName);
                        enumStr2 = enumStr2.Replace("dataName", dataName);

                        tempVal += enumStr2;
                    }
                    else if (dataType == "array")
                    {
                        string arrStr = @"
        public List<Type> arrName{ get; private set; }
";
                        arrStr = arrStr.Replace("Type", add);
                        arrStr = arrStr.Replace("arrName", dataName);
                        tempVal += arrStr;

                        if (add == "string")
                            arrayDic.Add(dataName, true);
                        else
                            arrayDic.Add(dataName, false);
                    }
                    else if (dataType == "dic")
                    {
                        if (dicDic.ContainsKey(dataName))
                        {
                            if (!dicDic[dataName].Contains(kv_key))
                                dicDic[dataName].Add(kv_key);

                            dataNameDic.Add(i, dataName);
                            dataTypeDic.Add(i, dataType);
                            continue;
                        }
                        string dicStr = @"
        public Dictionary<key,value> dicName{ get; private set; }
";
                        string[] str1s = add.ToString().Split(',');
                        dicStr = dicStr.Replace("key", str1s[0]);
                        dicStr = dicStr.Replace("value", str1s[1]);
                        dicStr = dicStr.Replace("dicName", dataName);
                        tempVal += dicStr;

                        dicDic.Add(dataName, new List<string>());
                        dicDic[dataName].Add(kv_key);
                        dicTypeDic[dataName] = str1s[1] == "string";
                    }
                    else if (dataType.ToLower() == "vector3")
                    {
                        string vecStr = @"
        public Vector3 name{ get; private set; }
";
                        vecStr = vecStr.Replace("name", dataName);
                        tempVal += vecStr;
                    }
                    else if (dataType.ToLower() == "vector2")
                    {
                        string vecStr = @"
        public Vector2 name{ get; private set; }
";
                        vecStr = vecStr.Replace("name", dataName);
                        tempVal += vecStr;
                    }
                    else if (dataType.ToLower() == "color")
                    {
                        string colorStr = @"
        public Color name{ get; private set; }
";
                        colorStr = colorStr.Replace("name", dataName);
                        tempVal += colorStr;
                    }
                    else if (dataType.ToLower() == "int" ||
                        dataType.ToLower() == "float" ||
                        dataType.ToLower() == "string" ||
                        dataType.ToLower() == "bool")
                    {
                        string tempPropertyStr = propertyStr;
                        tempPropertyStr = tempPropertyStr.Replace("dataType", dataType);
                        tempPropertyStr = tempPropertyStr.Replace("dataName", dataName);

                        tempVal += tempPropertyStr;
                    }
                    else if (dataType != "")
                    {
                        Logger.Error($@"====Error====
Path:{path}
Sheet:{className},Columns:{columns},Rows:{2}
ErrorType:{dataType}
");
                    }

                    dataNameDic.Add(i, dataName);
                    dataTypeDic.Add(i, dataType);
                }

                tempVal += parseStr;

                FileTool.WriteString($"{classPath}{className}.cs", tempVal);


                for (int i = 3; i < rows; ++i)
                {
                    Dictionary<string, bool> dicAddedDic = new Dictionary<string, bool>();
                    foreach (var it in dicTypeDic)
                    {
                        dicAddedDic[it.Key] = false;
                    }

                    for (int j = 1; j < columns; ++j)
                    {
                        try
                        {
                            AddData(result.Tables[index].Rows[i][j].ToString(), dataNameDic[j], dataTypeDic[j], enumDic, arrayDic, dicDic, dicTypeDic, dicAddedDic);
                        }
                        catch
                        {
                            Logger.Error($@"Path:{path}
Line:Rows--{i}Columns--{j}");
                        }
                    }

                    dataStr = dataStr.Substring(0, dataStr.Length - 1);
                    dataStr += "},{";
                }

                SaveData(dataStr, className);
                Logger.Log($"{className}.cs Created");
            }
        }

        private void InitClassStr()
        {
            classStr = @"using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dadabase
{
    [Serializable]
    public class className
    {
";
            propertyStr = @"
        public dataType dataName{ get;private set; }
";

            enumStr = @"
        public enum EnumType
        {
";

            parseStr = @"
        public void Parse(string data)
        {
        
        }
    }
}";
        }

        //首字母大写
        private string UpperFirstLetter(string value)
        {
            //开头是一个字母或者空格后的第一个字母
            return Regex.Replace(value, @"\b(\w)|\s(\w)", match =>
            {
                return match.Value.ToUpper();
            });
        }

        private string LowerFirstLetter(string value)
        {
            //开头是一个字母或者空格后的第一个字母
            return Regex.Replace(value, @"\b(\w)|\s(\w)", match =>
            {
                return match.Value.ToLower();
            });
        }

        private static DataSet OpenExcel(string path)
        {
            using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
                DataSet result = excelDataReader.AsDataSet();
                return result;
            }
        }

        public void Test(string path)
        {
            DataSet result = OpenExcel(path);
            Logger.Log(result.Tables[0].Rows[2][4].ToString());
            Logger.Log(result.Tables[0].Rows[3][4].ToString());
            Logger.Log(result.Tables[0].Rows[4][4].ToString());
        }

        private void AddData(string data, string name, string type, Dictionary<string, Dictionary<string, int>> enumDic, Dictionary<string, bool> arrayDic, Dictionary<string, List<string>> dicDic, Dictionary<string, bool> dicTypeDic, Dictionary<string, bool> dicAddedDic)
        {
            switch (Config.I.exportType)
            {
                case Config.ExportType.Json: AddJsonData(data, name, type, enumDic, arrayDic, dicDic, dicTypeDic, dicAddedDic); break;
                case Config.ExportType.Bytes: AddBytesData(data); break;
                case Config.ExportType.Protobuf: AddProtobufData(data); break;
            }
        }

        private void AddJsonData(string data, string name, string type, Dictionary<string, Dictionary<string, int>> enumDic, Dictionary<string, bool> arrayDic, Dictionary<string, List<string>> dicDic, Dictionary<string, bool> dicTypeDic, Dictionary<string, bool> dicAddedDic)
        {
            if (type == "int" || type == "float" || type == "bool")
            {
                if (type == "bool")
                {
                    if (data == "0")
                        data = "false";
                    else
                        data = "true";
                }
                dataStr += $"\"{name}\":{data},";
            }
            else if (type == "string")
            {
                dataStr += $"\"{name}\":\"{data}\",";
            }
            else if (type == "enum")
            {
                dataStr += $"\"{name}\":{enumDic[name][data]},";
            }
            else if (type == "array")
            {
                string[] arrStrs = data.Split(',');
                string tempStr11 = "";
                for (int i = 0; i < arrStrs.Length; i++)
                {
                    if (!arrayDic[name])
                        tempStr11 += arrStrs[i] + ",";
                    else
                        tempStr11 += $"\"{arrStrs[i]}\",";
                }
                tempStr11 = tempStr11.Substring(0, tempStr11.Length - 1);

                dataStr += $"\"{name}\":[{tempStr11}],";
            }
            else if (type == "color")
            {
                string[] tempStrs = data.Split(',');
                string tempStr11 = "";
                tempStr11 = $"{{\"r\":{float.Parse(tempStrs[0])},\"g\":{float.Parse(tempStrs[1])},\"b\":{float.Parse(tempStrs[2])},\"a\":{float.Parse(tempStrs[3])}}},";
                dataStr += $"\"{name}\":{tempStr11}";
            }
            else if (type == "vector2")
            {
                string[] tempStrs = data.Split(',');
                string tempStr11 = "";
                tempStr11 = $"{{\"x\":{float.Parse(tempStrs[0])},\"y\":{float.Parse(tempStrs[1])}}},";
                dataStr += $"\"{name}\":{tempStr11}";
            }
            else if (type == "vector3")
            {
                string[] tempStrs = data.Split(',');
                string tempStr11 = "";
                tempStr11 = $"{{\"x\":{float.Parse(tempStrs[0])},\"y\":{float.Parse(tempStrs[1])},\"z\":{float.Parse(tempStrs[2])}}},";
                dataStr += $"\"{name}\":{tempStr11}";
            }
            else if (type == "dic")
            {
                if (!dicAddedDic[name])
                {
                    dicAddedDic[name] = true;
                    if (dicTypeDic[name])
                    {
                        dataStr += $"\"{name}\":{{\"{dicDic[name][0]}\":\"{data}\",";
                    }
                    else
                    {
                        dataStr += $"\"{name}\":{{\"{dicDic[name][0]}\":{data},";
                    }
                    dicIndex++;
                }
                else
                {
                    if (dicTypeDic[name])
                    {
                        dataStr += $"\"{dicDic[name][dicIndex]}\":\"{data}\",";
                    }
                    else
                    {
                        dataStr += $"\"{dicDic[name][dicIndex]}\":{data},";
                    }
                    dicIndex++;
                    if (dicIndex == dicDic[name].Count)
                    {
                        dataStr = dataStr.Substring(0, dataStr.Length - 1);
                        dataStr += "},";
                        dicIndex = 0;
                    }
                }
            }
        }

        private void AddBytesData(string data)
        {

        }

        private void AddProtobufData(string data)
        {

        }

        private void SaveData(string dataStr, string className)
        {
            switch (Config.I.exportType)
            {
                case Config.ExportType.Json:
                    dataStr = "[{" + dataStr.Substring(0, dataStr.Length - 2) + "]";
                    FileTool.WriteString($"{Config.I.dataPath}/Json/{className}.json", dataStr);
                    break;
                case Config.ExportType.Bytes: FileTool.WriteString($"{Config.I.dataPath}{className}", dataStr); break;
                case Config.ExportType.Protobuf: FileTool.WriteString($"{Config.I.dataPath}{className}.proto", dataStr); break;
            }
        }

    }
}
