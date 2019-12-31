using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gin
{
    static class Menu
    {
        public static void NewGame()
        {
            Screen.Clear();

            Screen.cardBack = new CardBack(ConsoleColor.Black, ConsoleColor.Blue, '|');

            Screen.Clear();
            (int col, int row) = ((Console.WindowWidth / 2) - 12, (Console.WindowHeight / 2) - 2);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(col, row);
            Console.Write("                         ");
            Console.SetCursorPosition(col, row + 1);
            Console.Write("  Hit any key to begin.  ");
            Console.SetCursorPosition(col, row + 2);
            Console.Write("                         ");
            Console.ReadKey(true);
            Game.Play();
        }
    }
}
