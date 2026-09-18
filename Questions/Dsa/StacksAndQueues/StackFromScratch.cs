namespace CodingQuestions.Dsa.StacksAndQueues;

[Q(1_07_01, "Implement a Stack from Scratch", Easy,
"Build a generic stack (LIFO: last in, first out) on a resizable array with Push, Pop, Peek and Count.")]
public static class StackFromScratch
{
    public class MyStack<T>
    {
        T[] items = new T[4];
        public int Count { get; private set; }

        public void Push(T item) // amortized O(1): double the array when full
        {
            if (Count == items.Length) Array.Resize(ref items, items.Length * 2);
            items[Count++] = item;
        }

        public T Pop()
        {
            if (Count == 0) throw new InvalidOperationException("Stack is empty");
            T item = items[--Count];
            items[Count] = default!; // let the GC collect it
            return item;
        }

        public T Peek() => Count == 0 ? throw new InvalidOperationException("Stack is empty") : items[Count - 1];
    }

    public static void Run()
    {
        var s = new MyStack<string>();
        foreach (var x in new[] { "a", "b", "c", "d", "e" }) s.Push(x);
        Check("Push a..e, Peek", s.Peek(), "e");
        Check("Pop, Pop", $"{s.Pop()}{s.Pop()}", "ed");
        Check("Count", s.Count, 3);

        try { new MyStack<int>().Pop(); }
        catch (InvalidOperationException e) { Check("Pop on empty throws", e.Message, "Stack is empty"); }

        // Built-in: System.Collections.Generic.Stack<T>
        var builtIn = new Stack<int>([1, 2, 3]);
        Check("Stack<int>([1,2,3]).Pop()", builtIn.Pop(), 3);
    }
}
