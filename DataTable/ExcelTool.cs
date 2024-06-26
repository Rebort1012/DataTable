﻿using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace PerillaTable
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
        private string parseBytes;
        private string enumStr;

        private const char split01 = '~';
        private const char split02 = ';';
        private const char split03 = '}';

        string className = "";

        Dictionary<string, Dictionary<string, int>> enumTypesDic = new Dictionary<string, Dictionary<string, int>>();
        Dictionary<string, Dictionary<string, Int16>> enumTypesDicByte = new Dictionary<string, Dictionary<string, Int16>>();

        public void CreatEnumData()
        {
            string path = Config.I.excelPath + "enum.xlsx";
            string classPath = Config.I.classPath;
            List<DataTable> result = ExcelToDataTable(path);

            DataTable curSheet = result[0];

            int columns = curSheet.Columns.Count;
            int rows = curSheet.Rows.Count;

            for (int j = 0; j < columns; ++j)
            {
                string enumStrTemp = enumStr;
                string enumName = curSheet.Rows[0][j].ToString();
                enumTypesDic.Add(enumName.ToLower(), new Dictionary<string, int>());
                enumTypesDicByte.Add(enumName.ToLower(), new Dictionary<string, Int16>());
                int count = 0;
                Int16 countbyte = 0;
                enumStrTemp = enumStrTemp.Replace("Type", UpperFirstLetter(enumName));

                for (int i = 1; i < rows; ++i)
                {
                    if (curSheet.Rows[i][j].ToString() == "")
                        break;
                    string Type = curSheet.Rows[i][j].ToString();
                    enumTypesDic[enumName.ToLower()].Add(Type.ToLower(), count++);
                    enumTypesDicByte[enumName.ToLower()].Add(Type.ToLower(), (Int16)countbyte++);

                    enumStrTemp += $@"    {UpperFirstLetter(Type)},
";
                }

                enumStrTemp += @"}";
                string data = File.ReadAllText(classPath + "EnumType.cs");
                data += enumStrTemp;
                File.WriteAllText(classPath + "EnumType.cs", data);
            }
        }


        public void CreateDataTable(string path)
        {
            string classPath = Config.I.classPath;
            List<DataTable> result = ExcelToDataTable(path);
            if (result == null)
            {
                Logger.Log($"ExcelToDataTable:{path} NULL");
                return;
            }

            int tableNum = result.Count;
            for (int index = 0; index < tableNum; index++)
            {
                DataTable curSheet = result[index];

                //excel中包含多个sheet,sheet名为类型，忽略sheet名中包含"sheet"的表单;
                if (tableNum > 1)
                {
                    className = curSheet.Rows[0][0].ToString().Split('#')[1];
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
                string parseBystrStr = parseStr;
                string parseBytesStr = parseBytes;
                cshapClassStr = cshapClassStr.Replace("className", className);

                string parseByStr = "";
                string parseByBytes = "";

                #region 生成C#类
                //第一行类型
                for (int i = 0; i < columns; ++i)
                {
                    if (curSheet.Rows[0][i].ToString().StartsWith("#"))
                        continue;

                    string dataType = "";
                    string dataName = "";
                    string add = "";

                    for (int j = 1; j < 3; ++j)
                    {
                        string tempRow0 = curSheet.Rows[j][0].ToString();
                        switch (tempRow0.ToLower())
                        {
                            case "#type":
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
                            case "#name":
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
                            Dictionary<string, int> tempStrs = new Dictionary<string, int>();

                            string[] strstemp1 = add.Split(',');
                            foreach (string tempStr in strstemp1)
                            {
                                string[] strstemp2 = tempStr.Split(':');
                                tempStrs.Add(strstemp2[0], int.Parse(strstemp2[1]));
                            }

                            string enumStr2 = @"
        public dataType dataName{ get; protected set; }
";
                            enumStr2 = enumStr2.Replace("dataType", "Enum" + dataName);
                            enumStr2 = enumStr2.Replace("dataName", dataName);

                            cshapClassStr += enumStr2;

                            parseByStr += $@"
            {dataName} = (Enum{dataName})int.Parse(tempStrs[count++]);";
                            parseByBytes += $@"
            {dataName} = (Enum{dataName})rd.ReadInt16();";

                            break;
                        case "array":

                            string arrStr = @"
        public List<Type> arrName{ get; protected set; }
";
                            arrStr = arrStr.Replace("Type", add);
                            arrStr = arrStr.Replace("arrName", dataName);

                            cshapClassStr += arrStr;

                            string typeStr = "str";
                            if (add == "int")
                                typeStr = "int.Parse(value)";
                            else if (add == "float")
                                typeStr = "float.Parse(value)";
                            else if (add == "bool")
                                typeStr = "int.Parse(value) != 0";

                            typeStr = typeStr.Replace("value", "str");

                            string tempArr = "string[] tempArr";
                            if (parseByStr.Contains(tempArr))
                                tempArr = "tempArr";
                            parseByStr += $@"
            {tempArr} = tempStrs[count++].Split(',');
            foreach(string str in tempArr)
            {{
                {dataName}.Add({typeStr});
            }};";


                            typeStr = "rd.ReadString()";
                            if (add == "int")
                                typeStr = "rd.ReadInt32()";
                            else if (add == "float")
                                typeStr = "rd.ReadSingle()";
                            else if (add == "bool")
                                typeStr = "rd.ReadBoolean()";

                            typeStr = typeStr.Replace("value", "str");

                            string countStr = "int count";
                            if (parseByBytes.Contains(countStr))
                                countStr = "count";

                            parseByBytes += $@"
            {countStr} = rd.ReadInt16();
            {dataName} = new List<{add}>();
            for(int i = 0; i < count; i++)
            {{
                {dataName}.Add({typeStr});
            }}
            ";
                            break;
                        case "dic":
                            {
                                string dicStr = @"
        public Dictionary<key,value> dicName{ get; protected set; }
";
                                string[] str1s = add.ToString().Split(',');
                                dicStr = dicStr.Replace("key", str1s[0]);
                                dicStr = dicStr.Replace("value", str1s[1]);
                                dicStr = dicStr.Replace("dicName", dataName.Split(':')[0]);

                                if (!cshapClassStr.Contains(dicStr))
                                    cshapClassStr += dicStr;
                                break;
                            }
                        default:
                            {
                                if (dataType.ToLower() == "vector3")
                                {
                                    string vecStr = @"
        public Vector3 name{ get; protected set; }
";
                                    vecStr = vecStr.Replace("name", dataName);
                                    cshapClassStr += vecStr;

                                    tempArr = "string[] tempArr";
                                    if (parseByStr.Contains(tempArr))
                                        tempArr = "tempArr";
                                    parseByStr += $@"
            {tempArr} = tempStrs[count++].Split(',');
            {dataName} = new Vector3(float.Parse(tempArr[0]),float.Parse(tempArr[1]),float.Parse(tempArr[2]));";

                                    parseByBytes += $@"
            {dataName} = new Vector3(rd.ReadSingle(),rd.ReadSingle(),rd.ReadSingle());";
                                }
                                else if (dataType.ToLower() == "vector2")
                                {
                                    string vecStr = @"
        public Vector2 name{ get; protected set; }
";
                                    vecStr = vecStr.Replace("name", dataName);
                                    cshapClassStr += vecStr;

                                    tempArr = "string[] tempArr";
                                    if (parseByStr.Contains(tempArr))
                                        tempArr = "tempArr";
                                    parseByStr += $@"
            {tempArr} = tempStrs[count++].Split(',');
            {dataName} = new Vector2(float.Parse(tempArr[0]),float.Parse(tempArr[1]));";

                                    parseByBytes += $@"
            {dataName} = new Vector2(rd.ReadSingle(),rd.ReadSingle());";
                                }
                                else if (dataType.ToLower() == "color")
                                {
                                    string colorStr = @"
        public Color name{ get; protected set; }
";
                                    colorStr = colorStr.Replace("name", dataName);
                                    cshapClassStr += colorStr;

                                    tempArr = "string[] tempArr";
                                    if (parseByStr.Contains(tempArr))
                                        tempArr = "tempArr";
                                    parseByStr += $@"
            {tempArr} = tempStrs[count++].Split(',');
            {dataName} = new Color(float.Parse(tempArr[0]),float.Parse(tempArr[1]),float.Parse(tempArr[2]),float.Parse(tempArr[3]));";

                                    parseByBytes += $@"
            {dataName} = new Color(rd.ReadSingle(),rd.ReadSingle(),rd.ReadSingle(),rd.ReadSingle());";
                                }
                                else if (dataType.ToLower() == "int" ||
                                         dataType.ToLower() == "float" ||
                                         dataType.ToLower() == "string" ||
                                         dataType.ToLower() == "bool")
                                {
                                    if (dataName.ToLower() != "id")
                                    {
                                        string tempPropertyStr = propertyStr;
                                        tempPropertyStr = tempPropertyStr.Replace("dataType", dataType);
                                        tempPropertyStr = tempPropertyStr.Replace("dataName", dataName);
                                        cshapClassStr += tempPropertyStr;
                                    }

                                    if (dataType == "int" || dataType == "float")
                                    {

                                        parseByStr += $@"
            {dataName} = {dataType}.Parse(tempStrs[count++]);";
                                        if (dataType == "int")
                                            parseByBytes += $@"
            {dataName} = rd.ReadInt32();";
                                        else
                                            parseByBytes += $@"
            {dataName} = rd.ReadSingle();";


                                    }
                                    else if (dataType == "string")
                                    {
                                        parseByStr += $@"
            {dataName} = tempStrs[count++];";
                                        parseByBytes += $@"
            {dataName} = rd.ReadString();";
                                    }
                                    else if (dataType == "bool")
                                    {
                                        parseByStr += $@"
            {dataName} = int.Parse(tempStrs[count++]) != 0;";
                                        parseByBytes += $@"
            {dataName} = rd.ReadBoolean();";
                                    }
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

                parseBystrStr = parseBystrStr.Replace("value", parseByStr);
                parseBytesStr = parseBytesStr.Replace("value", parseByBytes);
                cshapClassStr += parseBystrStr;
                cshapClassStr += parseBytesStr;

                FileTool.WriteString($"{classPath}D{className}.cs", cshapClassStr);
                #endregion

                #region 生成数据文件
                CreateData(curSheet, rows, columns);

                Logger.Log($"{className}.cs Created");
                #endregion
            }
        }

        //自定义c#类
        private void InitClassStr()
        {
            classStr = @"using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Database
{
    [Serializable]
    public class DclassName: DataItem
    {
";
            propertyStr = @"
        public dataType dataName{ get;protected set; }
";

            enumStr = @"
public enum EnumType
{
";

            parseStr = @"
        public override void ParseByString(string data)
        {
            string[] tempStrs = data.Split('-');
            int count = -1;value
        }
";

            parseBytes = @"
        public override void ParseByBytes(MemoryStream ms)
        {
            BinaryReader rd = new BinaryReader(ms); value        
        }
    }
}   
";
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

        public static List<DataTable> ExcelToDataTable(string fileName)
        {
            IWorkbook workbook = null;
            ISheet sheet = null;

            List<DataTable> dataList = new List<DataTable>();
            int startRow = 0;
            string tempPath = $"./{fileName.Split('/')[^1].Replace(".xlsx", "")}.xlsx";
            if (File.Exists(tempPath))
                File.Delete(tempPath);
            File.Copy(fileName, tempPath);
            var fs = new FileStream(tempPath, FileMode.Open, FileAccess.Read);

            if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                workbook = new XSSFWorkbook(fs);
            else if (fileName.IndexOf(".xls") > 0) // 2003版本
                workbook = new HSSFWorkbook(fs);
            fs.Close();
            File.Delete(tempPath);

            try
            {
                for (int i = 0; i < workbook.NumberOfSheets; ++i)
                {
                    sheet = workbook.GetSheetAt(i);
                    if (sheet.SheetName.ToLower().Contains("sheet") && workbook.NumberOfSheets != 1)
                        continue;

                    DataTable data = new DataTable();
                    IRow firstRow = sheet.GetRow(1);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    for (int j = firstRow.FirstCellNum; j < cellCount; ++j)
                    {
                        ICell cell = firstRow.GetCell(j);
                        if (cell != null)
                        {
                            if (cell.CellType == CellType.Formula)
                                cell.SetCellType(CellType.String);

                            string cellValue = cell.StringCellValue;

                            if (cellValue != null)
                            {
                                DataColumn column = new DataColumn(cellValue);
                                data.Columns.Add(column);
                            }
                        }
                    }


                    startRow = sheet.FirstRowNum;

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int j = startRow; j <= rowCount; ++j)
                    {
                        IRow row = sheet.GetRow(j);
                        if (row == null)
                            continue; //没有数据的行默认是null　　　　　　　
                        DataRow dataRow = data.NewRow();
                        for (int k = row.FirstCellNum; k < cellCount; ++k)
                        {
                            if (row.GetCell(k) != null) //同理，没有数据的单元格都默认是null
                            {

                                if (row.GetCell(k).CellType == CellType.Formula)
                                    row.GetCell(k).SetCellType(CellType.String);
                                dataRow[k] = row.GetCell(k).ToString();
                            }
                        }

                        data.Rows.Add(dataRow);
                    }

                    dataList.Add(data);
                }
                return dataList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:{0}, datalistCount{1}", ex.Message, dataList.Count);
                return null;
            }
        }

        private void SaveData(string dataStr, string className, Config.ExportType exportType)
        {
            switch (exportType)
            {
                case Config.ExportType.Json:
                    FileTool.WriteString($"{Config.I.dataPath}/Json/{className}.json", dataStr);
                    break;
                case Config.ExportType.Bytes: FileTool.WriteString($"{Config.I.dataPath}{className}", dataStr); break;
                case Config.ExportType.Protobuf: FileTool.WriteString($"{Config.I.dataPath}{className}.proto", dataStr); break;
            }
        }

        private void CreateData(DataTable curSheet, int rows, int columns)
        {
            foreach (var enumType in Config.I.exportList)
            {
                if (enumType == Config.ExportType.Json)
                    CreateJson(curSheet, rows, columns);

                else if (enumType == Config.ExportType.Bytes)
                    CreateBytes(curSheet, rows, columns);
            }
        }

        private string CreateJson(DataTable curSheet, int rows, int columns)
        {
            string rowData = "";
            if (rows <= 3)
                return null;

            for (int i = 3; i < rows; ++i)
            {
                if (curSheet.Rows[i][0].ToString().StartsWith("#"))
                    continue;

                for (int j = 1; j < columns; ++j)
                {
                    if (string.IsNullOrEmpty(curSheet.Rows[2][j].ToString()))
                        continue;

                    rowData += $"{curSheet.Rows[2][j]}{split01}{curSheet.Rows[1][j]}{split01}{curSheet.Rows[i][j]}{split02}";
                }
                rowData = rowData.Substring(0, rowData.Length - 1);
                rowData += split03;
            }
            rowData = rowData.Substring(0, rowData.Length - 1);


            string jsonData = "[";
            string[] rowStrs = rowData.Split(split03);
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
                                if (string.IsNullOrEmpty(eachStrs[2]))
                                    eachStrs[2] = "0";
                                jsonData += $"\"{UpperFirstLetter(eachStrs[1])}\":{int.Parse(eachStrs[2])}";
                                break;
                            case "float":
                                if (string.IsNullOrEmpty(eachStrs[2]))
                                    eachStrs[2] = "0";
                                jsonData += $"\"{UpperFirstLetter(eachStrs[1])}\":{float.Parse(eachStrs[2])}";
                                break;
                            case "bool":
                                if (string.IsNullOrEmpty(eachStrs[2]))
                                    eachStrs[2] = "FALSE";
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
                                tempStrs = eachStrs[2].Split(',');
                                eachStrs[2] = "";
                                foreach (var str in tempStrs)
                                {
                                    if (str == "")
                                        eachStrs[2] += $"{0},";
                                    else
                                        eachStrs[2] += $"{str},";
                                }
                                eachStrs[2] = eachStrs[2].Substring(0, eachStrs[2].Length - 1);
                                jsonData += $"\"{UpperFirstLetter(eachStrs[1])}\":[{eachStrs[2].ToLower()}]";
                                break;
                            case "float[]":
                                tempStrs = eachStrs[2].Split(',');


                                eachStrs[2] = "";
                                foreach (var str in tempStrs)
                                {
                                    if (str == "")
                                        eachStrs[2] += $"{0},";
                                    else
                                        eachStrs[2] += $"{str},";
                                }
                                eachStrs[2] = eachStrs[2].Substring(0, eachStrs[2].Length - 1);
                                jsonData += $"\"{UpperFirstLetter(eachStrs[1])}\":[{eachStrs[2].ToLower()}]";
                                break;
                            case "bool[]":
                                if (eachStrs[2] == "")
                                    eachStrs[2] = "FALSE";
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
                                jsonData += $"\"{UpperFirstLetter(eachStrs[1])}\":{enumTypesDic[eachStrs[1].ToLower()][eachStrs[2].ToLower()]}";
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

            FileTool.WriteString($"{Config.I.dataPath}/Json/D{className}.json", jsonData);
            return jsonData;
        }

        private void CreateBytes(DataTable curSheet, int rows, int columns)
        {
            if (rows <= 3)
                return;

            using (MemoryStream ms = new MemoryStream())
            {
                int dataCount = -1;
                Dictionary<int, long> dataInfos = new Dictionary<int, long>();

                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    for (int i = 3; i < rows; ++i)
                    {
                        if (curSheet.Rows[i][0].ToString().StartsWith("#"))
                            continue;
                        dataCount++;
                        dataInfos.Add(dataCount, bw.BaseStream.Position);

                        long bwLen = bw.BaseStream.Length;
                        for (int j = 1; j < columns; ++j)
                        {
                            if (string.IsNullOrEmpty(curSheet.Rows[2][j].ToString()))
                                continue;

                            string type = curSheet.Rows[2][j].ToString().ToLower();
                            string value = curSheet.Rows[i][j].ToString();
                            switch (type)
                            {
                                case "int":
                                    if (string.IsNullOrEmpty(value))
                                        value = "0";
                                    bw.Write(Convert.ToInt32(value)); break;
                                case "float":
                                    if (string.IsNullOrEmpty(value))
                                        value = "0";
                                    bw.Write(Convert.ToSingle(value)); break;
                                case "string":
                                    bw.Write(value); break;
                                case "bool":
                                    if (string.IsNullOrEmpty(value))
                                        value = "FALSE";
                                    bw.Write(Convert.ToBoolean(value)); break;
                                case "vector3":
                                    string[] tempArr = value.Split(',');
                                    bw.Write(Convert.ToSingle(tempArr[0]));
                                    bw.Write(Convert.ToSingle(tempArr[1]));
                                    bw.Write(Convert.ToSingle(tempArr[2]));
                                    break;
                                case "color":
                                    tempArr = value.Split(',');
                                    bw.Write(Convert.ToSingle(tempArr[0]));
                                    bw.Write(Convert.ToSingle(tempArr[1]));
                                    bw.Write(Convert.ToSingle(tempArr[2]));
                                    bw.Write(Convert.ToSingle(tempArr[3]));
                                    break;
                                case "vector2":
                                    tempArr = value.Split(',');
                                    bw.Write(Convert.ToSingle(tempArr[0]));
                                    bw.Write(Convert.ToSingle(tempArr[1]));
                                    break;
                                case "enum":
                                    bw.Write(enumTypesDicByte[curSheet.Rows[1][j].ToString().ToLower()][value.ToLower()]);
                                    break;
                            }

                            if (type.EndsWith("[]"))
                            {
                                string[] tempArr = value.Split(',');
                                Int16 count = (Int16)tempArr.Length;
                                bw.Write(count);
                                foreach (string str in tempArr)
                                {
                                    switch (type.Substring(0, type.Length - 2))
                                    {
                                        case "int":
                                            if (str != "")
                                                bw.Write(Convert.ToInt32(str));
                                            else
                                                bw.Write(0);
                                            break;
                                        case "float":
                                            if (str != "")
                                                bw.Write(Convert.ToSingle(str));
                                            else
                                                bw.Write(0);
                                            break;
                                        case "bool":
                                            if (str != "")
                                                bw.Write(Convert.ToBoolean(str));
                                            else
                                                bw.Write(false);
                                            break;
                                        case "string": bw.Write(str); break;
                                        default: Logger.Error("no type:" + type); break;
                                    }
                                }
                            }
                        }
                    }

                    long pos = bw.BaseStream.Position;
                    foreach (var kv in dataInfos)
                    {
                        bw.Write(kv.Key);
                        bw.Write(kv.Value);
                    }
                    bw.Write(bw.BaseStream.Position - pos);

                    FileTool.WriteBytes($"{Config.I.dataPath}/Bytes/D{className}.bytes", ms);
                }
            }
        }
    }
}
