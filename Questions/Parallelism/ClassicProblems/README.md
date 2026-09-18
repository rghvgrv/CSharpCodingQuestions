# Classic Concurrency Problems

Famous puzzles about coordinating threads, several of them from LeetCode's concurrency section. Each one practices a key skill: ordering threads, taking turns, avoiding deadlock, and sharing work safely.

## The skills

- **Ordering**: thread B must wait until thread A has done something. Tools: `SemaphoreSlim`, `ManualResetEventSlim`.
- **Taking turns**: threads alternate. Tools: `Monitor.Wait` / `Monitor.PulseAll`, or a pair of semaphores.
- **Grouping**: act only when the right set of threads has arrived. Tools: semaphores plus a `Barrier`.
- **Avoiding deadlock**: take shared resources in a fixed order.
- **Exactly once**: create something once even when many threads ask for it at the same moment. Tool: `Lazy<T>`.

## Busy-waiting: the classic mistake

```csharp
while (!ready) { }   // burns 100% of a CPU core doing nothing useful
```

A spinning loop wastes a core, and without `volatile` it may never even see the change. The good approaches **sleep until they are woken up** by a signal.

## Monitor.Wait / PulseAll

`Monitor.Wait(gate)` releases the lock and sleeps. `Monitor.PulseAll(gate)` wakes the sleepers, who then take the lock again and **check their condition again**. Always wait inside a `while` loop, never an `if`:

```csharp
lock (gate)
{
    while (!myTurn)
    {
        Monitor.Wait(gate);
    }
    // ... do the work ...
    Monitor.PulseAll(gate);
}
```
