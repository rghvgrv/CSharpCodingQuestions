namespace CSharpCodingQuestions.Questions.Parallelism.ClassicProblems;

[Question(Order = 3, Title = "FizzBuzz With Four Threads", Level = Medium, Problem = """
    Four threads share one counter from 1 to `n`: one prints `fizz`, one `buzz`, one `fizzbuzz`, and one prints the plain numbers.
    Together they must produce the normal FizzBuzz sequence. (LeetCode 1195)
    """)]
public static class FizzBuzzThreads
{
    [Approach(Name = "Monitor.Wait and PulseAll", Idea = """
        Each thread owns a rule, like "multiples of 3 but not 5". Inside one shared `lock`:

        1. `while` the current number isn't mine, `Monitor.Wait(gate)`. This releases the lock and sleeps until woken.
        2. When it is mine: print it, move the counter forward, and `Monitor.PulseAll(gate)` to wake everyone to re-check.

        Always wait in a `while` loop, not an `if`: after waking up, the number may still belong to someone else.
        """)]
    public static List<string> FizzBuzz(int n)
    {
        var output = new List<string>();
        object gate = new object();
        int current = 1;

        Thread MakeWorker(Func<int, bool> isMine, Func<int, string> say)
        {
            return new Thread(() =>
            {
                lock (gate)
                {
                    while (true)
                    {
                        while (current <= n && !isMine(current))
                        {
                            Monitor.Wait(gate);
                        }
                        if (current > n)
                        {
                            Monitor.PulseAll(gate);
                            return;
                        }
                        output.Add(say(current));
                        current++;
                        Monitor.PulseAll(gate);
                    }
                }
            });
        }

        var workers = new[]
        {
            MakeWorker(i => i % 15 == 0, _ => "fizzbuzz"),
            MakeWorker(i => i % 3 == 0 && i % 5 != 0, _ => "fizz"),
            MakeWorker(i => i % 5 == 0 && i % 3 != 0, _ => "buzz"),
            MakeWorker(i => i % 3 != 0 && i % 5 != 0, i => i.ToString()),
        };
        foreach (Thread worker in workers)
        {
            worker.Start();
        }
        foreach (Thread worker in workers)
        {
            worker.Join();
        }
        return output;
    }

    public static void Demo()
    {
        Print("n = 15", FizzBuzz(15),
            expected: new[] { "1", "2", "fizz", "4", "buzz", "fizz", "7", "8", "fizz", "buzz", "11", "fizz", "13", "14", "fizzbuzz" });
    }
}
