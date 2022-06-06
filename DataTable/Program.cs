using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTable
{
    static class Program
    {
        static void Main(string[] args)
        {
            Logger.Log(ExcelTool.OpenExcel("./Excel/Skill.xlsx").Tables[0].Rows[12][10].ToString());
            Logger.Log(ExcelTool.OpenExcel("./Excel/Skill.xlsx").Tables[0].Rows[12][9].ToString());
            Logger.Log(ExcelTool.OpenExcel("./Excel/Skill.xlsx").Tables[0].Rows[12][11].ToString());
            Logger.Log(ExcelTool.OpenExcel("./Excel/Skill.xlsx").Tables[0].Rows[13][10].ToString());
            Logger.Log(ExcelTool.OpenExcel("./Excel/Skill.xlsx").Tables[0].Rows[12][8].ToString());

            Console.ReadKey();
            return;

            DateTime time = DateTime.Now.ToUniversalTime();

            ExcelTool excelTool = new ExcelTool();
            FileTool fileTool = new FileTool();

            fileTool.GetAllFiles(Config.I.excelPath);

            for (int i = 0; i < fileTool.fileList.Count; i++)
            {
                string item = fileTool.fileList[i];
                if (item.Contains(".xls"))
                    excelTool.CreateDataTable(item);
            }

            Logger.Log("TotalSeconds:" + (DateTime.Now.ToUniversalTime() - time).TotalSeconds);
            Logger.Log("Press Any Key Exit!");
            Console.ReadKey();



        }
    }
}

