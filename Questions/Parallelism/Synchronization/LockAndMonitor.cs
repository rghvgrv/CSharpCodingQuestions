namespace CodingQuestions.Parallelism.Synchronization;

[Q(3_02_01, "lock, Monitor & the .NET 9 Lock Type", Easy,
"A bank account starts at 1000. 100 threads each try to withdraw 15 at the same time. Make sure it never goes negative and exactly 66 withdrawals succeed.")]
public static class LockAndMonitor
{
    public class Account(decimal balance)
    {
        // .NET 9+: System.Threading.Lock is a dedicated lock type. `lock (new object())` works the same way.
        readonly Lock gate = new();

        // The check and the update must happen as ONE step. Without the lock, two threads can both
        // see balance = 15, both pass the check, and both withdraw.
        public bool Withdraw(decimal amount)
        {
            lock (gate)
            {
                if (balance < amount) return false;
                balance -= amount;
                return true;
            }
        }

        public decimal Balance { get { lock (gate) return balance; } }
    }

    public static void Run()
    {
        var account = new Account(1000);
        int successes = 0;
        var threads = Enumerable.Range(0, 100).Select(_ => new Thread(() =>
        {
            if (account.Withdraw(15)) Interlocked.Increment(ref successes);
        })).ToList();
        threads.ForEach(t => t.Start());
        threads.ForEach(t => t.Join());

        Check("Successful withdrawals", successes, 66);
        Check("Final balance", account.Balance, 10m);

        // `lock (obj) { ... }` on a plain object compiles to Monitor.Enter / Monitor.Exit in try/finally:
        object gate = new();
        bool taken = false;
        try { Monitor.Enter(gate, ref taken); Console.WriteLine("  inside Monitor.Enter block"); }
        finally { if (taken) Monitor.Exit(gate); }

        // Monitor.TryEnter lets you give up instead of waiting forever.
        var holder = new Thread(() => { lock (gate) Thread.Sleep(300); });
        holder.Start();
        Thread.Sleep(50);
        Check("TryEnter(100 ms) while another thread holds the lock", Monitor.TryEnter(gate, 100), false);
        holder.Join();
    }
}
