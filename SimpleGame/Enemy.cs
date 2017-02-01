using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using static SimpleGame.Values;

namespace SimpleGame
{
    public class Enemy : Actor
    {
        const int SPEED= 5;

        private static Size enemysize = new Size(32, 32);        

        public Enemy(Point startpoint) : base(new Rectangle(startpoint, enemysize))
        {
            images[0] = new Bitmap("enemy.png");
        }

        //public bool IsMarkedKilled { get; private set; }

        public void Hit()
        {
            enemies.Remove(this);
            coins.Add(new PointClass(actorrect.X + actorrect.Width / 2, actorrect.Y + actorrect.Height / 2));
        }

        public override void Step(ulong ticknum)
        {
            base.Step(ticknum);
            if (onground) TryMove((ref Rectangle r) => r.X += direction * SPEED);
            if (actorrect.IntersectsWith(player.actorrect))
            {
                if (groundpound)
                {
                    Hit();
                }
                else
                {
                    gameover = true;
                    timer.Enabled = false;
                }
            }
            //foreach(var b in bullets)
            //{
            //    if(actorrect.Contains(b))
                    
            //}            
        }

        public override void TryMove(RectAction a)
        {
            Rectangle testr = actorrect;
            a(ref testr);
            foreach (var bl in blocks)
            {
                if (testr.IntersectsWith(bl)) { flip ^= 1; return; }
            }
            a(ref actorrect);

        }
    }
}
