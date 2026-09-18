namespace CSharpCodingQuestions.Questions.Parallelism.DataParallelism;

[Question(Order = 6, Title = "Faster Matrix Multiplication", Level = Medium, Problem = """
    Multiply two 300 × 300 matrices: `c[i, j]` = sum over `k` of `a[i, k] × b[k, j]`. That's 27 million multiply-adds.
    Make it fast.
    """)]
public static class MatrixMultiply
{
    [Approach(Name = "Textbook Loop Order (i, j, k)", Idea = """
        The formula written directly. The inner loop walks **down a column** of `b`, jumping a whole row in memory at every step.
        The CPU's cache is built for reading memory in order, so these jumps make it slow.
        """)]
    public static double[,] MultiplyNaive(double[,] a, double[,] b)
    {
        int n = a.GetLength(0);
        var c = new double[n, n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                double sum = 0;
                for (int k = 0; k < n; k++)
                {
                    sum += a[i, k] * b[k, j];
                }
                c[i, j] = sum;
            }
        }
        return c;
    }

    [Approach(Name = "Cache-Friendly Order (i, k, j)", Idea = """
        Same math, with the two inner loops swapped. Now the innermost loop walks **along rows** of both `b` and `c`,
        reading memory in order. Same amount of work, often several times faster, and still on one core.
        """)]
    public static double[,] MultiplyCacheFriendly(double[,] a, double[,] b)
    {
        int n = a.GetLength(0);
        var c = new double[n, n];
        for (int i = 0; i < n; i++)
        {
            for (int k = 0; k < n; k++)
            {
                double aik = a[i, k];
                for (int j = 0; j < n; j++)
                {
                    c[i, j] += aik * b[k, j];
                }
            }
        }
        return c;
    }

    [Approach(Name = "Cache-Friendly + Parallel Rows", Idea = """
        Each row of the result depends only on one row of `a` and all of `b`, so rows are **independent**.
        Compute the rows in parallel with `Parallel.For`. Each thread writes different rows, so no locking is needed.
        """)]
    public static double[,] MultiplyParallel(double[,] a, double[,] b)
    {
        int n = a.GetLength(0);
        var c = new double[n, n];
        Parallel.For(0, n, i =>
        {
            for (int k = 0; k < n; k++)
            {
                double aik = a[i, k];
                for (int j = 0; j < n; j++)
                {
                    c[i, j] += aik * b[k, j];
                }
            }
        });
        return c;
    }

    public static void Demo()
    {
        const int size = 300;
        var a = new double[size, size];
        var b = new double[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                a[i, j] = (i + j) % 10;
                b[i, j] = (i * j) % 10;
            }
        }

        var clock = Stopwatch.StartNew();
        double[,] naive = MultiplyNaive(a, b);
        Print("Textbook order", $"{clock.ElapsedMilliseconds} ms");

        clock.Restart();
        double[,] friendly = MultiplyCacheFriendly(a, b);
        Print("Cache-friendly order", $"{clock.ElapsedMilliseconds} ms");

        clock.Restart();
        double[,] parallel = MultiplyParallel(a, b);
        Print("Cache-friendly + parallel", $"{clock.ElapsedMilliseconds} ms");

        bool same = naive.Cast<double>().SequenceEqual(friendly.Cast<double>()) && naive.Cast<double>().SequenceEqual(parallel.Cast<double>());
        Print("All three give the same matrix", same, expected: true);
    }
}
