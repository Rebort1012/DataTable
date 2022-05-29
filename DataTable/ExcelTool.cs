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
        public class AutoClass
        {
            public AutoClass() { InitClassStr(); }

            private string classStr;
            private string propertyStr;
            private string parseStr;
            private string enumStr;

            private void InitClassStr()
            {
                classStr = @"
public class className
{
";
                propertyStr = @"
    public dataType dataName
    {
        get;
    }
";

                enumStr = @"
    public enum EnumType
    {
";

                parseStr = @"
    public void Parse(string data)
    {
        
    }
}"
;
            }

            //首字母大写
            public string UpperFirstLetter(string value)
            {
                //开头是一个字母或者空格后的第一个字母
                return Regex.Replace(value, @"\b(\w)|\s(\w)", match =>
                {
                    return match.Value.ToUpper();
                });
            }

            public void AutoSaveClass(string path,string savePath)
            {
                DataSet result = OpenExcel(path);

                int columns = result.Tables[0].Columns.Count;
                int rows = result.Tables[0].Rows.Count;

                //TODO:单个sheet====0
                //sheet名 = 类名
                string className = result.Tables[0].TableName;
                string tempVal = classStr;
                tempVal = tempVal.Replace("className", className);
                //第一行类型
                for (int i = 0; i < columns; ++i)
                {
                    string dataType = result.Tables[0].Rows[0][i].ToString();
                    string dataName = UpperFirstLetter(result.Tables[0].Rows[1][i].ToString());

                    if (dataType == "enum")
                    {
                        List<string> tempStrs = new List<string>();
                        for (int j = 2; j < rows; ++j)
                        {
                            string temp = result.Tables[0].Rows[j][i].ToString();
                            if (!tempStrs.Contains(temp))
                                tempStrs.Add(temp);
                        }
                        string tempEnumStr = enumStr.Replace("Type", dataName);

                        foreach (string it in tempStrs)
                        {
                            tempEnumStr += $@"        {it},
";
                        }
                        tempEnumStr += @"    }
";
                        tempVal += tempEnumStr;
                    }

                    if (dataType == "array")
                    {
                        string arrStr = @"
    public List<Type> arrName;
";

                        arrStr = arrStr.Replace("Type", result.Tables[0].Rows[2][i].ToString());
                        arrStr = arrStr.Replace("arrName", dataName);

                        tempVal += arrStr;
                    }
                    else
                    {
                        string tempPropertyStr = propertyStr;
                        tempPropertyStr = tempPropertyStr.Replace("dataType", dataType);
                        tempPropertyStr = tempPropertyStr.Replace("dataName", dataName);

                        tempVal += tempPropertyStr;
                    }
                }

                tempVal += parseStr;

                FileTool.WriteString($"{savePath}{className}.cs", tempVal);
            }
        }

        public ExcelTool() { 
            autoClass = new AutoClass();
        }

        public AutoClass autoClass;

        private static DataSet OpenExcel(string path)
        {
            using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
                DataSet result = excelDataReader.AsDataSet();
                return result;
            }
        }
    }
}
