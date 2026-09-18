namespace CodingQuestions.Parallelism.Synchronization;

[Q(3_02_07, "CountdownEvent & Barrier", Medium,
"(1) Wait until 5 workers have each signaled completion. (2) Run 3 workers through 3 phases where nobody starts phase N+1 until all finished phase N.")]
public static class CountdownAndBarrier
{
    public static void Run()
    {
        // CountdownEvent: one side waits until the count drops to zero.
        using var remaining = new CountdownEvent(5);
        for (int i = 1; i <= 5; i++)
        {
            int n = i;
            new Thread(() => { Thread.Sleep(n * 10); Console.WriteLine($"  worker {n} done"); remaining.Signal(); }).Start();
        }
        remaining.Wait();
        Check("Main continued after all 5 signaled", remaining.CurrentCount, 0);

        // Barrier: every participant waits for the others at the end of each phase.
        var log = new ConcurrentQueue<long>();
        using var barrier = new Barrier(3, b => Console.WriteLine($"  ── phase {b.CurrentPhaseNumber} complete ──"));
        var workers = Enumerable.Range(1, 3).Select(w => new Thread(() =>
        {
            for (int phase = 0; phase < 3; phase++)
            {
                Thread.Sleep(Random.Shared.Next(5, 30)); // different speeds
                log.Enqueue(barrier.CurrentPhaseNumber);
                Console.WriteLine($"  worker {w} finished phase {phase}");
                barrier.SignalAndWait();
            }
        })).ToList();
        workers.ForEach(t => t.Start());
        workers.ForEach(t => t.Join());
        Check("Phase log never goes back (all finish phase N before N+1)", log.ToArray(), [0L, 0, 0, 1, 1, 1, 2, 2, 2]);
    }
}
