# Data Parallelism

Data parallelism means splitting a big job into chunks and processing the chunks on all CPU cores at once. In .NET that's Parallel.For, Parallel.ForEach and PLINQ.

## The tools

```csharp
// Parallel loop: iterations are spread over the cores
Parallel.For(0, 1000, i =>
{
    results[i] = Compute(i);   // each i writes its own slot, so no lock is needed
});

// PLINQ: add .AsParallel() to a LINQ query
int primeCount = numbers.AsParallel().Count(IsPrime);
```

## When it helps

- The work is **CPU-bound**: calculations, not waiting.
- Each item is **independent**, or can be combined at the end, like a sum.
- There's **enough** work. For tiny jobs, splitting and coordinating costs more than it saves.

## Speedup and Amdahl's law

**Speedup** = time with 1 core ÷ time with many cores. With 8 cores the best case is 8×, and real code gets less.

**Amdahl's law**: if only 90% of the work can run in parallel, the other 10% limits you. Even with infinite cores the speedup can't exceed 10×.

## Avoid shared state

The fastest parallel code gives each thread its **own** data, such as a private subtotal, and combines the pieces once at the end. A lock inside the loop makes all threads wait in line, which can be slower than no parallelism at all.

## Divide and conquer

Recursive algorithms like merge sort split into independent halves, which is natural parallelism. Stop splitting below a size threshold, or the overhead wins.
