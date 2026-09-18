# Thread Basics

A thread is an independent path of execution inside your program. With several threads, a program can do several things at once and use every CPU core, but threads that share data need care.

## Concurrency vs parallelism

- **Concurrency**: juggling several tasks, switching between them. One cook with three pans.
- **Parallelism**: really doing several things at the same moment on different CPU cores. Three cooks.

Threads give you both. Your computer has several cores (`Environment.ProcessorCount`), and each can run a thread at the same moment.

## CPU-bound vs I/O-bound work

- **CPU-bound**: the CPU is busy calculating, like sorting or image processing. More threads (up to the number of cores) make it faster. Use `Parallel`, PLINQ or `Task.Run`.
- **I/O-bound**: mostly **waiting** for a disk, network or database. Use `async`/`await`, which waits without holding a thread.

## Creating a thread

```csharp
var worker = new Thread(() => Console.WriteLine("Hello from another thread"));
worker.Start();   // begins running at the same time as this code
worker.Join();    // wait here until it finishes
```

Creating a thread is expensive (about 1 MB of memory plus setup), so .NET keeps a **thread pool** of reusable threads. `Task.Run`, `Parallel.For` and `async` all use it.

## The big danger: shared data

When two threads change the same variable at the same time, updates can be lost. That's a **race condition**. The questions in this topic show it happening, and the next topic, *Synchronization*, shows how to prevent it.

## Note about this site

The results you see come from **real threads on the server**, so numbers like timing and thread IDs change from run to run.
