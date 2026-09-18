namespace CSharpCodingQuestions.Questions.Dsa.Heaps;

[Question(Order = 1, Title = "Build Your Own Min-Heap", Level = Medium, Problem = """
    Build a priority queue with `Push(value)`, `Pop()` (remove and return the smallest) and `Peek()`.
    """)]
public static class BuildAMinHeap
{
    [Approach(Name = "Keep a Sorted List", Time = "Push O(n), Pop O(1)", Space = "O(n)", Idea = """
        Keep the values in a list sorted from **largest to smallest**, so the smallest is at the end and removing it is cheap.
        But every `Push` must find its place and shift items over, which is slow.
        """)]
    public class SortedListQueue
    {
        private readonly List<int> values = new();

        public void Push(int value)
        {
            int index = 0;
            while (index < values.Count && values[index] > value)
            {
                index++;
            }
            values.Insert(index, value);
        }

        public int Pop()
        {
            int smallest = values[^1];
            values.RemoveAt(values.Count - 1);
            return smallest;
        }
    }

    [Approach(Name = "Binary Heap in an Array", Time = "Push / Pop O(log n), Peek O(1)", Space = "O(n)", Idea = """
        A heap is a tree stored in an array: the parent of index `i` is `(i - 1) / 2`, and its children are `2i + 1` and `2i + 2`.
        Rule: **every parent ≤ its children**, so the smallest value is always at index 0.

        - **Push**: add at the end, then **bubble up**: swap with the parent while the parent is bigger.
        - **Pop**: take index 0, move the last item to the top, then **sink down**: swap with the smaller child while a child is smaller.

        The tree is only `log n` levels tall, so both take `O(log n)`. .NET's `PriorityQueue` works this way.
        """)]
    public class MinHeap
    {
        private readonly List<int> items = new();

        public int Count => items.Count;

        public int Peek() => items[0];

        public void Push(int value)
        {
            items.Add(value);
            int child = items.Count - 1;
            while (child > 0)
            {
                int parent = (child - 1) / 2;
                if (items[parent] <= items[child])
                {
                    break;
                }
                (items[parent], items[child]) = (items[child], items[parent]);   // swap
                child = parent;
            }
        }

        public int Pop()
        {
            int smallest = items[0];
            items[0] = items[^1];
            items.RemoveAt(items.Count - 1);

            int parent = 0;
            while (true)
            {
                int left = 2 * parent + 1;
                int right = 2 * parent + 2;
                int smaller = parent;
                if (left < items.Count && items[left] < items[smaller])
                {
                    smaller = left;
                }
                if (right < items.Count && items[right] < items[smaller])
                {
                    smaller = right;
                }
                if (smaller == parent)
                {
                    break;
                }
                (items[parent], items[smaller]) = (items[smaller], items[parent]);   // swap
                parent = smaller;
            }
            return smallest;
        }
    }

    public static void Demo()
    {
        var heap = new MinHeap();
        foreach (int value in new[] { 5, 3, 8, 1, 9, 2 })
        {
            heap.Push(value);
        }
        Print("Push 5, 3, 8, 1, 9, 2 → Peek", heap.Peek(), expected: 1);

        var popped = new List<int>();
        while (heap.Count > 0)
        {
            popped.Add(heap.Pop());
        }
        Print("Pop until empty", popped, expected: new[] { 1, 2, 3, 5, 8, 9 });

        var sorted = new SortedListQueue();
        sorted.Push(4);
        sorted.Push(2);
        sorted.Push(6);
        Print("SortedListQueue Pop", sorted.Pop(), expected: 2);

        var builtIn = new PriorityQueue<string, int>();
        builtIn.Enqueue("low priority", 5);
        builtIn.Enqueue("urgent", 1);
        Print("Built-in PriorityQueue Dequeue", builtIn.Dequeue(), expected: "urgent");
    }
}
