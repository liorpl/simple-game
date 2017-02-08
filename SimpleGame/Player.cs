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
        
        public bool GroundPound { get; private set; }

        public Player() : base(Point.Empty, new Bitmap[] { new Bitmap(@"Images\char.png"), new Bitmap(@"Images\charcrouch.png") })
        {
            
        }

        public bool crouch = false;

        public override void Step(ulong ticknum)
        {
            base.Step(ticknum);
            currentchar = Convert.ToInt32(crouch);
            

            
            
        }                

        public void CreateBullet()
        {
            int yb = actorrect.Y + (crouch ? actorrect.Height - 8 : actorrect.Height / 2);
            PointClass p = new PointClass(actorrect.X + actorrect.Width, yb);
            p.Direction = 1 - flip * 2;
            bullets.Add(new Bullet { Pos = p, BulletType = bulletstate });
        }       
        
        public async void HandleGroundPound()
        {
            if (OnGround) return;
            GroundPound = true;
            downspeed = 50;
            await Task.Factory.StartNew(() => { while (!OnGround) { } });
            if (LastGroundBlock.Rect != Rectangle.Empty && LastGroundBlock.Fragile)
                LastGroundBlock.Clear();
            GroundPound = false;
        } 
        
    }
}
