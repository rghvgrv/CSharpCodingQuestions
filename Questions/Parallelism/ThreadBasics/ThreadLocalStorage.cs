namespace CodingQuestions.Parallelism.ThreadBasics;

[Q(3_01_04, "Thread-Local State: [ThreadStatic], ThreadLocal<T>, AsyncLocal<T>", Medium,
"Give each thread its own copy of a variable. Show [ThreadStatic], ThreadLocal<T> (with per-thread subtotals), and AsyncLocal<T>, which follows async code across threads.")]
public static class ThreadLocalStorage
{
    [ThreadStatic] static int perThreadCounter; // one copy per thread, starts at 0 on each
    static readonly AsyncLocal<string?> RequestId = new();

    public static void Run()
    {
        // 1) [ThreadStatic]: each thread increments its own copy
        var threads = Enumerable.Range(1, 3).Select(n => new Thread(() =>
        {
            for (int i = 0; i < n * 10; i++) perThreadCounter++;
            Console.WriteLine($"  thread {n} sees its own counter = {perThreadCounter}");
        })).ToList();
        threads.ForEach(t => t.Start());
        threads.ForEach(t => t.Join());
        Check("Caller's copy untouched", perThreadCounter, 0);

        // 2) ThreadLocal<T>: no locking needed inside the loop; combine at the end.
        using var subtotal = new ThreadLocal<long>(() => 0, trackAllValues: true);
        Parallel.For(1, 100_001, i => subtotal.Value += i);
        Console.WriteLine($"  {subtotal.Values.Count} threads kept private subtotals");
        Check("Sum of subtotals = sum 1..100000", subtotal.Values.Sum(), 5_000_050_000L);

        // 3) AsyncLocal<T> flows with the logical call (across await and Task.Run), unlike [ThreadStatic].
        //    This app captures each question's Console output this way.
        RequestId.Value = "req-42";
        perThreadCounter = 7;
        var (flowed, staticSeen) = Task.Run(async () =>
        {
            await Task.Delay(10); // likely resumes on another pool thread
            return (RequestId.Value, perThreadCounter);
        }).Result;
        Check("AsyncLocal after await", flowed, "req-42");
        Check("[ThreadStatic] on the other thread", staticSeen == 7, false);
        perThreadCounter = 0;
    }
}
