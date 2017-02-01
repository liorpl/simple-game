using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using static SimpleGame.Values;

namespace SimpleGame
{
    public abstract class Actor
    {
        public int flip, previousflip = 0;
        public int direction { get { return flip * 2 - 1; } }
        protected bool gravity = false;
        public int downspeed = 0;
        public bool jump = false;
        protected bool onground = false;        

        protected Bitmap[] images = new Bitmap[8];
        protected int currentchar = 0;

        public Rectangle actorrect = new Rectangle();

        public Actor(Rectangle startrect)
        {
            actorrect = startrect;
        }

        public virtual void TryMove(RectAction a)
        {
            Rectangle testr = actorrect;
            a(ref testr);
            foreach (var bl in blocks)
                if (testr.IntersectsWith(bl)) return;
            a(ref actorrect);
        }

        public bool TryMoveDown()
        {
            Rectangle testr = actorrect;
            testr.Y += downspeed + 1;
            foreach (var b in blocks)
                if (b.IntersectsWith(testr)) { gravity = false; downspeed = 0; jump = false; onground = true; return false; }
            gravity = true;
            actorrect.Y += downspeed;
            onground = false;
            return true;
        }

        public virtual void Step(ulong ticknum)
        {
            if (gravity)
            {
                if (ticknum % 5 == 0) downspeed++;
            }

            TryMoveDown();
            if (previousflip != flip) Flip();
        }

        public void Flip() { foreach (var i in images) i?.RotateFlip(RotateFlipType.RotateNoneFlipX); }

        public void Draw(Graphics g) { g.DrawImage(images[currentchar], actorrect); }
    }
}
