using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundamentals.DataStructures.Queues
{
    public class SimpleBinaryHeap<Value> : IQueue<Value>
    {
        private SimpleBinaryHeap<Value, Value> data;
        public SimpleBinaryHeap(Func<Value, Value, int> compare, int initialSize = 1024)
        {
            this.data = new SimpleBinaryHeap<Value, Value>(compare, initialSize);
        }

        public SimpleBinaryHeap(IComparer<Value> comparer, int initialSize = 1024)
        {
            this.data = new SimpleBinaryHeap<Value, Value>(comparer, initialSize);
        }

        public int Count()
        {
            return this.data.Count();
        }

        public Value Peek()
        {
            return this.data.Peek().value;
        }

        public Value Pop()
        {
            return this.data.Pop().value;
        }

        public void Push(Value item)
        {
            this.data.Push(item, item);
        }
    }

    public static class SimpleBinaryHeap
    {
        public static Func<T,T,int> Max<T>() where T:IComparable<T>
        {
            return (T item1, T item2) => item2.CompareTo(item1); 
        }
        public static Func<T, T, int> Min<T>() where T : IComparable<T>
        {
            return (T item1, T item2) => item1.CompareTo(item2);
        }
    }

    public class SimpleBinaryHeap<Priority, Value> : IPriorityQueue<Priority, Value>, IQueue<(Priority, Value)>
    {
        private object dataLock = new object();

        protected (Priority, Value)[] data;
        protected Func<Priority, Priority, int> Compare;
        private int count = 0;
        public int Count()
        {
            return this.count;
        }
        
        public SimpleBinaryHeap(Func<Priority, Priority, int> compare, int initialSize = 1024)
        {
            this.data = new (Priority, Value)[initialSize];
            this.Compare = compare;
        }

        public SimpleBinaryHeap(IComparer<Priority> comparer, int initialSize = 1024) : this((x, y) => comparer.Compare(x, y), initialSize)
        {
        }
        
        public virtual void Push(Priority priority, Value item)
        {
            lock (dataLock)
            {
                if (this.Count() == this.data.Length)
                {
                    this.Resize();
                }
                data[this.Count()] = (priority, item);
                SiftUp(this.Count());
                this.count++;
            }
        }

        public (Priority priority, Value value) Peek()
        {
            lock (dataLock)
            {
                if (this.Count() > 0)
                {
                    var (priority, result) = data[0];
                    return (priority, result);
                }

                return (default(Priority), default(Value));
            }
        }

        public (Priority priority, Value value) Pop()
        {
            lock (dataLock)
            {
                if (this.Count() > 0)
                {
                    var (priority, result) = data[0];
                    data[0] = data[this.Count() - 1];
                    data[this.Count() - 1] = (default(Priority), default(Value));
                    this.count--;
                    this.SiftDown(0);
                    return (priority, result);
                }
            }

            return (default(Priority), default(Value));            
        }
        
        private void Resize()
        {
            lock (dataLock)
            {
                var biggerArray = new(Priority, Value)[data.Length * 2];
                Array.Copy(data, biggerArray, data.Length);
                data = biggerArray;
            }
        }

        private void SiftUp(int index)
        {
            if (index > 0)
            {
                lock (dataLock)
                {
                    var parentIndex = (index-1)/2; 
                    var (priority, item) = data[index];
                    var (otherPriority, otherItem) = data[parentIndex];
                    if (this.Compare(priority, otherPriority) < 0)
                    {
                        data[index] = (otherPriority, otherItem);
                        data[parentIndex] = (priority, item);
                        this.SiftUp(parentIndex);
                    }
                }
            }
        }

        private void SiftDown(int index)
        {
            var (currentPriority, currentValue) = data[index];
            if (this.Count() > 2 * index + 1)
            {
                var (leftPriority, leftValue, leftIndex) = (data[2 * index + 1].Item1, data[2 * index + 1].Item2, 2 * index + 1);
                var itemGoesAfterLeft = this.Compare(currentPriority, leftPriority) > 0;

                if (this.Count() > 2 * index + 2)
                {
                    var (rightPriority, rightValue, rightIndex) = (data[2 * index + 2].Item1, data[2 * index + 2].Item2, 2 * index + 2);
                    var itemGoesAfterRight = this.Compare(currentPriority, rightPriority) > 0;
                    var leftSmallerThanRight = this.Compare(leftPriority, rightPriority) < 0;
                    if (itemGoesAfterLeft && leftSmallerThanRight)
                    {
                        data[index] = (leftPriority, leftValue);
                        data[leftIndex] = (currentPriority, currentValue);
                        this.SiftDown(leftIndex);
                    }
                    else if(itemGoesAfterRight && !leftSmallerThanRight)
                    {
                        data[index] = (rightPriority, rightValue);
                        data[rightIndex] = (currentPriority, currentValue);
                        this.SiftDown(rightIndex);
                    }
                }
                else if (itemGoesAfterLeft)
                {
                    data[index] = (leftPriority, leftValue);
                    data[leftIndex] = (currentPriority, currentValue);
                    this.SiftDown(leftIndex);
                }
            }
        }

        public void Push((Priority, Value) item)
        {
            this.Push(item.Item1, item.Item2);
        }

        (Priority, Value) IQueue<(Priority, Value)>.Pop()
        {
            return this.Pop();
        }

        (Priority, Value) IQueue<(Priority, Value)>.Peek()
        {
            return this.Peek();
        }
    }    
}


