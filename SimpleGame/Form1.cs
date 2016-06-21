using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SimpleGame.Values;

namespace SimpleGame
{
    public delegate void RectAction(ref Rectangle r);    

    class DBPanel : Panel
    {
        public DBPanel()
        {
            DoubleBuffered = true;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DBPanel
            // 
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResumeLayout(false);

        }
    }

    public class PointClass
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

        Player p = new Player();
        public static Rectangle BoundsRect;
        Timer t;
        Rectangle nullrect = new Rectangle(-1000, -1000, 0, 0);
        
        //Point r = new Point(10, 10);
        
        
        
        //bool crouch = false;

        ulong ticknum = 0;
        
        int speed = 10;
        int bulletradius = 16;
        
        int collectedcoins = 0;

        public Form1()
        {
            InitializeComponent();
            BoundsRect = Bounds;
            blocks.Add(new Rectangle(0, 200, 300, 20));
            blocks.Add(new Rectangle(100, 400, 300, 20));
            AddFBlock(new Rectangle(250, 380, 20, 20));
            AddFBlock(new Rectangle(500, 40, 40, 80));
            coins.Add(new PointClass(200, 180));
            coins.Add(new PointClass(400, 390));
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
            for (int i = 0; i < coins.Count; i++)
                if (!collected[i]) g.FillEllipse(Brushes.Gold, coins[i].X - bulletradius, coins[i].Y - bulletradius, bulletradius, bulletradius);
            //foreach (var rect in blocks)
            //    g.DrawRectangle(Pens.Black, rect);
            for (int i = 0; i < blocks.Count; i++)
                if (fragile[i]) g.FillRectangle(Brushes.Brown, blocks[i]); else g.DrawRectangle(Pens.Black, blocks[i]);
            p.draw(g);            
            g.DrawString(collectedcoins.ToString(), dispfont, Brushes.Green, 550, 0);

        }

        private void keypress(object sender, KeyEventArgs e)
        {
            byte[] keys = new byte[256];
            GetKeyboardState(keys);
            if ((keys[(int)Keys.Right]  & 128) == 128)
            {
                p.trymove((ref Rectangle r) => { r.X += speed; });
                p.flip = false;
            }
            if ((keys[(int)Keys.Left]  & 128) == 128)
            {
                p.trymove((ref Rectangle r) => { r.X -= speed; });
                p.flip = true;
            }
            if ((keys[(int)Keys.Up]  & 128) == 128)
            {
                p.trymove((ref Rectangle r) => { if (p.downspeed == 0) r.Y -= 6 * speed; });
            }
            if ((keys[(int)Keys.Down]  & 128) == 128)
            {
                p.crouch = true;
            }
            if ((keys[(int)Keys.Space]  & 128) == 128)
            {
                p.createbullet();
            }            
        }


        private void keyrelease(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
                p.crouch = false;
        }

        private void Step(object s, EventArgs e)
        {
            p.Step(ticknum);                 
            if (p.previousflip != p.flip)
                p.Flip();           
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
            for (int i = 0; i < coins.Count; i++)
            {
                if (p.rect.Contains(coins[i].ToPoint()) && !collected[i])
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


            p.previousflip = p.flip;
            panel1.Invalidate();
            ticknum++;                        
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

        
    }
}
