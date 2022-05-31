using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTable
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ExcelTool excelTool = new ExcelTool();

            FileTool fileTool = new FileTool();
            fileTool.GetAllFiles(Config.I.excelPath);

            foreach (var item in fileTool.fileList)
            {
                if (item.Contains(".xls"))
                    excelTool.CreateDataTable(item);
            }

            Logger.Log("Press Any Key Exit!");
            Console.ReadKey();
        }
    }
}

