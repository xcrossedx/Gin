using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gin
{
    static class Player
    {
        private static int player;

        public static void TakeTurn(int p)
        {
            player = p;
            Screen.ACPopUp();
            ActionSelect();
        }

        private static void ActionSelect()
        {
            int actionSelection = 0;

            ConsoleKey key = ConsoleKey.A;

            while (key != ConsoleKey.Enter)
            {
                if (actionSelection == 0)
                {
                    Screen.buttons[0] = -1;
                    Screen.buttons[1] = 2;
                }
                else
                {
                    Screen.buttons[0] = 1;
                    Screen.buttons[1] = -2;
                }

                Screen.Draw();

                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.LeftArrow || key == ConsoleKey.RightArrow)
                {
                    actionSelection = (actionSelection + 1) % 2;
                }
            }

            Screen.buttons[0] = 0;
            Screen.buttons[1] = 0;

            Screen.Draw();

            if(actionSelection == 1)
            {
                SelectSortType(false);
            }
            else
            {
                DrawCard();
            }
        }

        private static void SelectSortType(bool final)
        {
            int sortType = 0;

            bool back = false;
            bool selected = false;

            while (!selected && !back)
            {
                if (sortType == 0)
                {
                    Screen.buttons[0] = -4;
                    Screen.buttons[1] = 5;
                }
                else
                {
                    Screen.buttons[0] = 4;
                    Screen.buttons[1] = -5;
                }

                Screen.Draw();

                ConsoleKey key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.RightArrow || key == ConsoleKey.LeftArrow)
                {
                    sortType = (sortType + 1) % 2;
                }
                else if (key == ConsoleKey.Enter)
                {
                    selected = true;
                }
                else if (key == ConsoleKey.Escape || key == ConsoleKey.Backspace)
                {
                    back = true;
                }
            }

            if (!back)
            {
                if (sortType == 0)
                {
                    AutoSort(final);
                }
                else
                {
                    Screen.buttons[0] = 0;
                    Screen.buttons[1] = 0;
                    ManualSort(final);
                }
            }
            else if (!final)
            {
                ActionSelect();
            }
            else
            {
                FinishTurn();
            }
        }

        private static void AutoSort(bool final)
        {
            if (player == 0)
            {
                Deck.p1Hand = Deck.p1Hand.OrderByDescending(x => x.value).ToList();
            }
            else
            {
                Deck.p2Hand = Deck.p2Hand.OrderByDescending(x => x.value).ToList();
            }

            Screen.RefreshHand();

            SelectSortType(final);
        }

        private static void ManualSort(bool final)
        {
            bool back = false;

            //LAST DIRECTION MOVED
            //0 = RIGHT
            //1 = LEFT
            int lastMoved = 0;

            int c1 = 0;
            int oldC1 = -1;
            int c2 = -1;
            int oldC2 = -1;

            while(!back)
            {
                if (Select(1))
                {
                    if (lastMoved == 0)
                    {
                        c2 = c1;
                    }
                    else
                    {
                        c2 = c1 + 1;
                    }

                    if (Select(2))
                    {
                        Card card;
                        int index = c2;
                        bool c1Left = false;

                        if (c1 < c2)
                        {
                            index = c2 - 1;
                            c1Left = true;
                        }

                        if (player == 0)
                        {
                            card = Deck.p1Hand[c1];
                            Deck.p1Hand.Remove(card);

                            if (index != 9)
                            {
                                Deck.p1Hand.Insert(index, card);
                            }
                            else
                            {
                                Deck.p1Hand.Add(card);
                            }
                        }
                        else
                        {
                            card = Deck.p2Hand[c1];
                            Deck.p2Hand.Remove(card);

                            if (index != 9)
                            {
                                Deck.p2Hand.Insert(index, card);
                            }
                            else
                            {
                                Deck.p2Hand.Add(card);
                            }
                        }

                        Screen.hands[0, c1] = 0;

                        if (c1Left)
                        {
                            for (int c = c1 + 1; c <= index; c++)
                            {
                                Screen.hands[0, c] = -2;
                            }
                        }
                        else
                        {
                            for (int c = index; c < c1; c++)
                            {
                                Screen.hands[0, c] = -2;
                            }
                        }

                        Screen.sortSelector[c2] = 0;

                        Screen.Draw();

                        c1 = index;
                        c2 = -1;
                    }
                    else
                    {
                        c2 = -1;
                    }
                }
                else
                {
                    Screen.hands[0, c1] = 0;
                    back = true;
                }
            }

            SelectSortType(final);

            bool Select(int c)
            {
                bool selected = true;
                bool done = false;
                
                while (!done)
                {
                    if (c1 != oldC1)
                    {
                        if (oldC1 != -1)
                        {
                            Screen.hands[0, oldC1] = 0;
                        }
                        
                        oldC1 = c1;
                    }
                    if (c2 != oldC2)
                    {
                        if (oldC2 != -1)
                        {
                            Screen.sortSelector[oldC2] = 0;
                        }

                        oldC2 = c2;
                    }

                    if (c == 1)
                    {
                        Screen.hands[0, c1] = 1;
                    }
                    else
                    {
                        Screen.hands[0, c1] = 2;
                        Screen.sortSelector[c2] = 1;
                    }

                    Screen.Draw();

                    ConsoleKey key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.RightArrow)
                    {
                        lastMoved = 0;

                        if (c == 1)
                        {
                            if (c1 != 9)
                            {
                                c1 += 1;
                            }
                            else
                            {
                                c1 = 0;
                            }
                        }
                        else
                        {
                            if (c2 != 10)
                            {
                                c2 += 1;
                            }
                            else
                            {
                                c2 = 0;
                            }
                        }
                    }
                    else if (key == ConsoleKey.LeftArrow)
                    {
                        lastMoved = 1;

                        if (c == 1)
                        {
                            if (c1 != 0)
                            {
                                c1 -= 1;
                            }
                            else
                            {
                                c1 = 9;
                            }
                        }
                        else
                        {
                            if (c2 != 0)
                            {
                                c2 -= 1;
                            }
                            else
                            {
                                c2 = 10;
                            }
                        }
                    }
                    else if (key == ConsoleKey.Enter)
                    {
                        done = true;
                    }
                    else if (key == ConsoleKey.Escape || key == ConsoleKey.Backspace)
                    {
                        done = true;
                        selected = false;
                    }
                }

                return selected;
            }
        }

        private static void DrawCard()
        {
            int drawPile = 0;

            bool back = false;
            bool selected = false;

            while (!selected && !back)
            {
                Screen.drawPiles[drawPile] = 1;
                Screen.drawPiles[(drawPile + 1) % 2] = 0;

                Screen.Draw();

                ConsoleKey key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.RightArrow || key == ConsoleKey.LeftArrow)
                {
                    drawPile = (drawPile + 1) % 2;
                }
                else if (key == ConsoleKey.Enter)
                {
                    Deck.Draw(player, drawPile);
                    Screen.hands[0, 10] = 0;

                    selected = true;
                }
                else if (key == ConsoleKey.Escape || key == ConsoleKey.Backspace)
                {
                    back = true;
                }
            }

            if (Deck.drawCards.Count() > 0)
            {
                Screen.drawPiles[0] = 0;
            }
            else
            {
                Screen.drawPiles[0] = -1;
            }

            if (Deck.discards.Count() > 0)
            {
                Screen.drawPiles[1] = -2;
            }
            else
            {
                Screen.drawPiles[1] = -1;
            }

            Screen.Draw();

            Game.CheckCards(player, true);

            if (!Game.over)
            {
                if (!back)
                {
                    SelectDiscard();
                }
                else
                {
                    ActionSelect();
                }
            }
        }

        private static void SelectDiscard()
        {
            int lastSelection = 0;
            int lowerSelection = 0;
            int selection = 0;

            bool selected = false;

            while (!selected)
            {
                Screen.hands[0, lastSelection] = 0;
                Screen.hands[0, selection] = 1;
                lastSelection = selection;

                Screen.Draw();

                ConsoleKey key = Console.ReadKey(true).Key;

                if (selection < 10)
                {
                    if (key == ConsoleKey.RightArrow)
                    {
                        if (selection != 9)
                        {
                            selection += 1;
                        }
                        else
                        {
                            selection = 0;
                        }
                    }
                    else if (key == ConsoleKey.LeftArrow)
                    {
                        if (selection != 0)
                        {
                            selection -= 1;
                        }
                        else
                        {
                            selection = 9;
                        }
                    }
                    else if (key == ConsoleKey.UpArrow || key == ConsoleKey.DownArrow)
                    {
                        lowerSelection = selection;
                        selection = 10;
                    }
                }
                else
                {
                    if (key == ConsoleKey.DownArrow || key == ConsoleKey.UpArrow)
                    {
                        selection = lowerSelection;
                    }
                }

                if (key == ConsoleKey.Enter)
                {
                    selected = true;
                }
            }

            Card card;

            if (player == 0)
            {
                card = Deck.p1Hand[selection];

                Deck.p1Hand.Remove(card);
                Deck.discards.Add(card);
            }
            else
            {
                card = Deck.p2Hand[selection];

                Deck.p2Hand.Remove(card);
                Deck.discards.Add(card);
            }

            Screen.hands[0, 10] = -1;
            Screen.drawPiles[1] = -2;
            
            if (Deck.discards.Count() == 1)
            {
                Screen.drawPiles[1] = 0;
            }

            Screen.RefreshHand();

            FinishTurn();
        }

        private static void FinishTurn()
        {
            int finalAction = 0;

            ConsoleKey key = ConsoleKey.A;

            while (key != ConsoleKey.Enter)
            {
                if (finalAction == 0)
                {
                    Screen.buttons[0] = -2;
                    Screen.buttons[1] = 3;
                }
                else
                {
                    Screen.buttons[0] = 2;
                    Screen.buttons[1] = -3;
                }

                Screen.Draw();

                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.LeftArrow || key == ConsoleKey.RightArrow)
                {
                    finalAction = (finalAction + 1) % 2;
                }
            }

            Screen.buttons[0] = 0;
            Screen.buttons[1] = 0;

            Screen.Draw();

            if (finalAction == 0)
            {
                SelectSortType(true);
            }
        }
    }
}
