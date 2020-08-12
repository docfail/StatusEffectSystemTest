using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer_Test
{
    class Program
    {
        public static bool VERY_DEBUG = false;
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("BATTLE START!");
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Battle battle = new Battle(1,1);
            while (true)
            {
                battle.Round();
            }
        }
    }
}
