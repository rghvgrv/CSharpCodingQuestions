namespace CodingQuestions.Dsa.StacksAndQueues;

[Q(1_07_04, "Min Stack", Medium,
"Design a stack that supports Push, Pop, Top and GetMin, all in O(1).")]
public static class MinStack
{
    // Store (value, minimum so far) on every entry. The min is always on top.
    public class MyMinStack
    {
        readonly Stack<(int Value, int Min)> stack = new();

        public void Push(int x) => stack.Push((x, stack.Count == 0 ? x : Math.Min(x, stack.Peek().Min)));
        public void Pop() => stack.Pop();
        public int Top() => stack.Peek().Value;
        public int GetMin() => stack.Peek().Min;
    }

    public static void Run()
    {
        var s = new MyMinStack();
        s.Push(-2); s.Push(0); s.Push(-3);
        Check("GetMin", s.GetMin(), -3);
        s.Pop();
        Check("Top", s.Top(), 0);
        Check("GetMin", s.GetMin(), -2);
    }
}
