namespace CodingQuestions.Parallelism.DataParallelism;

[Q(3_04_06, "Parallel Matrix Multiplication", Medium,
"Multiply two 300×300 matrices sequentially and with Parallel.For over rows. Verify both give the same result.")]
public static class ParallelMatrixMultiply
{
    // Each output row depends only on one row of A and all of B, so rows are independent: parallelize the outer loop.
    // Loop order i-k-j walks memory row by row (cache friendly) instead of jumping down columns.
    static void MultiplyRow(double[,] a, double[,] b, double[,] c, int i)
    {
        int n = b.GetLength(0), m = b.GetLength(1);
        for (int k = 0; k < n; k++)
        {
            double aik = a[i, k];
            for (int j = 0; j < m; j++) c[i, j] += aik * b[k, j];
        }
    }

    public static void Run()
    {
        const int size = 300;
        var rnd = new Random(3);
        var a = new double[size, size];
        var b = new double[size, size];
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++) { a[i, j] = rnd.Next(10); b[i, j] = rnd.Next(10); }

        var seq = new double[size, size];
        var clock = Stopwatch.StartNew();
        for (int i = 0; i < size; i++) MultiplyRow(a, b, seq, i);
        long seqMs = clock.ElapsedMilliseconds;

        var par = new double[size, size];
        clock.Restart();
        Parallel.For(0, size, i => MultiplyRow(a, b, par, i));
        long parMs = clock.ElapsedMilliseconds;

        Check("Parallel result equals sequential", seq.Cast<double>().SequenceEqual(par.Cast<double>()), true);
        Check("c[0,0] = row 0 of A · column 0 of B", par[0, 0], Enumerable.Range(0, size).Sum(k => a[0, k] * b[k, 0]));
        Console.WriteLine($"Sequential {seqMs} ms · Parallel {parMs} ms ({size}³ = {(long)size * size * size:N0} multiply-adds)");
    }
}
