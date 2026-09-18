namespace CSharpCodingQuestions.Questions.Parallelism.ClassicProblems;

[Question(Order = 2, Title = "Two Threads Take Turns: Odd and Even", Level = Medium, Problem = """
    One thread prints odd numbers and another prints even numbers. Together they must print `1, 2, 3, …, 10` in order,
    so they have to take turns.
    """)]
public static class OddEvenPrinter
{
    [Approach(Name = "Busy-Wait on a Shared Counter", Idea = """
        Each thread spins until the shared `next` number has its parity, prints it, then increases `next`.
        Correct, but both threads keep a CPU core busy just checking.
        """)]
    public static List<int> PrintBySpinning(int max)
    {
        var output = new List<int>();
        int next = 1;

        void Worker(int parity)
        {
            while (true)
            {
                int current = Volatile.Read(ref next);
                if (current > max)
                {
                    return;
                }
                if (current % 2 == parity)
                {
                    output.Add(current);
                    Volatile.Write(ref next, current + 1);
                }
            }
        }

        var odd = new Thread(() => Worker(1));
        var even = new Thread(() => Worker(0));
        odd.Start();
        even.Start();
        odd.Join();
        even.Join();
        return output;
    }

    [Approach(Name = "Pass a Turn With Two Semaphores", Idea = """
        Think of a baton passed back and forth:

        - `oddTurn` starts with 1 permit (odd goes first); `evenTurn` starts with 0.
        - The odd thread waits for `oddTurn`, prints, then releases `evenTurn`, handing the baton over.
        - The even thread does the opposite.

        The thread that isn't allowed to print just sleeps. No spinning.
        """)]
    public static List<int> PrintWithSemaphores(int max)
    {
        var output = new List<int>();
        using var oddTurn = new SemaphoreSlim(1);
        using var evenTurn = new SemaphoreSlim(0);

        var odd = new Thread(() =>
        {
            for (int number = 1; number <= max; number += 2)
            {
                oddTurn.Wait();
                output.Add(number);
                evenTurn.Release();
            }
        });
        var even = new Thread(() =>
        {
            for (int number = 2; number <= max; number += 2)
            {
                evenTurn.Wait();
                output.Add(number);
                oddTurn.Release();
            }
        });

        odd.Start();
        even.Start();
        odd.Join();
        even.Join();
        return output;
    }

    public static void Demo()
    {
        List<int> expected = Enumerable.Range(1, 10).ToList();
        Print("Busy-wait", PrintBySpinning(10), expected: expected);
        Print("Two semaphores", PrintWithSemaphores(10), expected: expected);
    }
}
