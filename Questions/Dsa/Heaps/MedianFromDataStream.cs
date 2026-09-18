namespace CodingQuestions.Dsa.Heaps;

[Q(1_10_05, "Find Median from Data Stream", Hard,
"Numbers arrive one at a time. Support AddNum in O(log n) and FindMedian in O(1).")]
public static class MedianFromDataStream
{
    // Two heaps: `low` = max-heap of the smaller half, `high` = min-heap of the larger half.
    // Keep low.Count == high.Count or low.Count == high.Count + 1. Median lives at the tops.
    public class MedianFinder
    {
        readonly PriorityQueue<int, int> low = new(Comparer<int>.Create((a, b) => b.CompareTo(a)));
        readonly PriorityQueue<int, int> high = new();

        public void AddNum(int x)
        {
            low.Enqueue(x, x);
            int top = low.Dequeue();         // move low's max to high...
            high.Enqueue(top, top);
            if (high.Count > low.Count)      // ...then rebalance sizes
            {
                int m = high.Dequeue();
                low.Enqueue(m, m);
            }
        }

        public double FindMedian() => low.Count > high.Count ? low.Peek() : (low.Peek() + (double)high.Peek()) / 2;
    }

    public static void Run()
    {
        var mf = new MedianFinder();
        var medians = new List<double>();
        foreach (int x in new[] { 5, 15, 1, 3, 8, 7 })
        {
            mf.AddNum(x);
            medians.Add(mf.FindMedian());
        }
        Check("Add 5,15,1,3,8,7 → median after each", medians, [5.0, 10, 5, 4, 5, 6]);
    }
}
