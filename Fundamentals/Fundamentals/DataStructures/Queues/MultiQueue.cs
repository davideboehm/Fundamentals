using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fundamentals.DataStructures.Queues
{
    public class MultiQueue<T> where T : IComparable<T>
    {
        private FunctionalPairingHeap<T>[] Queues;
        private int[] QueueLocks;
        private int NumberOfQueues;
        private static Random Random = new Random();

        public MultiQueue(int numberOfQueues)
        {
            if (numberOfQueues <= 0)
            {
                throw new ArgumentException("The number of queues that the multi queue will store must be greater than 0", nameof(numberOfQueues));
            }
            this.NumberOfQueues = numberOfQueues;
            this.Queues = new FunctionalPairingHeap<T>[numberOfQueues];
            this.QueueLocks = new int[numberOfQueues];
            for (int i = 0; i < numberOfQueues; i++)
            {
                this.Queues[i] = FunctionalPairingHeap<T>.Empty;
            }
        }

        public T Dequeue()
        {
            while (true)
            {
                int i = Random.Next(this.NumberOfQueues);
                int j = Random.Next(this.NumberOfQueues);
                if(i==j)
                {
                    if (j < this.NumberOfQueues - 1)
                    {
                        j++;
                    }
                    else
                    {
                        j--;
                    }
                }
                if ((this.Queues[i] == FunctionalPairingHeap<T>.Empty && this.Queues[j] == FunctionalPairingHeap<T>.Empty) ||
                    (i == j))
                {
                    throw new Exception();
                }
                else if(this.Queues[i] == FunctionalPairingHeap<T>.Empty || 
                    (this.Queues[j] != FunctionalPairingHeap<T>.Empty && this.Queues[j].FindMin().CompareTo(this.Queues[i].FindMin()) < 0))
                {
                    i = j;
                }

                using (var queue = this.GetLockedQueue(i))
                {
                    if (queue.HasValue)
                    {
                        var result = queue.Value.Queue.FindMin();
                        this.Queues[i] = queue.Value.Queue.DeleteMin();
                        return result;
                    }
                }
            }
        }

        public T Peek()
        {
            using (var queue1 = this.GetLockedQueue())
            {
                using (var queue2 = this.GetLockedQueue())
                {
                    return (queue1.Queue.FindMin().CompareTo(queue2.Queue.FindMin()) < 0) ?
                        queue1.Queue.FindMin() :
                        queue2.Queue.FindMin();
                }
            }
        }

        public void Enqueue(T value)
        {
            using (var queue = this.GetLockedQueue())
            {
                this.Queues[queue.index] = queue.Queue.Insert(value);
            }
        }

        private LockedQueue? GetLockedQueue(int index)
        {
            if (Interlocked.CompareExchange(ref this.QueueLocks[index], 1, 0) == 0)
            {
                return new LockedQueue(ref index, this.Queues[index], this.QueueLocks);
            }
            return null;
        }

        private LockedQueue GetLockedQueue()
        {
            var index = MultiQueue<T>.Random.Next(this.NumberOfQueues);
            while (true)
            {
                if (Interlocked.CompareExchange(ref this.QueueLocks[index], 1, 0) == 0)
                {
                    return new LockedQueue(ref index, this.Queues[index], this.QueueLocks);
                }
                index++;
                if(index == this.NumberOfQueues)
                {
                    index = 0;
                }
            }
        }

        private struct LockedQueue : IDisposable
        {
            public int index;
            private readonly int[] locks;
            public readonly FunctionalPairingHeap<T> Queue;

            public LockedQueue(ref int index, FunctionalPairingHeap<T> queue, int[] locks)
            {
                this.index = index;
                this.Queue = queue;
                this.locks = locks;
            }

            public void Dispose()
            {
                this.locks[this.index] = 0;
            }
        }
    }
}
