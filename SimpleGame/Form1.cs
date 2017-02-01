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
    }

    public class PointClass
    {
        public int X;
        public int Y;
        public int Direction; //Used in moving points
        public PointClass(int i1,int i2) { X = i1; Y = i2; }

        public PointClass Copy() { return (PointClass)MemberwiseClone(); }
        public Point ToPoint() { return new Point(X, Y); }
    }    

    public partial class Form1 : Form
    {
        //readonly Font dispfont = new Font("Arial", 22);
        readonly Font dispfont = new Font("Palatino Linotype", 22);

        //Player p = new Player();
        public static Rectangle BoundsRect;

        public static readonly Brush[] bulletcolors =
        {
            Brushes.Red,
            Brushes.Yellow,
            Brushes.Orange,
            Brushes.Blue,
            Brushes.Cyan,
            Brushes.ForestGreen,
            Brushes.LightGreen,
            Brushes.LightGray,
            Brushes.Magenta
        };

        Rectangle nullrect = new Rectangle(-1000, -1000, 0, 0);        

        ulong ticknum = 0;
        
        int speed = 10;
        int bulletradius = 16;
        
        int collectedcoins = 0;

        Point pointdown = Point.Empty;

        public Form1()
        {
            InitializeComponent();
            BoundsRect = Bounds;
            blocks.Add(new Rectangle(0, 200, 300, 20));
            blocks.Add(new Rectangle(100, 400, 300, 20));
            AddFBlock(new Rectangle(280, 395, 20, 5));
            AddFBlock(new Rectangle(350, 395, 20, 5));
            AddFBlock(new Rectangle(550, 90, 40, 80));
            coins.Add(new PointClass(200, 180));
            coins.Add(new PointClass(400, 390));
            coins.Add(new PointClass(450, 340));
            enemies.Add(new Enemy(new Point(310, 220)));            
        }

        private void load(object sender, EventArgs e)
        {
            timer = new Timer();
            timer.Interval = 1;
            timer.Tick += Step;
            timer.Enabled = true;                      
        }

        private void paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            foreach (var p in bullets)
                g.FillEllipse(bulletcolors[bulletstate], p.X - bulletradius, p.Y - bulletradius, bulletradius, bulletradius);
            for (int i = 0; i < coins.Count; i++)
                if (!collected[i]) g.FillEllipse(Brushes.Gold, coins[i].X - bulletradius, coins[i].Y - bulletradius, bulletradius, bulletradius);
            foreach (var en in enemies) en.Draw(g);
            for (int i = 0; i < blocks.Count; i++)
                if (fragile[i]) g.FillRectangle(Brushes.Brown, blocks[i]); else g.DrawRectangle(Pens.Black, blocks[i]);
            player.Draw(g);
            g.DrawString("COINS: " + collectedcoins.ToString(), dispfont, Brushes.Green, 550, 0);
            if (gameover) g.DrawString("Gameover", dispfont, Brushes.Crimson, 300, 100);
            g.DrawString(groundpound.ToString(), dispfont, Brushes.Black, 550, 20);

        }

        private void keypress(object sender, KeyEventArgs e)
        {
            byte[] keys = new byte[256];
            GetKeyboardState(keys);
            if ((keys[(int)Keys.Right] & 128) == 128)
            {
                player.TryMove((ref Rectangle r) => r.X += speed);
                player.flip = 0;
            }
            if ((keys[(int)Keys.Left] & 128) == 128)
            {
                player.TryMove((ref Rectangle r) => r.X -= speed);
                player.flip = 1;
            }
            if ((keys[(int)Keys.Up] & 128) == 128)
            {
                player.TryMove((ref Rectangle r) => { if (!player.jump) player.downspeed = -8; player.jump = true; });
            }
            if ((keys[(int)Keys.Down] & 128) == 128)
            {
                player.crouch = true;
            }
            if ((keys[(int)Keys.Space] & 128) == 128)
            {
                player.CreateBullet();
            }
            if((keys[(int)Keys.Z]& 128) == 128)
            {
                player.HandleGroundPound();
            }
            for(int i=0;i<9;i++)
            {
                if((keys[i+49] & 128) == 128)
                {
                    bulletstate = i;
                }
            }
            if (e.Shift) shift = true;
        }


        private void keyrelease(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
                player.crouch = false;
            if (!e.Shift) shift = false;
        }

        private void mousedown(object sender, MouseEventArgs e)
        {            
            pointdown = e.Location;
        }

        private void mouseup(object sender, MouseEventArgs e)
        {
            Size size = (Size)(e.Location - (Size)pointdown);
            Rectangle mouserect = new Rectangle(pointdown, size);
            if (shift) AddFBlock(mouserect);
            else blocks.Add(mouserect);                        
        }

        private void Step(object s, EventArgs e)
        {
            player.Step(ticknum);                 
            //if (p.previousflip != p.flip)
            //    p.Flip();           
            for(int i=0;i<bullets.Count;i++)
            {
                //bullets[i].Item1.X += bullets[i].Item2 * 4;
                var pt = trymovepoint(bullets[i], po => po.X += po.Direction * 4);
                if (pt != -1 && fragile[pt])
                {
                    blocks[pt] = nullrect;
                    bullets.RemoveAt(i);
                }
                else
                {
                    for(int j=0;j<enemies.Count;j++)
                    {
                        if (enemies[j].actorrect.Contains(bullets[i].ToPoint())){
                            enemies[j].Hit();
                            bullets.RemoveAt(i);
                        }
                    }
                    //foreach(var enm in enemies)
                    //{
                    //    if (enm.actorrect.Contains(bullets[i].ToPoint()))
                    //    {
                    //        enm.Hit();
                    //        bullets[i].SetKilledInList();
                    //    }
                    //}
                }
                
            }
            //foreach (var en in enemies) en.Step(ticknum);
            for (int i = 0; i < enemies.Count; i++) enemies[i].Step(ticknum);
            for (int i = 0; i < coins.Count; i++)
            {
                if (player.actorrect.Contains(coins[i].ToPoint()) && !collected[i])
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


            player.previousflip = player.flip;
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
