using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerillaTable
{
    static class Program
    {
        static void Main(string[] args)
        {
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

