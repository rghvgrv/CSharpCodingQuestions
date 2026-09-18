namespace CSharpCodingQuestions.Questions.Parallelism.Synchronization;

[Question(Order = 8, Title = "Work in Phases (Barrier)", Level = Medium, Problem = """
    3 workers do a job in 3 phases (for example: load, process, save). Nobody may start phase 2 until **everyone** has finished phase 1, and so on.
    """)]
public static class PhasesWithBarrier
{
    [Approach(Name = "Barrier", Idea = """
        A `Barrier` for 3 participants: each worker calls `SignalAndWait()` at the end of a phase and sleeps there
        until all 3 have arrived. Then all continue together into the next phase.

        The optional action passed to the constructor runs once per phase, right when the last worker arrives.
        """)]
    public static List<string> RunPhases(int workerCount, int phaseCount)
    {
        var log = new ConcurrentQueue<string>();
        using var barrier = new Barrier(workerCount, b => log.Enqueue($"--- phase {b.CurrentPhaseNumber} complete ---"));

        var workers = Enumerable.Range(1, workerCount).Select(worker => new Thread(() =>
        {
            for (int phase = 0; phase < phaseCount; phase++)
            {
                Thread.Sleep(Random.Shared.Next(5, 30));   // workers run at different speeds
                log.Enqueue($"worker {worker} finished phase {phase}");
                barrier.SignalAndWait();
            }
        })).ToList();

        workers.ForEach(thread => thread.Start());
        workers.ForEach(thread => thread.Join());
        return log.ToList();
    }

    public static void Demo()
    {
        List<string> log = RunPhases(workerCount: 3, phaseCount: 3);
        foreach (string line in log)
        {
            Console.WriteLine(line);
        }

        // Each "phase complete" line must come after exactly 3 worker lines of that phase.
        var markers = log.Select((line, index) => (line, index)).Where(item => item.line.StartsWith("---")).Select(item => item.index);
        Print("A phase closed only after all 3 workers finished it", markers.SequenceEqual(new[] { 3, 7, 11 }), expected: true);
    }
}
