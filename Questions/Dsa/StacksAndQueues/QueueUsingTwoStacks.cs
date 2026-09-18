namespace CodingQuestions.Dsa.StacksAndQueues;

[Q(1_07_02, "Queue Using Two Stacks", Easy,
"Implement a FIFO queue (Enqueue, Dequeue, Peek) using only two stacks.")]
public static class QueueUsingTwoStacks
{
    // New items go on `inbox`. When `outbox` is empty, pour inbox into it, which reverses the order to FIFO.
    // Each item moves at most once → amortized O(1) per operation.
    public class MyQueue
    {
        readonly Stack<int> inbox = new(), outbox = new();

        public void Enqueue(int x) => inbox.Push(x);
        public int Dequeue() { Shift(); return outbox.Pop(); }
        public int Peek() { Shift(); return outbox.Peek(); }
        public bool IsEmpty => inbox.Count == 0 && outbox.Count == 0;

        void Shift()
        {
            if (outbox.Count == 0)
                while (inbox.Count > 0) outbox.Push(inbox.Pop());
        }
    }

    public static void Run()
    {
        var q = new MyQueue();
        q.Enqueue(1); q.Enqueue(2);
        Check("Peek", q.Peek(), 1);
        Check("Dequeue", q.Dequeue(), 1);
        q.Enqueue(3);
        Check("Dequeue", q.Dequeue(), 2);
        Check("Dequeue", q.Dequeue(), 3);
        Check("IsEmpty", q.IsEmpty, true);
    }
}
