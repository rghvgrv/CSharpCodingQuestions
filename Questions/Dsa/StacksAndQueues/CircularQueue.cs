namespace CSharpCodingQuestions.Questions.Dsa.StacksAndQueues;

[Question(Order = 7, Title = "Circular Queue (Ring Buffer)", Level = Medium, Problem = """
    Build a fixed-size queue on an array where the end **wraps around** to the start, so freed slots at the front are reused.
    `Enqueue` returns `false` when full, `Dequeue` returns `false` when empty, and `Front`/`Rear` read the ends.
    """)]
public static class CircularQueue
{
    [Approach(Name = "Array + Wrap-Around Index", Time = "O(1) per operation", Space = "O(capacity)", Idea = """
        Keep the index of the front item (`head`) and the number of items (`count`).

        - The next free slot is `(head + count) % capacity`. The `%` makes the index wrap back to 0 after the last slot.
        - `Dequeue` just moves `head` forward (also with `%`).

        No items ever shift, unlike removing from the front of a normal array.
        """)]
    public class RingBuffer(int capacity)
    {
        private readonly int[] slots = new int[capacity];
        private int head;
        private int count;

        public bool IsEmpty => count == 0;

        public bool IsFull => count == slots.Length;

        public int Front => IsEmpty ? -1 : slots[head];

        public int Rear => IsEmpty ? -1 : slots[(head + count - 1) % slots.Length];

        public bool Enqueue(int value)
        {
            if (IsFull)
            {
                return false;
            }
            slots[(head + count) % slots.Length] = value;
            count++;
            return true;
        }

        public bool Dequeue()
        {
            if (IsEmpty)
            {
                return false;
            }
            head = (head + 1) % slots.Length;
            count--;
            return true;
        }
    }

    public static void Demo()
    {
        var queue = new RingBuffer(capacity: 3);
        Print("Enqueue 1, 2, 3", new[] { queue.Enqueue(1), queue.Enqueue(2), queue.Enqueue(3) }, expected: new[] { true, true, true });
        Print("Enqueue 4 (full)", queue.Enqueue(4), expected: false);
        Print("Dequeue", queue.Dequeue(), expected: true);
        Print("Enqueue 4 (wraps into slot 0)", queue.Enqueue(4), expected: true);
        Print("Front", queue.Front, expected: 2);
        Print("Rear", queue.Rear, expected: 4);
    }
}
