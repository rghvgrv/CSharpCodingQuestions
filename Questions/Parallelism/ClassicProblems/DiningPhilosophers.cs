namespace CodingQuestions.Parallelism.ClassicProblems;

[Q(3_06_05, "Dining Philosophers", Hard,
"Five philosophers sit at a round table with one fork between each pair. Each needs both neighboring forks to eat. Let each eat 3 times without deadlock. (LeetCode 1226)")]
public static class DiningPhilosophers
{
    // If everyone picks up their LEFT fork first, all hold one fork and wait forever (circular wait → deadlock).
    // Fix: resource ordering. Always pick up the LOWER-numbered fork first, which breaks the cycle.
    public static void Run()
    {
        const int n = 5, meals = 3;
        var forks = Enumerable.Range(0, n).Select(_ => new object()).ToArray();
        var eaten = new int[n];
        var log = new ConcurrentQueue<string>();

        var philosophers = Enumerable.Range(0, n).Select(p => new Thread(() =>
        {
            int left = p, right = (p + 1) % n;
            int first = Math.Min(left, right), second = Math.Max(left, right);
            for (int m = 0; m < meals; m++)
            {
                Thread.Sleep(Random.Shared.Next(1, 10)); // think
                lock (forks[first])
                    lock (forks[second])
                    {
                        eaten[p]++;
                        log.Enqueue($"P{p} eats (forks {first},{second})");
                        Thread.Sleep(2); // eat
                    }
            }
        })).ToList();

        philosophers.ForEach(t => t.Start());
        bool finished = philosophers.All(t => t.Join(5000));
        foreach (var line in log.Take(8)) Console.WriteLine("  " + line);
        Console.WriteLine("  …");
        Check("Finished without deadlock", finished, true);
        Check("Meals per philosopher", eaten, [3, 3, 3, 3, 3]);
    }
}
