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
        const int SPEED = 3;

        private static Size enemysize = new Size(32, 32);

        public Enemy(Point startpoint) : base(startpoint, new Bitmap[]{ new Bitmap(@"Images\enemy1.png") })
        {            
            flipping = true;
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
            if (OnGround) TryMove((ref Rectangle r) => r.X += Direction * SPEED);
            if (actorrect.IntersectsWith(player.actorrect))
            {
                if (player.GroundPound)
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

    }
}
