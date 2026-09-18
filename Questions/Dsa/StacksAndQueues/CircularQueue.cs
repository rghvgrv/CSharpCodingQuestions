namespace CodingQuestions.Dsa.StacksAndQueues;

[Q(1_07_07, "Circular Queue (Ring Buffer)", Medium,
"Implement a fixed-size queue on an array where the end wraps around to the start. Support Enqueue, Dequeue, Front, Rear, IsEmpty and IsFull.")]
public static class CircularQueue
{
    public class RingBuffer(int capacity)
    {
        readonly int[] data = new int[capacity];
        int head, count; // head = index of the front item

        public bool IsEmpty => count == 0;
        public bool IsFull => count == data.Length;

        public bool Enqueue(int x)
        {
            if (IsFull) return false;
            data[(head + count++) % data.Length] = x; // wrap with modulo
            return true;
        }

        public bool Dequeue()
        {
            if (IsEmpty) return false;
            head = (head + 1) % data.Length;
            count--;
            return true;
        }

        public int Front => IsEmpty ? -1 : data[head];
        public int Rear => IsEmpty ? -1 : data[(head + count - 1) % data.Length];
    }

    public static void Run()
    {
        var q = new RingBuffer(3);
        Check("Enqueue 1,2,3", new[] { q.Enqueue(1), q.Enqueue(2), q.Enqueue(3) }, [true, true, true]);
        Check("Enqueue 4 when full", q.Enqueue(4), false);
        Check("Rear", q.Rear, 3);
        Check("IsFull", q.IsFull, true);
        Check("Dequeue", q.Dequeue(), true);
        Check("Enqueue 4 (wraps to index 0)", q.Enqueue(4), true);
        Check("Front, Rear", (q.Front, q.Rear), (2, 4));
    }
}
