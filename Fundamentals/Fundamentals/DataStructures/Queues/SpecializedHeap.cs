using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundamentals.DataStructures.Queues
{
    public class SpecializedHeap<Priority, Value> : IPriorityQueue<Priority, Value>
    {
        private ConcurrentDictionary<Priority, Queue<Value>> dataLookup = new ConcurrentDictionary<Priority, Queue<Value>>();
        private SimpleBinaryHeap<Priority, Queue<Value>> data;

        public SpecializedHeap(Func<Priority, Priority, int> compare, int initialSize = 1024)
        {
            data = new SimpleBinaryHeap<Priority, Queue<Value>>(compare, initialSize);
        }

        public void Push(Priority priority, Value item)
        {
            if (!this.dataLookup.ContainsKey(priority))
            {
                var newQueue = new Queue<Value>();
                newQueue.Enqueue(item);
                dataLookup.TryAdd(priority, newQueue);
                this.data.Push(priority, newQueue);
            }
            else if (dataLookup.TryGetValue(priority, out var existingQueue))
            {
                existingQueue.Enqueue(item);
            }
        }

        public (Priority priority, Value value) Peek()
        {
            if (this.data.Count() > 0)
            {
                var resultQueue = data.Peek();
                return (resultQueue.priority, resultQueue.value.Peek());
            }

            return (default(Priority), default(Value));
        }

        public (Priority priority, Value value) Pop()
        {
            if (this.data.Count() > 0)
            {
                var (priority, queue) = this.data.Peek();
                var result = queue.Dequeue();
                if (!queue.Any())
                {
                    this.data.Pop();
                }
                return (priority, result);
            }

            return (default(Priority), default(Value));
        }

        public int Count()
        {
            return this.data.Count();
        }
    }
}


