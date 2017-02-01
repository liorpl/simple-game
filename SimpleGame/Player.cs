using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using static SimpleGame.Values;

namespace SimpleGame
{
    public class Player : Actor
    {
        //Bitmap character1 = new Bitmap("char.png");
        //Bitmap character2 = new Bitmap("charcrouch.png");

        //Bitmap character => crouch ? character2 : character1;


        public Player() : base(new Rectangle(10, 10, 64, 64))
        {
            images[0] = new Bitmap("char.png");
            images[1] = new Bitmap("charcrouch.png");
        }

        public bool crouch = false;

        public override void Step(ulong ticknum)
        {
            base.Step(ticknum);
            currentchar = Convert.ToInt32(crouch);
            

            if (actorrect.Bottom > Form1.BoundsRect.Bottom) { actorrect.Y = -1 * actorrect.Height; }
            
        }                

        public void CreateBullet()
        {
            int yb = actorrect.Y + (crouch ? actorrect.Height-8 : actorrect.Height/2);
            PointClass p = new PointClass(actorrect.X + actorrect.Width, yb);
            p.Direction = 1 - flip * 2;
            bullets.Add(p);
        }       
        
        public async void HandleGroundPound()
        {
            if (onground) return;
            groundpound = true;
            downspeed = 20;
            await Task.Factory.StartNew(() => { while (!onground) { } });
            groundpound = false;
        } 
        
    }
}
