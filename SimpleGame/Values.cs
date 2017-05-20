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

        public static List<IDeleteList> DeleteLists { get; } = new List<IDeleteList>();

        public static int bulletstate = 0;        

        public static Player player = new Player();

        public static bool gameover = false;
        public static bool shift = false;
        
        public static System.Windows.Forms.Timer timer;

        public static DeleteList<Bullet> bullets = new DeleteList<Bullet>();
        //public static int[] bullettypes = new int[65536];
        public static List<Point> coins = new List<Point>();
        public static bool[] collected = new bool[65536];
        //public static List<Rectangle> blocks = new List<Rectangle>();
        //public static bool[] fragile = new bool[65536];
        public static DeleteList<Block> blocks = new DeleteList<Block>();
        public static List<Enemy> enemies = new List<Enemy>();


        public static Rectangle MakeRect(this Point p1, Point p2)
        {
            Size rectsize = new Size(Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y));
            if (p2.X > p1.X && p2.Y > p1.Y)
                return new Rectangle(p1, rectsize);
            else if (p2.X > p1.X && p2.Y < p1.Y)
                 return new Rectangle(new Point(p1.X, p2.Y), rectsize);
            else if (p2.X < p1.X && p2.Y > p1.Y)
                 return new Rectangle(new Point(p2.X, p1.Y), rectsize);
            else
                return new Rectangle(p2, rectsize);
                       
        }
        //public static bool IntersectsUD(this Rectangle r, Rectangle r2) => (r.Y < r2.Y && r2.Bottom < r.Top) || (r2.Y < r.Y && r.Bottom < r2.Top);
        public static bool IntersectsLR(this Rectangle r, Rectangle r2) => (r.Right < r2.Left && r2.Right < r.Left);
    }
}
