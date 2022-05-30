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
            excelTool.CreateDataTable("./Excel/Hero.xlsx");

            //excelTool.Test("./Excel/Hero.xlsx");

            Logger.Log("Press Any Key Out!");
            Console.ReadKey();
        }
    }
}
