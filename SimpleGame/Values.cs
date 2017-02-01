using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SimpleGame
{
    public static class Values
    {
        [DllImport("user32.dll")]
        public static extern int GetKeyboardState(byte[] keystate);

        
        public static int bulletstate = 0;


        public static Player player = new Player();

        public static bool gameover = false;
        public static bool shift = false;
        public static bool groundpound = false;
        
        public static System.Windows.Forms.Timer timer;

        public static List<PointClass> bullets = new List<PointClass>();
        //public static int[] bullettypes = new int[65536];
        public static List<PointClass> coins = new List<PointClass>();
        public static bool[] collected = new bool[65536];
        public static List<Rectangle> blocks = new List<Rectangle>();
        public static bool[] fragile = new bool[65536];
        public static List<Enemy> enemies = new List<Enemy>();

        public static void AddFBlock(Rectangle b)
        {
            blocks.Add(b);
            fragile[blocks.Count - 1] = true;
        }

        //public static bool IntersectsUD(this Rectangle r, Rectangle r2) => (r.Y < r2.Y && r2.Bottom < r.Top) || (r2.Y < r.Y && r.Bottom < r2.Top);

        //TODO move this and GameList to a seperate namespace
    }
}
