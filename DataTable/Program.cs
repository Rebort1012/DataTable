using Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerillaTable
{
    static class Program
    {


        static void Main(string[] args)
        {
            /*          ExcelTool excelTool = new ExcelTool();
                        FileTool fileTool = new FileTool();
                        Config.I.exportType = Config.ExportType.Bytes;

                        string path = "./Excel/Hero.xlsx";
                        excelTool.CreateDataTable(path);

                        DataLoader.GetData<Hero>("DataTable/Bytes/Hero.bytes", 4).ToString();

                        Console.ReadLine();

                        return;*/


            DateTime time = DateTime.Now.ToUniversalTime();
            ExcelTool excelTool = new ExcelTool();
            FileTool fileTool = new FileTool();

            fileTool.GetAllFiles(Config.I.excelPath);

            //Config.I.exportType = Config.ExportType.Bytes;
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
