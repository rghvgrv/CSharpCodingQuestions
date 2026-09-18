namespace CSharpCodingQuestions.Questions.Dsa.StacksAndQueues;

[Question(Order = 4, Title = "Min Stack", Level = Medium, Problem = """
    Build a stack that also has `GetMin()`, which returns the smallest item currently in the stack, as fast as possible.
    """)]
public static class MinStack
{
    [Approach(Name = "Search for the Minimum", Time = "GetMin O(n)", Space = "O(n)", Idea = """
        Use a normal stack, and have `GetMin` look through every item.
        """)]
    public class SearchingMinStack
    {
        private readonly Stack<int> items = new();

        public void Push(int value) => items.Push(value);

        public void Pop() => items.Pop();

        public int GetMin() => items.Min();
    }

    [Approach(Name = "Remember the Minimum With Each Item", Time = "O(1) for everything", Space = "O(n)", Idea = """
        Store pairs: each item together with **the minimum at the moment it was pushed**.
        The top pair always knows the current minimum. When you pop, the pair below still knows the minimum from before.
        """)]
    public class FastMinStack
    {
        private readonly Stack<(int Value, int MinSoFar)> items = new();

        public void Push(int value)
        {
            int minSoFar = items.Count == 0 ? value : Math.Min(value, items.Peek().MinSoFar);
            items.Push((value, minSoFar));
        }

        public void Pop() => items.Pop();

        public int Top() => items.Peek().Value;

        public int GetMin() => items.Peek().MinSoFar;
    }

    public static void Demo()
    {
        var stack = new FastMinStack();
        stack.Push(5);
        stack.Push(3);
        stack.Push(7);
        stack.Push(2);
        Print("Push 5, 3, 7, 2 → GetMin", stack.GetMin(), expected: 2);
        stack.Pop();
        Print("Pop → GetMin", stack.GetMin(), expected: 3);
        Print("Top", stack.Top(), expected: 7);
        stack.Pop();
        stack.Pop();
        Print("Pop, Pop → GetMin", stack.GetMin(), expected: 5);

        var searching = new SearchingMinStack();
        searching.Push(4);
        searching.Push(1);
        Print("SearchingMinStack GetMin", searching.GetMin(), expected: 1);
    }
}
