using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gin
{
    static class Deck
    {
        public static List<Card> drawCards = new List<Card>();
        public static List<Card> discards = new List<Card>();
        public static List<Card> p1Hand = new List<Card>();
        public static List<Card> p2Hand = new List<Card>();

        public static void Initialize()
        {
            drawCards.Clear();
            for (int v = 0; v < 52; v++)
            {
                drawCards.Add(new Card(v % 13, v / 13));
            }
            discards.Clear();
            p1Hand.Clear();
            p2Hand.Clear();
        }

        public static void Shuffle()
        {
            for (int t = 0; t < Program.rng.Next(10, 25); t++)
            {
                List<Card> shuffledCards = new List<Card>();

                for (int i = 0; i < 52; i++)
                {
                    int cardIndex = Program.rng.Next(0, drawCards.Count());

                    shuffledCards.Add(drawCards[cardIndex]);
                    drawCards.Remove(drawCards[cardIndex]);
                }

                drawCards = shuffledCards;
            }
        }

        public static void Deal()
        {
            for (int c = 0; c < 10; c++)
            {
                Draw(0, 0);
                Draw(1, 0);
            }

            Draw(2, 0);
        }

        public static void Draw(int player, int deck)
        {
            Card card;

            if (deck == 0)
            {
                card = drawCards.Last();
            }
            else
            {
                card = discards.Last();
            }

            if (player == 0)
            {
                p1Hand.Add(card);
            }
            else if (player == 1)
            {
                p2Hand.Add(card);
            }
            else
            {
                discards.Add(card);
            }

            if (deck == 0)
            {
                drawCards.Remove(card);
            }
            else
            {
                discards.Remove(card);
            }
        }
    }
}
