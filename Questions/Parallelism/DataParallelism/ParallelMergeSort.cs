namespace CSharpCodingQuestions.Questions.Parallelism.DataParallelism;

[Question(Order = 5, Title = "Parallel Merge Sort", Level = Hard, Problem = """
    Merge sort splits the array into two halves that can be sorted **independently**, which is natural parallelism.
    Sort 500,000 numbers faster by sorting the halves at the same time.
    """)]
public static class ParallelMergeSort
{
    private static void Merge(int[] numbers, int[] buffer, int low, int middle, int high)
    {
        int i = low;
        int j = middle + 1;
        int k = low;
        while (i <= middle && j <= high)
        {
            buffer[k++] = numbers[i] <= numbers[j] ? numbers[i++] : numbers[j++];
        }
        while (i <= middle)
        {
            buffer[k++] = numbers[i++];
        }
        while (j <= high)
        {
            buffer[k++] = numbers[j++];
        }
        Array.Copy(buffer, low, numbers, low, high - low + 1);
    }

    [Approach(Name = "Sequential", Idea = """
        Normal merge sort: sort the left half, then the right half, then merge.
        """)]
    public static void SortSequential(int[] numbers, int[] buffer, int low, int high)
    {
        if (low >= high)
        {
            return;
        }
        int middle = (low + high) / 2;
        SortSequential(numbers, buffer, low, middle);
        SortSequential(numbers, buffer, middle + 1, high);
        Merge(numbers, buffer, low, middle, high);
    }

    [Approach(Name = "Parallel at Every Level", Idea = """
        Sort both halves with `Parallel.Invoke` at **every** level of the recursion.
        That creates a parallel job for every tiny piece, even 2-item pieces, and the overhead of all those jobs eats the gain.
        """)]
    public static void SortParallelEverywhere(int[] numbers, int[] buffer, int low, int high)
    {
        if (low >= high)
        {
            return;
        }
        int middle = (low + high) / 2;
        Parallel.Invoke(
            () => SortParallelEverywhere(numbers, buffer, low, middle),
            () => SortParallelEverywhere(numbers, buffer, middle + 1, high));
        Merge(numbers, buffer, low, middle, high);
    }

    [Approach(Name = "Parallel Above a Size Threshold", Idea = """
        Split in parallel only while the piece is **big** (here over 10,000 items). Smaller pieces are sorted sequentially.
        A few big parallel jobs are enough to keep every core busy, without the cost of millions of tiny ones.
        This "stop splitting below a threshold" rule applies to any divide-and-conquer parallelism.
        """)]
    public static void SortParallelWithThreshold(int[] numbers, int[] buffer, int low, int high)
    {
        if (high - low < 10_000)
        {
            SortSequential(numbers, buffer, low, high);
            return;
        }
        int middle = (low + high) / 2;
        Parallel.Invoke(
            () => SortParallelWithThreshold(numbers, buffer, low, middle),
            () => SortParallelWithThreshold(numbers, buffer, middle + 1, high));
        Merge(numbers, buffer, low, middle, high);
    }

    public static void Demo()
    {
        int[] original = Enumerable.Range(0, 500_000).Select(_ => Random.Shared.Next()).ToArray();
        int[] expected = original.Order().ToArray();

        void Time(string name, Action<int[], int[], int, int> sort)
        {
            int[] numbers = (int[])original.Clone();
            var clock = Stopwatch.StartNew();
            sort(numbers, new int[numbers.Length], 0, numbers.Length - 1);
            Print($"{name} ({clock.ElapsedMilliseconds} ms) sorted correctly", numbers.SequenceEqual(expected), expected: true);
        }

        Time("Sequential", SortSequential);
        Time("Parallel at every level", SortParallelEverywhere);
        Time("Parallel above threshold", SortParallelWithThreshold);
    }
}
