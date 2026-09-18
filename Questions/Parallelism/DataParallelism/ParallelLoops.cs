namespace CodingQuestions.Parallelism.DataParallelism;

[Q(3_04_01, "Parallel.For, Parallel.ForEach & Parallel.Invoke", Easy,
"Square 1..20 in parallel and see which threads did the work. Run independent jobs with Parallel.Invoke. Stop a parallel search early with ParallelLoopState.")]
public static class ParallelLoops
{
    // Parallel.* splits CPU-bound work across pool threads and blocks until everything is done.
    // Each index goes to exactly one thread, so writing to results[i] needs no lock.
    public static void Run()
    {
        var results = new int[20];
        var threadsUsed = new ConcurrentDictionary<int, byte>();
        Parallel.For(0, 20, i =>
        {
            results[i] = (i + 1) * (i + 1);
            threadsUsed.TryAdd(Environment.CurrentManagedThreadId, 0);
        });
        Check("Parallel.For squares", results, Enumerable.Range(1, 20).Select(x => x * x).ToArray());
        Console.WriteLine($"  work was spread over {threadsUsed.Count} thread(s) on {Environment.ProcessorCount} cores");

        var words = new[] { "alpha", "beta", "gamma", "delta" };
        var upper = new ConcurrentBag<string>();
        Parallel.ForEach(words, w => upper.Add(w.ToUpper()));
        Check("Parallel.ForEach (bag has no order, so sort)", upper.Order().ToArray(), ["ALPHA", "BETA", "DELTA", "GAMMA"]);

        // Parallel.Invoke: run a few different actions at once, return when all are done.
        long a = 0, b = 0, c = 0;
        Parallel.Invoke(
            () => a = Enumerable.Range(1, 1000).Sum(x => (long)x),
            () => b = Enumerable.Range(1, 1000).Where(x => x % 2 == 0).Count(),
            () => c = Enumerable.Range(1, 20).Aggregate(1L, (p, x) => p * x));
        Check("Parallel.Invoke (sum, evens, 20!)", (a, b, c), (500500L, 500L, 2432902008176640000L));

        // Stop(): end the loop ASAP once any thread finds an answer (iterations already running still finish).
        var data = Enumerable.Range(0, 1_000_000).ToArray();
        int found = -1;
        var loop = Parallel.For(0, data.Length, (i, state) =>
        {
            if (data[i] == 777_777) { found = i; state.Stop(); }
        });
        Check("Found with state.Stop()", found, 777_777);
        Check("Loop completed normally?", loop.IsCompleted, false);
    }
}
