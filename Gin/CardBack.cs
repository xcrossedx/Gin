using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gin
{
    public class CardBack
    {
        public ConsoleColor fore;
        public ConsoleColor back;
        public char print;

        public CardBack(ConsoleColor fore, ConsoleColor back, char print)
        {
            this.fore = fore;
            this.back = back;
            this.print = print;
        }
    }
}
