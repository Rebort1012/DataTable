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
            DateTime time = DateTime.Now.ToUniversalTime();
            ExcelTool excelTool = new ExcelTool();
            FileTool fileTool = new FileTool();
            try
            {
                excelTool.CreatEnumData();
            }
            catch (Exception ex)
            {
                Logger.Error($"EnumError:{ex.Message}");
                Console.ReadKey();
            }

            fileTool.GetAllFiles(Config.I.excelPath);

            //Config.I.exportType = Config.ExportType.Bytes;
            for (int i = 0; i < fileTool.fileList.Count; i++)
            {
                string item = fileTool.fileList[i];

                if (item.Contains("Enum.xlsx"))
                    continue;
                if (item.Contains(".xls"))
                {
                    // try
                    {
                        excelTool.CreateDataTable(item);
                    }
                    /*                 catch (Exception ex)
                                     {
                                         Logger.Error(ex);
                                         Console.ReadKey();
                                     }*/
                }
            }

            Logger.Log("TotalSeconds:" + (DateTime.Now.ToUniversalTime() - time).TotalSeconds);
            Logger.Log("Press Any Key Exit!");
            Console.ReadKey();
        }
    }



}
