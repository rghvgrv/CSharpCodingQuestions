namespace CSharpCodingQuestions.Questions.Dsa.Heaps;

[Question(Order = 2, Title = "K-th Largest Element", Level = Medium, Problem = """
    Return the `k`-th largest number (counting duplicates). `numbers = [3, 2, 1, 5, 6, 4]`, `k = 2` → `5`.
    """)]
public static class KthLargestElement
{
    [Approach(Name = "Sort", Time = "O(n log n)", Space = "O(n)", Idea = """
        Sort a copy; the `k`-th largest is `k` places from the end.
        """)]
    public static int KthLargestBySorting(int[] numbers, int k)
    {
        int[] sorted = (int[])numbers.Clone();
        Array.Sort(sorted);
        return sorted[sorted.Length - k];
    }

    [Approach(Name = "Min-Heap of Size k", Time = "O(n log k)", Space = "O(k)", Idea = """
        Keep only the `k` largest numbers seen so far in a **min-heap**. When it grows past `k`, remove its smallest.
        At the end, the heap holds the top `k`, and its smallest (the top) is the `k`-th largest.
        Great when `k` is small or the numbers arrive as a stream.
        """)]
    public static int KthLargestWithHeap(int[] numbers, int k)
    {
        var heap = new PriorityQueue<int, int>();
        foreach (int number in numbers)
        {
            heap.Enqueue(number, number);
            if (heap.Count > k)
            {
                heap.Dequeue();
            }
        }
        return heap.Peek();
    }

    [Approach(Name = "Quickselect", Time = "O(n) on average", Space = "O(1)", Idea = """
        Use quick sort's **partition** step, but recurse into only one side.
        After partitioning around a random pivot, the pivot sits at its final sorted position.
        If that position is the one we want (`n - k`), we're done. Otherwise continue only in the half that contains it.
        On average the work is `n + n/2 + n/4 + … ≈ 2n`.
        """)]
    public static int KthLargestQuickselect(int[] numbers, int k)
    {
        int[] items = (int[])numbers.Clone();
        int target = items.Length - k;
        int low = 0;
        int high = items.Length - 1;

        while (true)
        {
            int randomIndex = Random.Shared.Next(low, high + 1);
            (items[randomIndex], items[high]) = (items[high], items[randomIndex]);

            int pivot = items[high];
            int smallerCount = low;
            for (int i = low; i < high; i++)
            {
                if (items[i] < pivot)
                {
                    (items[i], items[smallerCount]) = (items[smallerCount], items[i]);
                    smallerCount++;
                }
            }
            (items[smallerCount], items[high]) = (items[high], items[smallerCount]);

            if (smallerCount == target)
            {
                return items[smallerCount];
            }
            if (smallerCount < target)
            {
                low = smallerCount + 1;
            }
            else
            {
                high = smallerCount - 1;
            }
        }
    }

    public static Example[] Examples =>
    [
        new([new[] { 3, 2, 1, 5, 6, 4 }, 2], 5),
        new([new[] { 3, 2, 3, 1, 2, 4, 5, 5, 6 }, 4], 4),
        new([new[] { 7 }, 1], 7),
    ];
}
