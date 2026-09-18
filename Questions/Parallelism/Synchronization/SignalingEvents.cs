namespace CodingQuestions.Parallelism.Synchronization;

[Q(3_02_06, "Signaling: ManualResetEventSlim vs AutoResetEvent", Medium,
"Use a ManualResetEventSlim as a starting gate that releases all waiting threads at once, and an AutoResetEvent as a turnstile that releases one thread per Set().")]
public static class SignalingEvents
{
    // ManualResetEvent = a door: Set() opens it for everyone until Reset().
    // AutoResetEvent   = a turnstile: each Set() lets exactly ONE waiter through, then closes automatically.
    public static void Run()
    {
        using var gate = new ManualResetEventSlim(false);
        int passed = 0;
        var runners = Enumerable.Range(1, 4).Select(n => new Thread(() =>
        {
            gate.Wait();
            Interlocked.Increment(ref passed);
        })).ToList();
        runners.ForEach(t => t.Start());
        Thread.Sleep(100);
        Check("Before Set(): runners through the gate", Volatile.Read(ref passed), 0);
        gate.Set();
        runners.ForEach(t => t.Join());
        Check("After one Set(): all runners through", passed, 4);

        using var turnstile = new AutoResetEvent(false);
        int through = 0;
        var people = Enumerable.Range(1, 3).Select(_ => new Thread(() =>
        {
            turnstile.WaitOne();
            Interlocked.Increment(ref through);
        })).ToList();
        people.ForEach(t => t.Start());
        Thread.Sleep(100);

        turnstile.Set();
        Thread.Sleep(100);
        Check("After 1st Set(): through the turnstile", Volatile.Read(ref through), 1);
        turnstile.Set(); turnstile.Set();
        people.ForEach(t => t.Join());
        Check("After 3 Set() calls", through, 3);
    }
}
