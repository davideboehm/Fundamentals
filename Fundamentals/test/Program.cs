using Fundamentals.DataStructures.Queues;
using Fundamentals.Timing.TaskScheduling;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace test
{
    class Program
    {

        static List<int> numbers = new List<int>();
        static void Main(string[] args)
        {
            Stopwatch sw1 = new Stopwatch();
            Stopwatch sw2 = new Stopwatch();
            var rand = new Random();
            var heap1 = FunctionalPairingHeap<int>.Empty;
            var heap1Lock = new object();
            var heap2Lock = new object();
            var heap2 = new MultiQueue<int>(2);
            int limit = 3000000;
            List<int> numbers = new List<int>();
            for (int i = 0; i < limit; i++)
            {
                numbers.Add(rand.Next(limit / 5));
                numbers.Add(rand.Next(limit / 5));
                numbers.Add(rand.Next(limit / 5));
            }

            var list1 = new List<int>();

            sw1.Start();
            Parallel.For(0, limit, i => 
            {
                lock (heap1Lock)
                {
                    heap1 = heap1.Insert(numbers[i]);
                }
                lock (heap1Lock)
                {
                    heap1 = heap1.Insert(numbers[i + limit]);
                }
            });
            
            Parallel.For(0, limit, i =>
            {
                lock (heap1Lock)
                {
                    list1.Add(heap1.FindMin());
                }
                lock (heap1Lock)
                {
                    heap1 = heap1.DeleteMin();
                }
                lock (heap1Lock)
                {
                    heap1 = heap1.Insert(numbers[i + limit * 2]);
                }
            });
            sw1.Stop();

            var list2 = new List<int>();

            Parallel.For(0, limit, i =>
            {
                heap2.Enqueue(numbers[i]);
                heap2.Enqueue(numbers[i + limit]);
                heap2.Dequeue();
            });
            sw2.Start();

            Parallel.For(0, limit, i =>
            {
                lock (heap2Lock)
                {
                    list2.Add(heap2.Dequeue());
                }
                heap2.Enqueue(numbers[i + limit * 2]);
            });
            sw2.Stop();

            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i] != list2[i])
                {
                    throw new Exception();
                }
            }
        }
    }
}
