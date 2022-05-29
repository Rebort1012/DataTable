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
            Config config = new Config();


            ExcelTool excelTool = new ExcelTool();
            excelTool.autoClass.AutoSaveClass("./Excel/Hero.xlsx", config.classPath);


            Console.ReadKey();
        }
    }
}
