using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using static SimpleGame.Values;

namespace SimpleGame
{
    public class Player
    {
        Bitmap character1 = new Bitmap("char.png");
        Bitmap character2 = new Bitmap("charcrouch.png");

        Bitmap character => crouch ? character2 : character1;

        public Rectangle rect = new Rectangle(10, 10, 64, 64);

        public bool flip, previousflip = false;
        bool gravity = false;        
        public int downspeed = 0;
        public bool crouch = false;

        public void Step(ulong ticknum)
        {
            if (gravity)
            {
                if (ticknum % 5 == 0) downspeed++;
                //rect.Y = testloc;
            }

            if (rect.Bottom > Form1.BoundsRect.Bottom) { rect.Y = 0; }
            
            trymovedown();
        }

        public void trymove(RectAction a)
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
            testr.Y += downspeed;
            foreach (var b in blocks)
                if (b.IntersectsWith(testr)) { gravity = false; downspeed = 0; return; }
            gravity = true;
            rect.Y += downspeed;
        }

        public void createbullet()
        {
            int yb = rect.Y + (crouch ? rect.Height-8 : rect.Height/2);
            PointClass p = new PointClass(rect.X + rect.Width, yb);
            p.Direction = 1 - Convert.ToInt32(flip) * 2;
            bullets.Add(p);
        }

        public void Flip() { character1.RotateFlip(RotateFlipType.RotateNoneFlipX); character2.RotateFlip(RotateFlipType.RotateNoneFlipX); }

        public void draw(Graphics g) { g.DrawImage(character, rect); }
    }
}
