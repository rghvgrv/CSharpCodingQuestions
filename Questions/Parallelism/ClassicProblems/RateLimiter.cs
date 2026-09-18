namespace CodingQuestions.Parallelism.ClassicProblems;

[Q(3_06_09, "Thread-Safe Rate Limiter (Token Bucket)", Hard,
"Allow bursts of up to 5 requests, refilling 10 tokens per second. 20 threads fire requests at the same instant: exactly 5 may pass. After a pause, more are allowed.")]
public static class RateLimiter
{
    // Token bucket: tokens refill continuously up to a cap; each request spends one.
    // The refill and the spend must happen together under one lock, or two threads could spend the same token.
    public class TokenBucket(int capacity, double refillPerSecond)
    {
        readonly Lock gate = new();
        readonly Stopwatch clock = Stopwatch.StartNew();
        double tokens = capacity;
        double lastRefill;

        public bool TryAcquire()
        {
            lock (gate)
            {
                double now = clock.Elapsed.TotalSeconds;
                tokens = Math.Min(capacity, tokens + (now - lastRefill) * refillPerSecond);
                lastRefill = now;
                if (tokens < 1) return false;
                tokens--;
                return true;
            }
        }
    }

    public static void Run()
    {
        var bucket = new TokenBucket(capacity: 5, refillPerSecond: 10);

        int allowed = 0;
        using var go = new ManualResetEventSlim();
        var threads = Enumerable.Range(0, 20).Select(_ => new Thread(() =>
        {
            go.Wait();
            if (bucket.TryAcquire()) Interlocked.Increment(ref allowed);
        })).ToList();
        threads.ForEach(t => t.Start());
        go.Set();
        threads.ForEach(t => t.Join());
        Check("Burst of 20 at once → allowed", allowed, 5);

        Thread.Sleep(300); // ~3 tokens refill
        int later = Enumerable.Range(0, 10).Count(_ => bucket.TryAcquire());
        Console.WriteLine($"  after 300 ms, {later} more allowed (≈ 0.3 s × 10/s)");
        Check("Some refill happened, capped at 5", later is >= 2 and <= 5, true);

        // .NET also ships this: System.Threading.RateLimiting (TokenBucketRateLimiter), used by ASP.NET Core's rate limiting middleware.
    }
}
