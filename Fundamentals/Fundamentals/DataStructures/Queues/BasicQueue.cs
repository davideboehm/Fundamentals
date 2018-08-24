using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundamentals.DataStructures.Queues
{
    public class BasicQueue<T> : Queue<T>, IQueue<T>
    {
        public T Pop()
        {
            return base.Dequeue();
        }

        public void Push(T item)
        {
            base.Enqueue(item);
        }

        int IQueue<T>.Count()
        {
            return base.Count;
        }
    }
}
