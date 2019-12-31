using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gin
{
    class Program
    {
        public static Random rng = new Random();

        static void Main()
        {
            Initialize();
            Menu.NewGame();
        }

        private static void Initialize()
        {
            Console.Title = "Gin";
            Console.CursorVisible = false;
            Console.WindowWidth = 106;
            Console.Write(Console.WindowHeight);
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            Screen.oldScreenSize = (Console.WindowWidth, Console.WindowHeight);
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }
    }
}
