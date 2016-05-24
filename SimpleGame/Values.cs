using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace SimpleGame
{
    public static class Values
    {
        public static List<PointClass> bullets = new List<PointClass>();
        public static List<PointClass> coins = new List<PointClass>();
        public static bool[] collected = new bool[1000];
        public static List<Rectangle> blocks = new List<Rectangle>();
        public static bool[] fragile = new bool[1000];

        public static void AddFBlock(Rectangle b)
        {
            blocks.Add(b);
            fragile[blocks.Count - 1] = true;
        }

    }
}
