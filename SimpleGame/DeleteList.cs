using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SimpleGame
{
    public class DeleteList<T> : List<T>, IDeleteList where T : IMarkDelete
    {        

        public DeleteList() {
            Values.DeleteLists.Add(this);
        }
        
        public void ClearList()
        {
            RemoveAll(x => x.IsMarkedKilled);
        }
    }
}
