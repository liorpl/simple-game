using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SimpleGame
{    

    public class Block : IMarkDelete
    {
        public Rectangle Rect { get; private set; }
        public bool Fragile { get; private set; }

        public bool IsMarkedKilled
        {
            get; private set;
        }

        public Block(Rectangle r, bool f = false)
        {
            Rect = r;
            Fragile = f;
        }

        public void Clear() { IsMarkedKilled = true; /*Rect = Rectangle.Empty;*/ }

        public static implicit operator Block(Rectangle r) => new Block(r);
    }
}
