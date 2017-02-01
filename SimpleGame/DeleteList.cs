using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SimpleGame
{
    public class DeleteList<T> : List<T> where T : IMarkDelete
    {
        Timer timer1 = new Timer(10);

        public DeleteList()
        {
            timer1.Elapsed += Timer1_Elapsed;
            timer1.Start();
        }

        private void Timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            for(int i = 0; i < Count; i++)
            {
                if (this[i].IsMarkedKilled)
                    Remove(this[i]);
            }
        }
    }
}
