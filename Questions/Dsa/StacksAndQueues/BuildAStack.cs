namespace CSharpCodingQuestions.Questions.Dsa.StacksAndQueues;

[Question(Order = 1, Title = "Build Your Own Stack", Level = Easy, Problem = """
    Build a stack of integers with `Push`, `Pop`, `Peek` and `Count`, using an array inside.
    The last item pushed is the first one popped (LIFO).
    """)]
public static class BuildAStack
{
    [Approach(Name = "Fixed-Size Array", Time = "O(1) per operation", Space = "O(capacity)", Idea = """
        Keep an array and a `count`. `Push` writes at `items[count]` and increases `count`; `Pop` does the reverse.
        Problem: once the array is full, you can't push anymore.
        """)]
    public class FixedStack(int capacity)
    {
        private readonly int[] items = new int[capacity];
        private int count;

        public void Push(int value)
        {
            if (count == items.Length)
            {
                throw new InvalidOperationException("Stack is full");
            }
            items[count] = value;
            count++;
        }

        public int Pop()
        {
            if (count == 0)
            {
                throw new InvalidOperationException("Stack is empty");
            }
            count--;
            return items[count];
        }
    }

    [Approach(Name = "Growing Array", Time = "O(1) on average", Space = "O(n)", Idea = """
        Same idea, but when the array is full, copy everything into a new array **twice as big**.
        Copying is slow, but it happens so rarely (at sizes 4, 8, 16, 32, …) that the average cost per push is still `O(1)`.
        That's how `List<T>` and `Stack<T>` work in .NET.
        """)]
    public class GrowingStack
    {
        private int[] items = new int[4];

        public int Count { get; private set; }

        public void Push(int value)
        {
            if (Count == items.Length)
            {
                int[] bigger = new int[items.Length * 2];
                Array.Copy(items, bigger, items.Length);
                items = bigger;
            }
            items[Count] = value;
            Count++;
        }

        public int Pop()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Stack is empty");
            }
            Count--;
            return items[Count];
        }

        public int Peek()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Stack is empty");
            }
            return items[Count - 1];
        }
    }

    public static void Demo()
    {
        var stack = new GrowingStack();
        for (int i = 1; i <= 10; i++)
        {
            stack.Push(i * 10);
        }
        Console.WriteLine("Pushed 10, 20, …, 100 (the array grew from 4 to 8 to 16).");
        Print("Peek", stack.Peek(), expected: 100);
        Print("Pop", stack.Pop(), expected: 100);
        Print("Pop", stack.Pop(), expected: 90);
        Print("Count", stack.Count, expected: 8);

        var small = new FixedStack(capacity: 2);
        small.Push(1);
        small.Push(2);
        try
        {
            small.Push(3);
        }
        catch (InvalidOperationException error)
        {
            Print("FixedStack(2) after a third Push", error.Message, expected: "Stack is full");
        }
    }
}
