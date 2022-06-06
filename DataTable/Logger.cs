using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerillaTable
{
    static class Logger
    {
        public static void Log(Object value)
        {
            Console.WriteLine(value);
        }

        public static void Error(Object value)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(value);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void LogCls(Object value)
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(value.ToString() + " ", Console.BufferWidth);
        }
    }
}
