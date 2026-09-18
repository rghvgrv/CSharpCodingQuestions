# Synchronization

Synchronization tools stop threads from stepping on each other: locks for exclusive access, atomic operations for simple counters, semaphores for limits, and signals for waiting on each other.

## The toolbox

| Tool | What it does | Use it when |
|---|---|---|
| `lock` | One thread at a time inside a block | Protecting shared data (the default choice) |
| `Interlocked` | One atomic operation on a number | Counters, flags, simple updates |
| `SemaphoreSlim` | At most N threads at a time | Limiting concurrent work; locking with `await` |
| `ReaderWriterLockSlim` | Many readers OR one writer | Data that is read much more than written |
| `ManualResetEventSlim` | A gate that opens for everyone | "Start now!" signals |
| `CountdownEvent` | Wait until N signals arrive | Waiting for N workers to finish |
| `Barrier` | Everyone waits for everyone | Work in phases |

## The lock

```csharp
private readonly object gate = new object();
private int balance;

public void Deposit(int amount)
{
    lock (gate)          // only one thread at a time can be in here
    {
        balance += amount;
    }
}
```

Rules of thumb:

- Keep the code inside a lock **short**.
- Lock on a **private** object that only this class uses.
- Every access to the shared data must use the **same** lock, reads included.

## Deadlock

Thread A holds lock 1 and waits for lock 2, while thread B holds lock 2 and waits for lock 1. Both wait forever. The simplest fix is to **always take locks in the same order**.

## Rule of thumb

Share as little as possible. Data that only one thread touches needs no lock at all.
