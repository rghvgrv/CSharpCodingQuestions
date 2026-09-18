# Dynamic Programming

Dynamic Programming (DP) speeds up recursion by remembering answers to smaller problems, so each one is solved only once. It often turns exponential time into polynomial time.

## Why it's needed

Plain recursive Fibonacci recomputes the same values again and again: `Fib(5)` calls `Fib(3)` twice, `Fib(2)` three times, and so on. The work doubles with every step: `O(2ⁿ)`.

If we **store** each answer the first time we compute it, every `Fib(i)` is computed once: `O(n)`.

## Two styles

**Top-down (memoization)**: write the recursion, and add a cache.

```csharp
long Fib(int n, Dictionary<int, long> memo)
{
    if (n < 2) return n;
    if (memo.TryGetValue(n, out long saved)) return saved;
    long result = Fib(n - 1, memo) + Fib(n - 2, memo);
    memo[n] = result;
    return result;
}
```

**Bottom-up (tabulation)**: fill a table from the smallest problems up, with no recursion.

```csharp
long[] table = new long[n + 1];
table[1] = 1;
for (int i = 2; i <= n; i++)
{
    table[i] = table[i - 1] + table[i - 2];
}
```

## A recipe for DP problems

1. **State**: what does `dp[i]` (or `dp[i, j]`) mean, in words?
2. **Choice / recurrence**: how is `dp[i]` built from smaller states?
3. **Base cases**: the smallest answers you know directly.
4. **Order**: fill the table so that smaller states are ready first.
5. **Answer**: which cell holds the final answer?

## How to spot a DP problem

- "Count the number of ways…", "minimum / maximum …", "is it possible to …"
- The brute force tries all choices, and the **same sub-problems repeat**.

Every question in this topic shows the progression: **plain recursion → memoization → table**, and sometimes a final version that keeps only the last row or two variables.
