using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleGame
{
    delegate void RectAction(ref Rectangle r);    

    class DBPanel : Panel
    {
        public DBPanel()
        {
            DoubleBuffered = true;
        }
    }

    class PointClass
    {
        public int X;
        public int Y;
        public int Direction;
        public PointClass(int i1,int i2) { X = i1; Y = i2; }

        public PointClass Copy() { return (PointClass)MemberwiseClone(); }
        public Point ToPoint() { return new Point(X, Y); }
    }    

    public partial class Form1 : Form
    {
        readonly Font dispfont = new Font("Arial", 22);
        
        Timer t;
        Rectangle nullrect = new Rectangle(-1000, -1000, 0, 0);
        Rectangle rect = new Rectangle(10, 10, 64, 64);
        //Point r = new Point(10, 10);
        bool flip, previousflip = false;
        Bitmap character = new Bitmap("char.png");
        List<PointClass> bullets = new List<PointClass>();
        List<PointClass> coins = new List<PointClass>();
        bool[] collected = new bool[1000];
        List<Rectangle> blocks = new List<Rectangle>();
        bool[] fragile = new bool[1000];
        bool crouch = false;

        ulong ticknum = 0;
        bool debug = false;
        int speed = 10;
        int bulletradius = 16;
        int downspeed = 0;
        int gravity = 1;
        int collectedcoins = 0;

        public Form1()
        {
            InitializeComponent();
            blocks.Add(new Rectangle(0, 200, 300, 20));
            blocks.Add(new Rectangle(100, 400, 300, 20));
            AddFBlock(new Rectangle(80, 380, 20, 20));
            AddFBlock(new Rectangle(500, 10, 40, 80));
            coins.Add(new PointClass(200, 180));
            coins.Add(new PointClass(400, 400));
        }

        private void load(object sender, EventArgs e)
        {
            t = new Timer();
            t.Interval = 1;
            t.Tick += Step;
            t.Enabled = true;            
        }

        private void paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            foreach (var p in bullets)
                g.FillEllipse(Brushes.Blue, p.X - bulletradius, p.Y - bulletradius, bulletradius, bulletradius);
            for(int i=0;i<coins.Count;i++)
                if(!collected[i]) g.FillEllipse(Brushes.Gold, coins[i].X - bulletradius, coins[i].Y - bulletradius, bulletradius, bulletradius);
            foreach (var rect in blocks)
                g.DrawRectangle(Pens.Black, rect);
            g.DrawImage(character, rect);
            g.DrawString(downspeed.ToString(), dispfont, Brushes.Violet, 500, 0);
            g.DrawString(collectedcoins.ToString(), dispfont, Brushes.Green, 550, 0);

        }

        private void keypress(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    //trymove(delegate (ref Rectangle r) { r.X += speed; });                    
                    trymove((ref Rectangle r) => { r.X += speed; });
                    flip = false;
                    break;
                case Keys.Left:
                    trymove((ref Rectangle r) => { r.X -= speed; });
                    flip = true;
                    break;                
                case Keys.Up:
                    trymove((ref Rectangle r) => { if(downspeed==0) r.Y -= 3 * speed; });
                    break;
                case Keys.Down:
                    crouch = true;
                    break;
                case Keys.Space:
                    createbullet();
                    break;
                case Keys.F5:
                    debug = true;
                    break;
                case Keys.F6:
                    rect.Location = Point.Empty;
                    break;
            }
        }


        private void keyrelease(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
                crouch = false;
        }

        private void Step(object s, EventArgs e)
        {            
            if (debug)
            {
                if (ticknum % 25 == 0) downspeed += gravity;
                //rect.Y = testloc;
            }               
            if (previousflip != flip)
                character.RotateFlip(RotateFlipType.RotateNoneFlipX);            
            for(int i=0;i<bullets.Count;i++)
            {
                //bullets[i].Item1.X += bullets[i].Item2 * 4;
                var p = trymovepoint(bullets[i], po => { po.X += po.Direction * 4; });
                if (p != -1 && fragile[p])
                {
                    blocks[p] = nullrect;
                    bullets.RemoveAt(i);
                }
                
            }
            if (rect.Bottom > Bounds.Bottom) { rect.Y = 0; }
            for(int i=0;i<coins.Count;i++)
            {
                if (rect.Contains(coins[i].ToPoint())&&!collected[i])
                {
                    collected[i] = true;
                    collectedcoins++;
                }
            }
            int c = bullets.Count;
            for (int i = 0; i < c; i++)
                if (!panel1.Bounds.Contains(bullets[i].ToPoint()))
                {
                    bullets.RemoveAt(i);
                    c = bullets.Count;
                }
            previousflip = flip;
            trymovedown();
            panel1.Invalidate();
            ticknum++;                        
        }

        private void createbullet()
        {
            PointClass p = new PointClass(rect.X + rect.Width, rect.Y + (rect.Height / 2));
            p.Direction = 1 - Convert.ToInt32(flip) * 2;
            bullets.Add(p);
        }

        private void trymove(RectAction a)
        {
            Rectangle testr = rect;            
            a(ref testr);            
            foreach (var bl in blocks)
                if (testr.IntersectsWith(bl)) return;
            a(ref rect);
        }

        private void trymovedown()
        {
            Rectangle testr = rect;
            testr.Y++;
            foreach (var b in blocks)
                if (b.IntersectsWith(testr)) { debug = false; downspeed = 0; return; }
            debug = true;
            rect.Y += downspeed;
        }

        private int trymovepoint(PointClass p, Action<PointClass> pa)
        {
            PointClass testp = p.Copy();
            //PointClass testp = p;
            pa(testp);
            for (int i = 0; i < blocks.Count; i++)
                if (blocks[i].Contains(p.ToPoint()))
                    return i;
            pa(p);
            return -1;
        }

        private void AddFBlock(Rectangle b)
        {
            blocks.Add(b);
            fragile[blocks.Count - 1] = true;
        }
    }
}
