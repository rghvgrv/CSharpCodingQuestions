namespace CSharpCodingQuestions.Questions.Parallelism.ClassicProblems;

[Question(Order = 5, Title = "Dining Philosophers", Level = Hard, Problem = """
    Five philosophers sit around a table with one fork between each pair. To eat, a philosopher needs **both** neighboring forks.
    Let each of them eat 3 times, without anyone getting stuck forever. (LeetCode 1226)
    """)]
public static class DiningPhilosophers
{
    [Approach(Name = "Everyone Takes the Left Fork First (Deadlock)", Idea = """
        If all five pick up their left fork at the same moment, every fork is taken and everyone waits for their right fork forever.
        That's a **circular wait**, the classic deadlock.

        This demo uses `Monitor.TryEnter` with a timeout, so it can **detect** the problem instead of freezing.
        """)]
    public static bool EatLeftFirst(int philosopherCount)
    {
        object[] forks = Enumerable.Range(0, philosopherCount).Select(_ => new object()).ToArray();
        using var allHoldLeftFork = new Barrier(philosopherCount);
        using var allTried = new Barrier(philosopherCount);
        int stuck = 0;

        var philosophers = Enumerable.Range(0, philosopherCount).Select(p => new Thread(() =>
        {
            object left = forks[p];
            object right = forks[(p + 1) % philosopherCount];
            lock (left)
            {
                allHoldLeftFork.SignalAndWait();       // everyone now holds their left fork
                if (Monitor.TryEnter(right, 300))
                {
                    Monitor.Exit(right);
                }
                else
                {
                    Interlocked.Increment(ref stuck);
                }
                allTried.SignalAndWait();              // keep holding the left fork until everyone has tried
            }
        })).ToList();

        philosophers.ForEach(thread => thread.Start());
        philosophers.ForEach(thread => thread.Join());
        return stuck == philosopherCount;   // true = everyone was stuck: deadlock
    }

    [Approach(Name = "Always Take the Lower-Numbered Fork First", Idea = """
        Number the forks. Each philosopher picks up the **lower-numbered** of their two forks first.
        For four of them that's their left fork, but the last philosopher reaches for fork 0 (their right fork) first.
        The circle is broken, so at least one philosopher can always get both forks, eat, and put them down.

        Consistent resource ordering is the standard cure for deadlocks.
        """)]
    public static int[] EatWithOrderedForks(int philosopherCount, int meals)
    {
        object[] forks = Enumerable.Range(0, philosopherCount).Select(_ => new object()).ToArray();
        int[] eaten = new int[philosopherCount];

        var philosophers = Enumerable.Range(0, philosopherCount).Select(p => new Thread(() =>
        {
            int left = p;
            int right = (p + 1) % philosopherCount;
            int first = Math.Min(left, right);
            int second = Math.Max(left, right);

            for (int meal = 0; meal < meals; meal++)
            {
                Thread.Sleep(Random.Shared.Next(1, 5));   // think
                lock (forks[first])
                {
                    lock (forks[second])
                    {
                        eaten[p]++;                         // eat
                    }
                }
            }
        })).ToList();

        philosophers.ForEach(thread => thread.Start());
        bool finished = philosophers.All(thread => thread.Join(5000));
        return finished ? eaten : Array.Empty<int>();
    }

    public static void Demo()
    {
        Print("Left fork first: everyone got stuck", EatLeftFirst(5), expected: true);
        Print("Ordered forks: meals eaten by each philosopher", EatWithOrderedForks(5, 3), expected: new[] { 3, 3, 3, 3, 3 });
    }
}
