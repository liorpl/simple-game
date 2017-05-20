using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Sw = System.Diagnostics.Stopwatch;

namespace SimpleGame
{
    public class AnimatedBitmap
    {
        Bitmap[] images;
        int interval;
        Sw counter = new Sw();

        public AnimatedBitmap(Bitmap[] ims, int fps = 25)
        {
            images = ims;
            interval = 1000 / fps;
            counter.Start();
        }

        public Bitmap GetImage()
        {
            int imgnum = (int)(counter.ElapsedMilliseconds / interval % images.Length);
            return images[imgnum];
        }

        public void Flip()
        {
            foreach (var i in images) i.RotateFlip(RotateFlipType.RotateNoneFlipY);
        }

        public static implicit operator Bitmap(AnimatedBitmap a) => a.GetImage();
        public static implicit operator AnimatedBitmap(Bitmap b) => new AnimatedBitmap(new Bitmap[] {b}, 1);
    }
}
