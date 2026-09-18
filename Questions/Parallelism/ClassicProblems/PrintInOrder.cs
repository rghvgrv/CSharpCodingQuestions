namespace CSharpCodingQuestions.Questions.Parallelism.ClassicProblems;

[Question(Order = 1, Title = "Print in Order", Level = Easy, Problem = """
    Three threads call `First()`, `Second()` and `Third()`. They may **start in any order**,
    but the output must always be `first second third`. (LeetCode 1114)
    """)]
public static class PrintInOrder
{
    [Approach(Name = "Busy-Wait on Flags", Idea = """
        `Second` spins in a loop until a `firstDone` flag becomes true, and `Third` waits for `secondDone`.
        It works, but a spinning thread burns a whole CPU core doing nothing. `volatile` makes each thread see the other's update.
        """)]
    public class SpinningPrinter
    {
        private volatile bool firstDone;
        private volatile bool secondDone;

        public void First(Action print)
        {
            print();
            firstDone = true;
        }

        public void Second(Action print)
        {
            while (!firstDone)
            {
            }
            print();
            secondDone = true;
        }

        public void Third(Action print)
        {
            while (!secondDone)
            {
            }
            print();
        }
    }

    [Approach(Name = "Semaphores as Signals", Idea = """
        A `SemaphoreSlim` that starts with **0** permits works as a one-time signal: `Wait()` sleeps until someone calls `Release()`.

        - `Second` waits for `firstDone`, which `First` releases when it's finished.
        - `Third` waits for `secondDone`, which `Second` releases.

        Waiting threads sleep instead of spinning.
        """)]
    public class SignalingPrinter
    {
        private readonly SemaphoreSlim firstDone = new(0);
        private readonly SemaphoreSlim secondDone = new(0);

        public void First(Action print)
        {
            print();
            firstDone.Release();
        }

        public void Second(Action print)
        {
            firstDone.Wait();
            print();
            secondDone.Release();
        }

        public void Third(Action print)
        {
            secondDone.Wait();
            print();
        }
    }

    public static void Demo()
    {
        for (int round = 1; round <= 3; round++)
        {
            var printer = new SignalingPrinter();
            var output = new ConcurrentQueue<string>();
            var threads = new List<Thread>
            {
                new(() => printer.Third(() => output.Enqueue("third"))),
                new(() => printer.First(() => output.Enqueue("first"))),
                new(() => printer.Second(() => output.Enqueue("second"))),
            };
            foreach (Thread thread in threads.OrderBy(_ => Random.Shared.Next()))
            {
                thread.Start();   // random start order
            }
            threads.ForEach(thread => thread.Join());
            Print($"Round {round}", string.Join(" ", output), expected: "first second third");
        }

        var spinning = new SpinningPrinter();
        var spinOutput = new ConcurrentQueue<string>();
        var spinThreads = new List<Thread>
        {
            new(() => spinning.Second(() => spinOutput.Enqueue("second"))),
            new(() => spinning.First(() => spinOutput.Enqueue("first"))),
        };
        spinThreads.ForEach(thread => thread.Start());
        spinThreads.ForEach(thread => thread.Join());
        Print("Busy-wait version also orders correctly", string.Join(" ", spinOutput), expected: "first second");
    }
}
