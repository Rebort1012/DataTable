﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using Excel;


namespace DataTable
{
    class ExcelTool
    {
        public ExcelTool()
        {
            InitClassStr();
        }

        private string classStr;
        private string propertyStr;
        private string parseStr;
        private string enumStr;

        private const char split01 = '-';
        private const char split02 = ';';
        private const char split03 = '}';

        string className = "";
        public void CreateDataTable(string path)
        {
            string classPath = Config.I.classPath;
            DataSet result = OpenExcel(path);

            int tableNum = result.Tables.Count;
            for (int index = 0; index < tableNum; index++)
            {
                System.Data.DataTable curSheet = result.Tables[index];

                //excel中包含多个sheet,sheet名为类型，忽略sheet名中包含"sheet"的表单;
                if (tableNum > 1)
                {
                    if (curSheet.TableName.ToLower().Contains("sheet"))
                        continue;
                    className = curSheet.TableName;
                }
                //只有一个sheet的excel文件，文件名为类名;
                else
                {
                    string[] tempS11 = path.Split('/');
                    className = tempS11[tempS11.Length - 1].Split('.')[0];
                }

                int columns = curSheet.Columns.Count;
                int rows = curSheet.Rows.Count;

                //sheet名 = 类名
                string cshapClassStr = classStr;
                cshapClassStr = cshapClassStr.Replace("className", className);

                #region 生成C#类
                //第一行类型
                for (int i = 0; i < columns; ++i)
                {
                    if (curSheet.Rows[0][i].ToString().StartsWith("#"))
                        continue;

                    string dataType = "";
                    string dataName = "";
                    string add = "";

                    for (int j = 0; j < 3; ++j)
                    {
                        string tempRow0 = curSheet.Rows[j][0].ToString();

                        if (tempRow0.StartsWith("#"))
                            continue;
                        switch (tempRow0.ToLower())
                        {
                            case "type":
                                {
                                    dataType = curSheet.Rows[j][i].ToString();
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
                                        int enumCount = 0;
                                        for (int k = 4; k < rows; ++k)
                                        {
                                            string strenum = curSheet.Rows[k][i].ToString();
                                            if (!add.Contains(strenum))
                                            {
                                                add += $"{strenum}:{enumCount},";
                                                enumCount++;
                                            }
                                        }
                                        add = add.Substring(0, add.Length - 1);
                                    }

                                    break;
                                }
                            case "name":
                                {
                                    dataName = UpperFirstLetter(curSheet.Rows[j][i].ToString());
                                    if (dataType == "dic")
                                    {
                                        string[] tempStr3 = dataName.Split(':');
                                        dataName = tempStr3[0];
                                    }

                                    break;
                                }
                            default:
                                {
                                    if (tempRow0 == "")
                                    {
                                        continue;
                                    }
                                    break;
                                }
                        }
                    }

                    switch (dataType)
                    {
                        case "enum":
                            {
                                Dictionary<string, int> tempStrs = new Dictionary<string, int>();

                                string[] strstemp1 = add.Split(',');
                                foreach (string tempStr in strstemp1)
                                {
                                    string[] strstemp2 = tempStr.Split(':');
                                    tempStrs.Add(strstemp2[0], int.Parse(strstemp2[1]));
                                }


                                string tempEnumStr = enumStr.Replace("Type", dataName);

                                foreach (var it in tempStrs)
                                {
                                    tempEnumStr += $@"            {UpperFirstLetter(it.Key)},
";
                                }
                                tempEnumStr += @"        }
";

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

                                cshapClassStr += enumStr2;
                                break;
                            }
                        case "array":

                            string arrStr = @"
        public List<Type> arrName{ get; private set; }
";
                            arrStr = arrStr.Replace("Type", add);
                            arrStr = arrStr.Replace("arrName", dataName);
                            cshapClassStr += arrStr;


                            break;
                        case "dic":
                            {
                                string dicStr = @"
        public Dictionary<key,value> dicName{ get; private set; }
";
                                string[] str1s = add.ToString().Split(',');
                                dicStr = dicStr.Replace("key", str1s[0]);
                                dicStr = dicStr.Replace("value", str1s[1]);
                                dicStr = dicStr.Replace("dicName", dataName);

                                if (!cshapClassStr.Contains(dicStr))
                                    cshapClassStr += dicStr;
                                break;
                            }
                        default:
                            {
                                if (dataType.ToLower() == "vector3")
                                {
                                    string vecStr = @"
        public Vector3 name{ get; private set; }
";
                                    vecStr = vecStr.Replace("name", dataName);
                                    cshapClassStr += vecStr;
                                }
                                else if (dataType.ToLower() == "vector2")
                                {
                                    string vecStr = @"
        public Vector2 name{ get; private set; }
";
                                    vecStr = vecStr.Replace("name", dataName);
                                    cshapClassStr += vecStr;
                                }
                                else if (dataType.ToLower() == "color")
                                {
                                    string colorStr = @"
        public Color name{ get; private set; }
";
                                    colorStr = colorStr.Replace("name", dataName);
                                    cshapClassStr += colorStr;
                                }
                                else if (dataType.ToLower() == "int" ||
                                         dataType.ToLower() == "float" ||
                                         dataType.ToLower() == "string" ||
                                         dataType.ToLower() == "bool")
                                {
                                    string tempPropertyStr = propertyStr;
                                    tempPropertyStr = tempPropertyStr.Replace("dataType", dataType);
                                    tempPropertyStr = tempPropertyStr.Replace("dataName", dataName);

                                    cshapClassStr += tempPropertyStr;
                                }
                                else if (dataType != "")
                                {
                                    Logger.Error($@"====Error====
Path:{path}
Sheet:{className},Columns:{columns},Rows:{2}
ErrorType:{dataType}
");
                                }

                                break;
                            }
                    }
                }

                cshapClassStr += parseStr;
                FileTool.WriteString($"{classPath}{className}.cs", cshapClassStr);
                #endregion

                #region 生成数据文件
                string rowData = "";
                for (int i = 3; i < rows; ++i)
                {
                    if (curSheet.Rows[i][0].ToString() != "")
                        continue;

                    for (int j = 1; j < columns; ++j)
                    {
                        rowData += $"{curSheet.Rows[1][j]}{split01}{curSheet.Rows[2][j]}{split01}{curSheet.Rows[i][j]}{split02}";
                    }
                    rowData = rowData.Substring(0, rowData.Length - 1);
                    rowData += split03;
                }
                rowData = rowData.Substring(0, rowData.Length - 1);

                //记录枚举信息
                Dictionary<string, Dictionary<string, int>> enumTypesDic = new Dictionary<string, Dictionary<string, int>>();

                for (int j = 1; j < columns; ++j)
                {
                    if (curSheet.Rows[1][j].ToString() == "enum")
                    {
                        Dictionary<string, int> enumTemp = new Dictionary<string, int>();
                        int count = 0;
                        for (int k = 3; k < rows; ++k)
                        {
                            if (!enumTemp.ContainsKey(curSheet.Rows[k][j].ToString()))
                            {
                                enumTemp.Add(curSheet.Rows[k][j].ToString(), count);
                                count++;
                            }
                        }

                        enumTypesDic.Add(curSheet.Rows[2][j].ToString(), enumTemp);
                    }
                }

                CreateData(rowData, enumTypesDic);


                Logger.Log($"{className}.cs Created");
                #endregion
            }
        }

        private void InitClassStr()
        {
            classStr = @"using System;
using System.Collections.Generic;
using UnityEngine;

namespace Database
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
            return Regex.Replace(value, @"\b(\w)|\s(\w)", match => match.Value.ToUpper());
        }

        private string LowerFirstLetter(string value)
        {
            //开头是一个字母或者空格后的第一个字母
            return Regex.Replace(value, @"\b(\w)|\s(\w)", match => match.Value.ToLower());
        }

        public static DataSet OpenExcel(string path)
        {
            using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
                DataSet result = excelDataReader.AsDataSet();
                return result;
            }
        }

        private void SaveData(string dataStr, string className)
        {
            switch (Config.I.exportType)
            {
                case Config.ExportType.Json:
                    FileTool.WriteString($"{Config.I.dataPath}/Json/{className}.json", dataStr);
                    break;
                case Config.ExportType.Bytes: FileTool.WriteString($"{Config.I.dataPath}{className}", dataStr); break;
                case Config.ExportType.Protobuf: FileTool.WriteString($"{Config.I.dataPath}{className}.proto", dataStr); break;
            }
        }


        private void CreateData(string data, Dictionary<string, Dictionary<string, int>> enumTypesDic)
        {
            switch (Config.I.exportType)
            {
                case Config.ExportType.Json:
                    CreateJson(data, enumTypesDic);
                    break;
                case Config.ExportType.Bytes:
                    CreateBytes(data, enumTypesDic);
                    break;
            }
        }

        private string CreateJson(string data, Dictionary<string, Dictionary<string, int>> enumTypesDic)
        {
            string jsonData = "[";
            string[] rowStrs = data.Split(split03);
            int rowNum = 0;
            foreach (var rowStr in rowStrs)
            {
                string[] colStrs = rowStr.Split(split02);
                bool isDic = false;
                bool isFistDic = true;
                jsonData += "{";
                for (int i = 0; i < colStrs.Length; i++)
                {
                    string[] eachStrs = colStrs[i].Split(split01);

                    if (eachStrs[0].ToLower().StartsWith("dic"))
                    {
                        isDic = true;
                        eachStrs[0] = eachStrs[0].Split('<')[1].Split('>')[0];
                        string[] dicTypes = eachStrs[0].Split(',');
                        string[] dicDataStrs = eachStrs[1].Split(':');
                        if (isFistDic)
                        {
                            isFistDic = false;
                            jsonData += $"\"{UpperFirstLetter(dicDataStrs[0])}\":{{";
                        }

                        if (dicTypes[0] == "string")
                            jsonData += $"\"{UpperFirstLetter(dicDataStrs[1])}\":";
                        else
                            jsonData += $"{UpperFirstLetter(dicDataStrs[1])}:";

                        if (dicTypes[1] == "string")
                            jsonData += $"\"{eachStrs[2]}\",";
                        else
                            jsonData += $"{eachStrs[2]},";
                    }
                    else
                    {
                        if (isDic)
                        {
                            isDic = false;
                            isFistDic = true;
                            jsonData = jsonData.Substring(0, jsonData.Length - 1);
                            jsonData += "},";
                        }

                        switch (eachStrs[0].ToLower())
                        {
                            case "int":
                                jsonData += $"\"{UpperFirstLetter(eachStrs[1])}\":{ int.Parse(eachStrs[2])}";
                                break;
                            case "float":
                                jsonData += $"\"{UpperFirstLetter(eachStrs[1])}\":{float.Parse(eachStrs[2])}";
                                break;
                            case "bool":
                                jsonData += $"\"{UpperFirstLetter(eachStrs[1])}\":{eachStrs[2].ToLower()}";
                                break;
                            case "string":
                                jsonData += $"\"{UpperFirstLetter(eachStrs[1])}\":\"{eachStrs[2]}\"";
                                break;
                            case "string[]":
                                string[] tempStrs = eachStrs[2].Split(',');
                                eachStrs[2] = "";
                                foreach (var str in tempStrs)
                                {
                                    eachStrs[2] += $"\"{str}\",";
                                }
                                eachStrs[2] = eachStrs[2].Substring(0, eachStrs[2].Length - 1);
                                jsonData += $"\"{UpperFirstLetter(eachStrs[1])}\":[{eachStrs[2]}]";
                                break;
                            case "int[]":
                            case "float[]":
                            case "bool[]":
                                jsonData += $"\"{UpperFirstLetter(eachStrs[1])}\":[{eachStrs[2].ToLower()}]";
                                break;
                            case "vector3":
                                string[] tempStrs1 = eachStrs[2].Split(',');
                                jsonData +=
                                    $"\"{UpperFirstLetter(eachStrs[1])}\":{{\"x\":{float.Parse(tempStrs1[0])},\"y\":{float.Parse(tempStrs1[1])},\"z\":{float.Parse(tempStrs1[2])}}}";
                                break;
                            case "vector2":
                                string[] tempStrs2 = eachStrs[2].Split(',');
                                jsonData +=
                                    $"\"{UpperFirstLetter(eachStrs[1])}\":{{\"x\":{float.Parse(tempStrs2[0])},\"y\":{float.Parse(tempStrs2[1])}}}";
                                break;
                            case "color":
                                string[] tempStrs3 = eachStrs[2].Split(',');
                                jsonData +=
                                    $"\"{UpperFirstLetter(eachStrs[1])}\":{{\"r\":{float.Parse(tempStrs3[0])},\"g\":{float.Parse(tempStrs3[1])},\"b\":{float.Parse(tempStrs3[2])},\"a\":{float.Parse(tempStrs3[2])}}}";
                                break;
                            case "enum":
                                jsonData += $"\"{UpperFirstLetter(eachStrs[1])}\":{enumTypesDic[eachStrs[1]][eachStrs[2]]}";
                                break;
                            default:
                                Logger.Error($"wrong type int row:{rowNum}--column:{i}");
                                break;
                        }
                        jsonData += ",";
                    }
                }
                jsonData = jsonData.Substring(0, jsonData.Length - 1);
                jsonData += "},";
                rowNum++;
            }

            jsonData = jsonData.Substring(0, jsonData.Length - 1) + "]";

            SaveData(jsonData, className);
            return jsonData;
        }

        private void CreateBytes(string data, Dictionary<string, Dictionary<string, int>> enumTypesDic)
        {

        }
    }
}
