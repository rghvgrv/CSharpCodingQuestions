namespace CodingQuestions.Dsa.Heaps;

[Q(1_10_01, "Implement a Min-Heap / Priority Queue", Medium,
"Build a binary min-heap with Push, Pop and Peek in O(log n). Then use .NET's built-in PriorityQueue<TElement, TPriority>.")]
public static class MinHeapFromScratch
{
    // Complete binary tree stored in a list: parent(i) = (i-1)/2, children = 2i+1, 2i+2.
    // Invariant: every parent <= its children, so the minimum is at index 0.
    public class MinHeap
    {
        readonly List<int> a = [];
        public int Count => a.Count;
        public int Peek() => a[0];

        public void Push(int x)
        {
            a.Add(x);
            for (int i = a.Count - 1; i > 0 && a[(i - 1) / 2] > a[i]; i = (i - 1) / 2) // sift up
                (a[i], a[(i - 1) / 2]) = (a[(i - 1) / 2], a[i]);
        }

        public int Pop()
        {
            int min = a[0];
            a[0] = a[^1];
            a.RemoveAt(a.Count - 1);
            for (int i = 0; ;) // sift down
            {
                int smallest = i, l = 2 * i + 1, r = l + 1;
                if (l < a.Count && a[l] < a[smallest]) smallest = l;
                if (r < a.Count && a[r] < a[smallest]) smallest = r;
                if (smallest == i) break;
                (a[i], a[smallest]) = (a[smallest], a[i]);
                i = smallest;
            }
            return min;
        }
    }

    public static void Run()
    {
        var heap = new MinHeap();
        foreach (int x in new[] { 5, 3, 8, 1, 9, 2 }) heap.Push(x);
        Check("Peek", heap.Peek(), 1);
        var popped = new List<int>();
        while (heap.Count > 0) popped.Add(heap.Pop());
        Check("Pop all", popped, [1, 2, 3, 5, 8, 9]);

        // Built-in (.NET 6+): element + priority, lowest priority dequeued first
        var pq = new PriorityQueue<string, int>();
        pq.Enqueue("low", 5); pq.Enqueue("urgent", 1); pq.Enqueue("normal", 3);
        Check("PriorityQueue order", new[] { pq.Dequeue(), pq.Dequeue(), pq.Dequeue() }, ["urgent", "normal", "low"]);

        // Max-heap: reverse the comparer
        var maxPq = new PriorityQueue<int, int>(Comparer<int>.Create((x, y) => y.CompareTo(x)));
        foreach (int x in new[] { 5, 3, 8 }) maxPq.Enqueue(x, x);
        Check("Max-heap Dequeue", maxPq.Dequeue(), 8);
    }
}
