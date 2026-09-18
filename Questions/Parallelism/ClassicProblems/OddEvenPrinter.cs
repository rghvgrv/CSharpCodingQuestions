namespace CodingQuestions.Parallelism.ClassicProblems;

[Q(3_06_02, "Two Threads Print Odd & Even Alternately", Medium,
"One thread prints odd numbers, another prints even numbers. Together they must print 1, 2, 3 … 20 in order.")]
public static class OddEvenPrinter
{
    // Monitor.Wait releases the lock and sleeps until another thread calls Pulse/PulseAll.
    // Always wait in a `while` loop: re-check the condition after waking up.
    public static List<int> Print(int max)
    {
        var output = new List<int>();
        var gate = new object();
        int next = 1;

        void Worker(int parity)
        {
            lock (gate)
            {
                while (true)
                {
                    while (next <= max && next % 2 != parity) Monitor.Wait(gate); // not my turn
                    if (next > max) { Monitor.PulseAll(gate); return; }
                    output.Add(next);
                    Console.WriteLine($"  {(parity == 1 ? "odd " : "even")} thread {Environment.CurrentManagedThreadId}: {next}");
                    next++;
                    Monitor.PulseAll(gate); // wake the other thread
                }
            }
        }

        var odd = new Thread(() => Worker(1));
        var even = new Thread(() => Worker(0));
        even.Start(); odd.Start();
        odd.Join(); even.Join();
        return output;
    }

    public static void Run() => Check("Print(20)", Print(20), Enumerable.Range(1, 20).ToList());
}
