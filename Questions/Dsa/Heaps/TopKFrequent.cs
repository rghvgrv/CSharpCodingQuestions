namespace CSharpCodingQuestions.Questions.Dsa.Heaps;

[Question(Order = 3, Title = "Top K Most Frequent Numbers", Level = Medium, Problem = """
    Return the `k` numbers that appear most often.
    `numbers = [1, 1, 1, 2, 2, 3]`, `k = 2` → `[1, 2]`.
    """)]
public static class TopKFrequent
{
    [Approach(Name = "Count, Then Sort by Count", Time = "O(n log n)", Space = "O(n)", Idea = """
        Count each number with a dictionary, sort the numbers by their count (highest first), and take the first `k`.
        """)]
    public static List<int> TopKBySorting(int[] numbers, int k)
    {
        var counts = new Dictionary<int, int>();
        foreach (int number in numbers)
        {
            counts[number] = counts.GetValueOrDefault(number) + 1;
        }
        return counts.OrderByDescending(pair => pair.Value)
                     .Take(k)
                     .Select(pair => pair.Key)
                     .ToList();
    }

    [Approach(Name = "Min-Heap of Size k", Time = "O(n log k)", Space = "O(n)", Idea = """
        Count first. Then push each number into a min-heap ordered by count, and whenever the heap holds more than `k`, remove the least frequent.
        The `k` survivors are the answer.
        """)]
    public static List<int> TopKWithHeap(int[] numbers, int k)
    {
        var counts = new Dictionary<int, int>();
        foreach (int number in numbers)
        {
            counts[number] = counts.GetValueOrDefault(number) + 1;
        }

        var heap = new PriorityQueue<int, int>();
        foreach (var pair in counts)
        {
            heap.Enqueue(pair.Key, pair.Value);
            if (heap.Count > k)
            {
                heap.Dequeue();
            }
        }

        var result = new List<int>();
        while (heap.Count > 0)
        {
            result.Add(heap.Dequeue());
        }
        result.Reverse();
        return result;
    }

    [Approach(Name = "Buckets by Count", Time = "O(n)", Space = "O(n)", Idea = """
        A count can only be between 1 and `n`. So make `n + 1` buckets, where `buckets[c]` holds the numbers that appear exactly `c` times.
        Then walk the buckets from the highest count down, collecting numbers until you have `k`. No sorting needed.
        """)]
    public static List<int> TopKWithBuckets(int[] numbers, int k)
    {
        var counts = new Dictionary<int, int>();
        foreach (int number in numbers)
        {
            counts[number] = counts.GetValueOrDefault(number) + 1;
        }

        var buckets = new List<int>[numbers.Length + 1];
        foreach (var pair in counts)
        {
            if (buckets[pair.Value] == null)
            {
                buckets[pair.Value] = new List<int>();
            }
            buckets[pair.Value].Add(pair.Key);
        }

        var result = new List<int>();
        for (int count = numbers.Length; count > 0 && result.Count < k; count--)
        {
            if (buckets[count] != null)
            {
                result.AddRange(buckets[count]);
            }
        }
        return result.Take(k).ToList();
    }

    public static Example[] Examples =>
    [
        new([new[] { 1, 1, 1, 2, 2, 3 }, 2], new[] { 1, 2 }, AnyOrder: true),
        new([new[] { 4, 4, 4, 4, 7, 7, 9 }, 1], new[] { 4 }),
    ];
}
