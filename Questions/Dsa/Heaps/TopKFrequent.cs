namespace CodingQuestions.Dsa.Heaps;

[Q(1_10_03, "Top K Frequent Elements", Medium,
"Return the k most frequent elements. Solve it with a heap (O(n log k)) and with bucket sort (O(n)).")]
public static class TopKFrequent
{
    public static List<int> WithHeap(int[] nums, int k)
    {
        var freq = nums.CountBy(x => x);
        var heap = new PriorityQueue<int, int>(); // min-heap by frequency, size k
        foreach (var (value, count) in freq)
        {
            heap.Enqueue(value, count);
            if (heap.Count > k) heap.Dequeue();
        }
        var result = new List<int>();
        while (heap.Count > 0) result.Add(heap.Dequeue());
        result.Reverse();
        return result;
    }

    // bucket[f] = values that appear exactly f times. Walk buckets from high to low.
    public static List<int> WithBuckets(int[] nums, int k)
    {
        var buckets = new List<int>[nums.Length + 1];
        foreach (var (value, count) in nums.CountBy(x => x)) (buckets[count] ??= []).Add(value);
        var result = new List<int>();
        for (int f = nums.Length; f > 0 && result.Count < k; f--)
            if (buckets[f] != null) result.AddRange(buckets[f]);
        return result.Take(k).ToList();
    }

    public static void Run()
    {
        Check("WithHeap([1,1,1,2,2,3], k=2)", WithHeap([1, 1, 1, 2, 2, 3], 2), [1, 2]);
        Check("WithBuckets([1,1,1,2,2,3], k=2)", WithBuckets([1, 1, 1, 2, 2, 3], 2), [1, 2]);
        Check("WithBuckets([4,4,4,4,7,7,9], k=1)", WithBuckets([4, 4, 4, 4, 7, 7, 9], 1), [4]);
    }
}
