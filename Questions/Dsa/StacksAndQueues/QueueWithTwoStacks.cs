namespace CSharpCodingQuestions.Questions.Dsa.StacksAndQueues;

[Question(Order = 2, Title = "Queue Using Two Stacks", Level = Easy, Problem = """
    Build a first-in-first-out queue (`Enqueue`, `Dequeue`) using only two stacks.
    """)]
public static class QueueWithTwoStacks
{
    [Approach(Name = "Pour Everything Back and Forth", Time = "Dequeue O(n)", Space = "O(n)", Idea = """
        Keep items in `main`, with the newest on top. To dequeue, pour everything into `helper` (reversing the order, so the oldest is on top),
        pop it, then pour everything back. Every dequeue moves all the items twice.
        """)]
    public class SlowQueue
    {
        private readonly Stack<int> main = new();
        private readonly Stack<int> helper = new();

        public void Enqueue(int value) => main.Push(value);

        public int Dequeue()
        {
            while (main.Count > 0)
            {
                helper.Push(main.Pop());
            }
            int oldest = helper.Pop();
            while (helper.Count > 0)
            {
                main.Push(helper.Pop());
            }
            return oldest;
        }
    }

    [Approach(Name = "Inbox and Outbox", Time = "O(1) on average", Space = "O(n)", Idea = """
        New items go onto `inbox`. `Dequeue` pops from `outbox`. Only when `outbox` is **empty** do we pour `inbox` into it.
        That reverses the order once, and the items then stay in `outbox` until they leave.

        Each item is moved at most once, so on average every operation is `O(1)`.
        """)]
    public class FastQueue
    {
        private readonly Stack<int> inbox = new();
        private readonly Stack<int> outbox = new();

        public void Enqueue(int value) => inbox.Push(value);

        public int Dequeue()
        {
            if (outbox.Count == 0)
            {
                while (inbox.Count > 0)
                {
                    outbox.Push(inbox.Pop());
                }
            }
            return outbox.Pop();
        }

        public bool IsEmpty => inbox.Count == 0 && outbox.Count == 0;
    }

    public static void Demo()
    {
        var queue = new FastQueue();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);
        Print("Enqueue 1, 2, 3 then Dequeue", queue.Dequeue(), expected: 1);
        queue.Enqueue(4);
        Print("Enqueue 4 then Dequeue", queue.Dequeue(), expected: 2);
        Print("Dequeue", queue.Dequeue(), expected: 3);
        Print("Dequeue", queue.Dequeue(), expected: 4);
        Print("IsEmpty", queue.IsEmpty, expected: true);

        var slow = new SlowQueue();
        slow.Enqueue(7);
        slow.Enqueue(8);
        Print("SlowQueue Dequeue", slow.Dequeue(), expected: 7);
    }
}
