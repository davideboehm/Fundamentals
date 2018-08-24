using System;

namespace Fundamentals.DataStructures.Queues
{
    public class FunctionalPairingHeap2<T> where T : IComparable<T>
    {
        private readonly PairingTree value;

        public static readonly FunctionalPairingHeap2<T> Empty = new FunctionalPairingHeap2<T>(default, null);

        private FunctionalPairingHeap2(T value, Node subHeaps)
        {
            this.value = new PairingTree(value, subHeaps);
        }

        private FunctionalPairingHeap2(PairingTree value)
        {
            this.value = value;
        }

        public T FindMin()
        {
            return FindMin(this);
        }

        public FunctionalPairingHeap2<T> Insert(T value)
        {
            return Insert(this, value);
        }

        public FunctionalPairingHeap2<T> DeleteMin()
        {
            return DeleteMin(this);
        }

        public static T FindMin(FunctionalPairingHeap2<T> heap)
        {
            if (heap == null || heap == Empty)
            {
                throw new ArgumentNullException();
            }

            return heap.value.elem;
        }

        private static PairingTree Merge(PairingTree heap1, PairingTree heap2)
        {
            if (heap1.elem.CompareTo(heap2.elem) < 0)
            {
                return new PairingTree(heap1.elem, new Node(
                    heap2,
                    heap1.subHeaps));
            }
            return new PairingTree(heap2.elem, new Node(
                heap1,
                heap2.subHeaps));
        }

        public static FunctionalPairingHeap2<T> Insert(FunctionalPairingHeap2<T> heap, T value)
        {
            if (heap == Empty)
            {
                return new FunctionalPairingHeap2<T>(value, null); ;
            }
            else if (value.CompareTo(heap.value.elem) < 0)
            {
                return new FunctionalPairingHeap2<T>(value, new Node(
                    heap.value,
                    null));
            }

            return new FunctionalPairingHeap2<T>(heap.value.elem, new Node(
                new PairingTree(value, null),
                heap.value.subHeaps));
        }

        public static FunctionalPairingHeap2<T> DeleteMin(FunctionalPairingHeap2<T> heap)
        {
            if (heap == null || heap == Empty)
            {
                throw new Exception();
            }

            return MergePairs(heap.value.subHeaps);
        }

        private static FunctionalPairingHeap2<T> MergePairs(Node subHeaps)
        {
            if (subHeaps == null)
            {
                return FunctionalPairingHeap2<T>.Empty;
            }
            else if (subHeaps.Next == null)
            {
                return new FunctionalPairingHeap2<T>(subHeaps.Value);
            }

            PairingTree currentResult = (Merge(subHeaps.Value, subHeaps.Next.Value));
            Node second;
            for (var first = subHeaps.Next.Next; first != null; first = second?.Next)
            {
                second = first.Next;
                if (second != null)
                {
                    currentResult = Merge(currentResult, Merge(first.Value, second.Value));
                }
                else
                {
                    currentResult = Merge(currentResult, first.Value);
                }
            }

            return new FunctionalPairingHeap2<T>(currentResult);

        }

        private struct PairingTree
        {
            public readonly T elem;
            public readonly Node subHeaps;

            public PairingTree(T value, Node subHeaps)
            {
                this.elem = value;
                this.subHeaps = subHeaps;
            }
        }

        private class Node
        {
            public readonly Node Next;
            public readonly PairingTree Value;

            public Node(PairingTree value, Node next)
            {
                this.Value = value;
                this.Next = next;
            }
        }
    }
}
