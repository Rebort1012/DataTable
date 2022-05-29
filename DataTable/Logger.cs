using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTable
{
    internal class Logger
    {
        public static void Log(string value)
        { 
            Console.WriteLine(value);
        }

        public static void Error(string value)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(value);
            Console.ForegroundColor = ConsoleColor.White;
        }

    }
}
