using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundamentals.DataStructures.Queues
{
    public interface IQueue<Value>
    {       
        void Push(Value item);
        Value Pop();
        Value Peek();
        int Count();
    }
    public interface IPriorityQueue<Priority, Value>
    {
        void Push(Priority priority, Value item);
        (Priority priority, Value value) Pop();
        (Priority priority, Value value) Peek();
        int Count();
    }
}
