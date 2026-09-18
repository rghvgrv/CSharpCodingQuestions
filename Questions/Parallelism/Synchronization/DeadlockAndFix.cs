namespace CodingQuestions.Parallelism.Synchronization;

[Q(3_02_03, "Deadlock: Reproduce and Fix", Medium,
"Two threads transfer money between the same two accounts in opposite directions. Show how this deadlocks, then fix it with consistent lock ordering.")]
public static class DeadlockAndFix
{
    // Deadlock needs 4 conditions: mutual exclusion, hold-and-wait, no preemption, circular wait.
    // Break circular wait: always take locks in the same global order (e.g. by account id).
    class Account(int id) { public readonly int Id = id; public int Balance = 100; }

    public static void Run()
    {
        var a = new Account(1);
        var b = new Account(2);

        // Broken: T1 locks a then b, T2 locks b then a. The barrier makes each grab its first lock before the second.
        using var bothHoldFirstLock = new Barrier(2);
        bool deadlocked = false;
        void Broken(Account from, Account to)
        {
            lock (from)
            {
                bothHoldFirstLock.SignalAndWait();
                // A real `lock (to)` would hang forever here. TryEnter with a timeout lets us detect it.
                if (!Monitor.TryEnter(to, 300)) { deadlocked = true; return; }
                try { from.Balance -= 10; to.Balance += 10; }
                finally { Monitor.Exit(to); }
            }
        }
        var t1 = new Thread(() => Broken(a, b));
        var t2 = new Thread(() => Broken(b, a));
        t1.Start(); t2.Start(); t1.Join(); t2.Join();
        Check("Opposite lock order → deadlock detected", deadlocked, true);

        // Fixed: lock the lower id first, whatever the transfer direction.
        void Transfer(Account from, Account to, int amount)
        {
            var (first, second) = from.Id < to.Id ? (from, to) : (to, from);
            lock (first)
                lock (second)
                {
                    from.Balance -= amount;
                    to.Balance += amount;
                }
        }
        a.Balance = b.Balance = 100;
        var workers = Enumerable.Range(0, 8).Select(i => new Thread(() =>
        {
            for (int k = 0; k < 1000; k++)
                if (i % 2 == 0) Transfer(a, b, 1); else Transfer(b, a, 1);
        })).ToList();
        workers.ForEach(t => t.Start());
        bool finished = workers.All(t => t.Join(5000));
        Check("Ordered locking: 8000 opposite transfers finish", finished, true);
        Check("Money is conserved", a.Balance + b.Balance, 200);
        Check("Balances (equal transfers each way)", (a.Balance, b.Balance), (100, 100));
    }
}
