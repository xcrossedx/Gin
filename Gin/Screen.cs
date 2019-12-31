using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gin
{
    static class Screen
    {
        //CARD BACK PATTERN
        public static CardBack cardBack;

        //CARD MAP
        //-2 = IN NEED OF UPDATE
        //-1 = NOT VISIBLE
        //0 = VISIBLE NOT HIGHLIGHTED
        //1 = VISIBLE WITH FRESH HIGHLIGHT
        //2 = VISIBLE WITH OLD HIGHLIGHT
        public static int[,] hands =
        {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1 }
        };
        public static int[] drawPiles = { 0, 0 };

        //SORT SELECTOR
        //0 = NOT VISIBLE
        //1 = VISIBLE
        public static int[] sortSelector = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        //BUTTON MAP
        //POSITIVE = HIGHLIGHTED
        //NEGETIVE = UNHIGHLIGHTED
        //0 = NOT VISIBLE
        //1/-1 = SORT
        //2/-2 = DRAW
        //3/-3 = DONE
        //4/-4 = AUTO
        //5/-5 = MANUAL 
        public static int[] buttons = { 0, 0 };

        //OLD CARD MAP FOR COMPARISON
        private static int[,] oldHands =
        {
            { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
            { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 }
        };
        private static int[] oldDrawPiles = { -1, -1 };

        //OLD SORT SELECTOR
        public static int[] oldSortSelector = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        //OLD BUTTON MAP
        private static int[] oldButtons = { 0, 0 };

        //SETS ALL VISIBLE CARDS AND DRAWS THEM
        public static void Initialize()
        {
            Console.Clear();

            for (int p = 0; p < 2; p++)
            {
                for (int c = 0; c < 10; c++)
                {
                    hands[p, c] = 0;
                    oldHands[p, c] = -1;
                }

                drawPiles[p] = 0;
                oldDrawPiles[p] = -1;

                buttons[p] = 0;
            }

            for (int s = 0; s < 11; s++)
            {
                sortSelector[s] = 0;
                oldSortSelector[s] = 0;
            }

            Draw();
        }

        public static (int width, int height) oldScreenSize;

        //CHECKS FOR CHANGES IN THE SCREEN APPEARANCE AND REQUESTS UPDATES
        public static void Draw()
        {
            (int width, int height) = (Console.WindowWidth, Console.WindowHeight);

            if (oldScreenSize != (width, height))
            {
                Console.Clear();

                if (width < 106) { Console.WindowWidth = 106; }
                if (height < 30) { Console.WindowHeight = 30; }

                Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);

                Clear();

                Console.CursorVisible = false;
            }

            for (int p = 0; p < 2; p++)
            {
                for (int c = 0; c < 11; c++)
                {
                    //COMPARING EACH CARD OF EACH PLAYERS HAND
                    if (hands[p, c] != oldHands[p, c] || oldScreenSize != (width, height))
                    {
                        DrawCard(p, c, hands[p, c]);

                        if (hands[p, c] == -2)
                        {
                            hands[p, c] = 0;
                        }
                        else
                        {
                            oldHands[p, c] = hands[p, c];
                        }
                    }
                }

                //COMPARING EACH DRAW PILE
                if (drawPiles[p] != oldDrawPiles[p] || oldScreenSize != (width, height))
                {
                    DrawCard(2, p, drawPiles[p]);

                    if (drawPiles[p] == -2)
                    {
                        drawPiles[p] = 0;
                    }
                    else
                    {
                        oldDrawPiles[p] = drawPiles[p];
                    }
                }

                //COMPARING BUTTONS
                if (buttons[p] != oldButtons[p] || oldScreenSize != (width, height))
                {
                    if (Math.Abs(oldButtons[p]) == 5)
                    {
                        DrawButton(p, 0);
                    }

                    DrawButton(p, buttons[p]);
                    oldButtons[p] = buttons[p];
                }
            }

            for (int s = 0; s < 11; s++)
            {
                if (sortSelector[s] != oldSortSelector[s] || oldScreenSize != (width, height))
                {
                    DrawSortSelector(s, sortSelector[s]);
                }

                oldSortSelector[s] = sortSelector[s];
            }

            oldScreenSize = (Console.WindowWidth, Console.WindowHeight);
        }

        //DRAWS A CARD IN THE GIVEN LOCATION WITH THE GIVEN ID AND HIGHLIGHTS IT IF IT IS SELECTED
        private static void DrawCard(int location, int id, int appearance)
        {
            //CURRENT PLAYER
            int player = Game.turn % 2;

            //SUITS
            string[] suits = { "♠", "♣", "♦", "♥" };

            //TEMP CARD ORIGIN
            (int col, int row) = (0, 0);

            //CUTOFF FOR THE CARD IF ITS ON THE EDGE OF THE SCREEN
            int topCutoff = 0;
            int bottomCutoff = 6;

            //FACE
            //0 = FRONT
            //1 = BACK
            int face = 0;

            //TEMP CARD VALUES
            string value = "";
            string suit = "";

            //LOCATIONS
            //0 = YOUR HAND
            if (location == 0)
            {
                //IF CARD IS VISIBLE
                if (appearance != -1)
                {
                    //ASSIGNING CARD VALUE BASED ON TURN
                    if (player == 0)
                    {
                        value = AssignValue(Deck.p1Hand[id].value);
                        suit = suits[Deck.p1Hand[id].suit];
                    }
                    else
                    {
                        value = AssignValue(Deck.p2Hand[id].value);
                        suit = suits[Deck.p2Hand[id].suit];
                    }
                }
                //IF CARD IS NOT VISIBLE
                else
                {
                    face = 1;
                }

                //SETTING CARD ORIGIN
                if (id != 10)
                {
                    bottomCutoff = 4;

                    (col, row) = (((Console.WindowWidth / 2) - 49) + (id * 10), Console.WindowHeight - 4);
                }
                else
                {
                    (col, row) = (((Console.WindowWidth / 2) - 4), Console.WindowHeight - 11);
                }
            }
            //1 = OPP HAND
            else if (location == 1)
            {
                topCutoff = 2;
                face = 1;
                (col, row) = (((Console.WindowWidth / 2) - 49) + (id * 10), 0);
            }
            //2 = DRAW PILES
            else if (location == 2)
            {
                //RANDOM DRAW PILE
                if (id == 0)
                {
                    face = 1;
                    (col, row) = ((Console.WindowWidth / 2) - 12, (Console.WindowHeight / 2) - 4);
                }
                //DISCARD PILE
                else
                {
                    if (appearance != -1)
                    {
                        value = AssignValue(Deck.discards.Last().value);
                        suit = suits[Deck.discards.Last().suit];
                    }
                    else
                    {
                        face = 1;
                    }

                    (col, row) = ((Console.WindowWidth / 2) + 4, (Console.WindowHeight / 2) - 4);
                }
            }

            //DRAWING THE CARD LINE BY LINE
            for (int r = topCutoff; r < bottomCutoff; r++)
            {
                Console.SetCursorPosition(col, row + r - topCutoff);

                for (int c = 0; c < 9; c++)
                {
                    char piece = ' ';

                    //IF THE CARD IS VISIBLE
                    if (appearance != -1)
                    {
                        //DRAWING OUTLINE
                        if ((r == 0 || r == 5) || (c <= 1 || c >= 7))
                        {
                            if (appearance == 1)
                            {
                                Console.BackgroundColor = ConsoleColor.Red;
                            }
                            else if (appearance == 2)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.White;
                            }
                        }
                        //DRAWING CARD FACE
                        else
                        {
                            if (face == 0)
                            {
                                Console.BackgroundColor = ConsoleColor.White;
                            }
                            else
                            {
                                Console.BackgroundColor = cardBack.back;
                                Console.ForegroundColor = cardBack.fore;
                                
                                if ((r + c) % 2 == 1)
                                {
                                    piece = cardBack.print;
                                }
                            }
                        }
                    }
                    //IF THE CARD ISNT VISIBLE
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                    }

                    Console.Write(piece);
                }
            }

            //IF CARD IS FACE UP WRITE IN THE VALUE
            if (face == 0)
            {
                Console.BackgroundColor = ConsoleColor.White;

                //ADJUST THE SPACING FOR THE CARD VALUE AND SUIT
                string space = "  ";
                
                if (value.Length == 1)
                {
                    space = "   ";
                }

                //COLOR THE VALUE AND SUIT BASED ON THE SUIT COLOR
                if (suit == "♠" || suit == "♣")
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                }

                //WRITE THE VALUE AT THE TOP OF THE CARD
                Console.SetCursorPosition(col + 2, row + 1);

                Console.Write($"{value}{space}{suit}");

                //IF THE CARD IS NOT CUT OFF WRITE THE VALUE AT THE BOTTOM OF THE CARD
                if (bottomCutoff >= 5)
                {
                    Console.SetCursorPosition(col + 2, row + 4);

                    Console.Write($"{suit}{space}{value}");
                }
            }

            //CREATE A STRING CONTAINING THE VALUE OF THE CARD CONVERTED TO STANDARD PLAYING CARD VALUES
            string AssignValue(int cardV)
            {
                string val;

                //ACE
                if (cardV == 0)
                {
                    val = "A";
                }
                //FACE CARDS
                else if (cardV == 10)
                {
                    val = "J";
                }
                else if (cardV == 11)
                {
                    val = "Q";
                }
                else if (cardV == 12)
                {
                    val = "K";
                }
                //NUMBERED CARDS
                else
                {
                    val = $"{(cardV) + 1}";
                }

                return val;
            }
        }

        //DRAW BUTTONS BASED ON ID AND CURRENT APPEARANCE
        private static void DrawButton(int id, int appearance)
        {
            //AVAILABLE BUTTON LABELS
            //BLANK ONE IS FOR CLEARING OUT OLD BUTTONS
            string[] labels = { "      ", "Draw", "Sort", "Done", "Auto", "Manual" };

            //TEMP BUTTON ORIGIN
            (int col, int row) = (0, 0);

            //EMPTY CHAR FOR DRAWING BACKGROUND COLOR
            char piece = ' ';

            //SETTING BUTTON ORIGIN
            //LEFT
            if (id == 0)
            {
                //FOR LARGER BUTTONS
                if (Math.Abs(appearance) == 5 || appearance == 0)
                {
                    (col, row) = ((Console.WindowWidth / 2) - 14, Console.WindowHeight - 10);
                }
                //FOR SMALLER BUTTONS
                else
                {
                    (col, row) = ((Console.WindowWidth / 2) - 13, Console.WindowHeight - 10);
                }
            }
            //RIGHT
            else
            {
                //LARGER
                if (Math.Abs(appearance) == 5 || appearance == 0)
                {
                    (col, row) = ((Console.WindowWidth / 2) + 5, Console.WindowHeight - 10);
                }
                //SMALLER
                else
                {
                    (col, row) = ((Console.WindowWidth / 2) + 6, Console.WindowHeight - 10);
                }
            }

            //IF BUTTON IS NOT VISIBLE
            if (appearance == 0)
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
            }
            //IF BUTTON IS HIGHLIGHTED
            else if (appearance < 0)
            {
                Console.BackgroundColor = ConsoleColor.Red;
            }
            //IF BUTTON ISN'T HIGHLIGHTED
            else
            {
                Console.BackgroundColor = ConsoleColor.White;
            }

            //DRAW BUTTON BACKGROUND
            for (int r = 0; r < 3; r++)
            {
                Console.SetCursorPosition(col, row + r);

                //MAKE THE BUTTON AS LARGE AS THE LABEL
                for (int c = 0; c < labels[Math.Abs(appearance)].Length + 4; c++)
                {
                    Console.Write(piece);
                }
            }

            //IF THE BUTTON IS VISIBLE THEN LABEL IT
            if (appearance != 0)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(col + 2, row + 1);
                Console.Write(labels[Math.Abs(appearance)]);
            }
        }

        private static void DrawSortSelector(int id, int appearance)
        {
            (int col, int row) = (((Console.WindowWidth / 2) - 50) + (id * 10), Console.WindowHeight - 4);

            if (appearance == 0)
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Red;
            }

            for (int r = 0; r < 4; r ++)
            {
                Console.SetCursorPosition(col, row + r);
                Console.Write(" ");
            }
        }

        public static void ACPopUp()
        {
            Clear();

            (int col, int row) = ((Console.WindowWidth / 2) - 31, (Console.WindowHeight / 2) - 2);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Red;

            Console.SetCursorPosition(col, row);
            Console.Write("                                                               ");
            Console.SetCursorPosition(col, row + 1);
            Console.Write($"  Only hit enter once player {((Game.turn + 1) % 2) + 1} is not able to see the screen.  ");
            Console.SetCursorPosition(col, row + 2);
            Console.Write("                                                               ");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Black;

            ConsoleKey key = ConsoleKey.A;

            while (key != ConsoleKey.Enter)
            {
                key = Console.ReadKey(true).Key;
            }

            Initialize();
        }

        public static void DrawWinner()
        {
            (int col, int row) = ((Console.WindowWidth / 2) - 13, (Console.WindowHeight / 2) - 2);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Red;

            Console.SetCursorPosition(col, row);
            Console.Write("                           ");
            Console.SetCursorPosition(col, row + 1);
            Console.Write($"  Player {(Game.turn % 2) + 1} wins the game!  ");
            Console.SetCursorPosition(col, row + 2);
            Console.Write("                           ");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Black;

            ConsoleKey key = ConsoleKey.A;

            while (key != ConsoleKey.Enter)
            {
                key = Console.ReadKey(true).Key;
            }
        }

        public static void Clear()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkGreen;

            for (int r = 0; r < Console.WindowHeight; r++)
            {
                Console.SetCursorPosition(0, r);

                for (int c = 0; c < Console.WindowWidth; c++)
                {
                    Console.Write(" ");
                }
            }
        }

        public static void RefreshHand()
        {
            for (int c = 0; c < 10; c++)
            {
                hands[0, c] = -2;
            }
            Draw();
        }
    }
}
