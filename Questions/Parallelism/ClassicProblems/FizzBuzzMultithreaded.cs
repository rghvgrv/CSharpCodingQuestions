namespace CodingQuestions.Parallelism.ClassicProblems;

[Q(3_06_03, "FizzBuzz Multithreaded", Medium,
"Four threads share one counter: one prints \"fizz\", one \"buzz\", one \"fizzbuzz\", one the numbers. Together they must produce the normal FizzBuzz sequence. (LeetCode 1195)")]
public static class FizzBuzzMultithreaded
{
    // Each thread owns a condition on i. It waits until the shared counter matches its condition,
    // prints, advances the counter, and wakes everyone.
    public static List<string> Solve(int n)
    {
        var output = new List<string>();
        var gate = new object();
        int i = 1;

        Thread Worker(Func<int, bool> mine, Func<int, string> say) => new(() =>
        {
            lock (gate)
            {
                while (true)
                {
                    while (i <= n && !mine(i)) Monitor.Wait(gate);
                    if (i > n) { Monitor.PulseAll(gate); return; }
                    output.Add(say(i));
                    i++;
                    Monitor.PulseAll(gate);
                }
            }
        });

        var threads = new[]
        {
            Worker(x => x % 15 == 0, _ => "fizzbuzz"),
            Worker(x => x % 3 == 0 && x % 5 != 0, _ => "fizz"),
            Worker(x => x % 5 == 0 && x % 3 != 0, _ => "buzz"),
            Worker(x => x % 3 != 0 && x % 5 != 0, x => x.ToString()),
        };
        foreach (var t in threads) t.Start();
        foreach (var t in threads) t.Join();
        return output;
    }

    public static void Run() =>
        Check("Solve(15)", Solve(15), ["1", "2", "fizz", "4", "buzz", "fizz", "7", "8", "fizz", "buzz", "11", "fizz", "13", "14", "fizzbuzz"]);
}
