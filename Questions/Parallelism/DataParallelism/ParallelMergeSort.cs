namespace CodingQuestions.Parallelism.DataParallelism;

[Q(3_04_05, "Parallel Merge Sort (Divide & Conquer)", Hard,
"Sort 1,000,000 integers with merge sort, sorting the two halves in parallel. Stop spawning parallel work below a size threshold.")]
public static class ParallelMergeSort
{
    // Fork/join: the two halves are independent → sort them at the same time (Parallel.Invoke), then merge.
    // Tiny tasks cost more than they save, so below the threshold we recurse sequentially.
    const int Threshold = 8_192;

    public static void Sort(int[] a, int[] tmp, int lo, int hi, bool parallel)
    {
        if (hi - lo < 1) return;
        int mid = lo + (hi - lo) / 2;
        if (parallel && hi - lo > Threshold)
            Parallel.Invoke(() => Sort(a, tmp, lo, mid, true), () => Sort(a, tmp, mid + 1, hi, true));
        else
        {
            Sort(a, tmp, lo, mid, false);
            Sort(a, tmp, mid + 1, hi, false);
        }
        Merge(a, tmp, lo, mid, hi);
    }

    static void Merge(int[] a, int[] tmp, int lo, int mid, int hi)
    {
        int i = lo, j = mid + 1, k = lo;
        while (i <= mid && j <= hi) tmp[k++] = a[i] <= a[j] ? a[i++] : a[j++];
        while (i <= mid) tmp[k++] = a[i++];
        while (j <= hi) tmp[k++] = a[j++];
        Array.Copy(tmp, lo, a, lo, hi - lo + 1);
    }

    public static void Run()
    {
        var rnd = new Random(1);
        var original = Enumerable.Range(0, 1_000_000).Select(_ => rnd.Next()).ToArray();
        var expected = original.Order().ToArray();

        var seq = (int[])original.Clone();
        var clock = Stopwatch.StartNew();
        Sort(seq, new int[seq.Length], 0, seq.Length - 1, parallel: false);
        long seqMs = clock.ElapsedMilliseconds;

        var par = (int[])original.Clone();
        clock.Restart();
        Sort(par, new int[par.Length], 0, par.Length - 1, parallel: true);
        long parMs = clock.ElapsedMilliseconds;

        Check("Sequential result sorted", seq.SequenceEqual(expected), true);
        Check("Parallel result sorted", par.SequenceEqual(expected), true);
        Console.WriteLine($"Sequential {seqMs} ms · Parallel {parMs} ms on {Environment.ProcessorCount} cores");
    }
}
