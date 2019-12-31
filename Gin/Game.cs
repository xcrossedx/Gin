using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gin
{
    static class Game
    {
        public static bool over = false;

        public static int turn = 0;

        public static void Play()
        {
            Initialize();

            while (!over)
            {
                Player.TakeTurn(turn % 2);

                CheckCards(turn % 2, false);

                if (!over)
                {
                    turn += 1;
                }
            }

            Screen.DrawWinner();

            Menu.NewGame();
        }

        public static void CheckCards(int player, bool big)
        {
            List<Card> hand = new List<Card>();
            List<Card> fours = new List<Card>();
            int winCards = 0;

            GetHand();
            CheckPairs();
            CheckRuns();
            Result();

            void GetHand()
            {
                hand.Clear();

                if (player == 0)
                {
                    for (int c = 0; c < Deck.p1Hand.Count(); c++)
                    {
                        hand.Add(Deck.p1Hand[c]);
                    }
                }
                else
                {
                    for (int c = 0; c < Deck.p2Hand.Count(); c++)
                    {
                        hand.Add(Deck.p2Hand[c]);
                    }
                }
            }

            void CheckPairs()
            {
                for (int v = 0; v < 13; v++)
                {
                    List<Card> set = new List<Card>();

                    for (int s = 0; s < 4; s++)
                    {
                        Card card = hand.Find(x => x.value == v && x.suit == s);

                        if (card != null)
                        {
                            set.Add(card);
                            hand.Remove(card);
                        }
                    }

                    if (set.Count() > 2)
                    {
                        if (set.Count() == 4)
                        {
                            fours.Add(set.Last());
                            set.Remove(set.Last());
                        }

                        winCards += set.Count();
                    }
                    else
                    {
                        hand.AddRange(set);
                    }

                    set.Clear();
                }
            }

            void CheckRuns()
            {
                for (int s = 0; s < 4; s++)
                {
                    List<Card> set = new List<Card>();

                    for (int v = 0; v < 13; v++)
                    {
                        Card card = hand.Find(x => x.value == v && x.suit == s);

                        if (card == null && fours.Exists(x => x.value == v))
                        {
                            card = fours.Find(x => x.value == v);
                        }

                        if (card != null)
                        {
                            set.Add(card);
                        }
                        
                        if (card == null || v == 12)
                        {
                            if (set.Count() > 2)
                            {
                                winCards += set.Count();

                                foreach (Card c in set)
                                {
                                    if (fours.Contains(card))
                                    {
                                        fours.Remove(card);
                                    }
                                    else
                                    {
                                        hand.Remove(card);
                                    }
                                }
                            }
                            
                            set.Clear();
                        }
                    }
                }

                winCards += fours.Count();
            }

            void Result()
            {
                if (big && winCards == 11)
                {
                    over = true;
                }
                else if (!big && winCards >= 10)
                {
                    over = true;
                }
            }
        }

        private static void Initialize()
        {
            over = false;
            turn = 0;

            Deck.Initialize();
            Deck.Shuffle();
            Deck.Deal();
        }
    }
}
