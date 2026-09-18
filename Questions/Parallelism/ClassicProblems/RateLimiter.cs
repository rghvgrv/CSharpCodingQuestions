namespace CSharpCodingQuestions.Questions.Parallelism.ClassicProblems;

[Question(Order = 9, Title = "Rate Limiter", Level = Hard, Problem = """
    Allow at most **5 requests per second** per user, even when requests arrive from many threads at once.
    (The limiters take the current time as a parameter so the demo is repeatable. Real code would use a clock.)
    """)]
public static class RateLimiter
{
    [Approach(Name = "Fixed Window Counter", Idea = """
        Count requests per calendar second (window). A new second resets the count to 0.
        Simple, but there's a burst problem: 5 requests at 0.99 s and 5 more at 1.01 s all pass, so 10 requests get through in 20 ms.
        """)]
    public class FixedWindowLimiter(int limit)
    {
        private readonly object gate = new object();
        private long currentWindow = -1;
        private int countInWindow;

        public bool TryAcquire(double nowSeconds)
        {
            lock (gate)
            {
                long window = (long)Math.Floor(nowSeconds);
                if (window != currentWindow)
                {
                    currentWindow = window;
                    countInWindow = 0;
                }
                if (countInWindow >= limit)
                {
                    return false;
                }
                countInWindow++;
                return true;
            }
        }
    }

    [Approach(Name = "Token Bucket", Idea = """
        A bucket holds up to 5 **tokens** and refills smoothly at 5 tokens per second. Each request spends one token; with no token, it's rejected.

        - Short bursts up to the bucket size are fine.
        - There's no window edge to exploit, because tokens come back gradually.

        The refill and the spend happen inside one `lock`, so two threads can never spend the same token.
        (.NET has a ready-made `TokenBucketRateLimiter` in `System.Threading.RateLimiting`.)
        """)]
    public class TokenBucketLimiter(int capacity, double tokensPerSecond)
    {
        private readonly object gate = new object();
        private double tokens = capacity;
        private double lastRefill;

        public bool TryAcquire(double nowSeconds)
        {
            lock (gate)
            {
                tokens = Math.Min(capacity, tokens + (nowSeconds - lastRefill) * tokensPerSecond);
                lastRefill = nowSeconds;
                if (tokens < 1)
                {
                    return false;
                }
                tokens--;
                return true;
            }
        }
    }

    public static void Demo()
    {
        var fixedWindow = new FixedWindowLimiter(limit: 5);
        var bucket = new TokenBucketLimiter(capacity: 5, tokensPerSecond: 5);

        int fixedAllowed = 0;
        int bucketAllowed = 0;
        foreach (double time in new[] { 0.99, 0.99, 0.99, 0.99, 0.99, 1.01, 1.01, 1.01, 1.01, 1.01 })
        {
            fixedAllowed += fixedWindow.TryAcquire(time) ? 1 : 0;
            bucketAllowed += bucket.TryAcquire(time) ? 1 : 0;
        }
        Console.WriteLine("10 requests between 0.99 s and 1.01 s:");
        Print("  Fixed window allowed", fixedAllowed, expected: 10);
        Print("  Token bucket allowed", bucketAllowed, expected: 5);

        var shared = new TokenBucketLimiter(capacity: 5, tokensPerSecond: 5);
        int allowed = 0;
        Parallel.For(0, 20, _ =>
        {
            if (shared.TryAcquire(nowSeconds: 0))
            {
                Interlocked.Increment(ref allowed);
            }
        });
        Print("20 threads at the same instant, token bucket allowed", allowed, expected: 5);
        Print("One second later, a new request is allowed", shared.TryAcquire(nowSeconds: 1), expected: true);
    }
}
