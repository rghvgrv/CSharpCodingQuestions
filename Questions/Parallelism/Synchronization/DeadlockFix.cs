namespace CSharpCodingQuestions.Questions.Parallelism.Synchronization;

[Question(Order = 3, Title = "Deadlock: Two Threads Waiting Forever", Level = Medium, Problem = """
    Moving money between two accounts locks both of them. Thread 1 moves A → B while thread 2 moves B → A, at the same time.
    Show how this can freeze forever, then fix it.
    """)]
public static class DeadlockFix
{
    public class Account(int id)
    {
        public int Id { get; } = id;
        public int Balance { get; set; } = 100;
    }

    [Approach(Name = "Lock From, Then To (Can Deadlock)", Idea = """
        Thread 1 locks A and waits for B. Thread 2 locks B and waits for A. Each holds what the other needs, so both wait forever.

        This demo uses `Monitor.TryEnter` with a timeout instead of `lock`, so it can **notice** the deadlock and back out
        instead of freezing the page.
        """)]
    public static bool TransferMayDeadlock(Account from, Account to, int amount, Barrier bothHoldFirstLock)
    {
        lock (from)
        {
            bothHoldFirstLock.SignalAndWait();       // make sure both threads hold their first lock
            if (!Monitor.TryEnter(to, 300))
            {
                return false;                         // gave up: this is the deadlock
            }
            try
            {
                from.Balance -= amount;
                to.Balance += amount;
                return true;
            }
            finally
            {
                Monitor.Exit(to);
            }
        }
    }

    [Approach(Name = "Always Lock in the Same Order", Idea = """
        A deadlock needs a **circle** of waiting threads. Break the circle by always locking the account with the **smaller id first**,
        whatever the direction of the transfer. Now both threads compete for the same first lock, and one simply waits for the other to finish.
        """)]
    public static void TransferSafely(Account from, Account to, int amount)
    {
        Account first = from.Id < to.Id ? from : to;
        Account second = from.Id < to.Id ? to : from;
        lock (first)
        {
            lock (second)
            {
                from.Balance -= amount;
                to.Balance += amount;
            }
        }
    }

    public static void Demo()
    {
        var a = new Account(1);
        var b = new Account(2);
        using var barrier = new Barrier(2);
        bool done1 = false;
        bool done2 = false;
        var thread1 = new Thread(() => done1 = TransferMayDeadlock(a, b, 10, barrier));
        var thread2 = new Thread(() => done2 = TransferMayDeadlock(b, a, 10, barrier));
        thread1.Start();
        thread2.Start();
        thread1.Join();
        thread2.Join();
        Print("Opposite lock order: both transfers finished?", done1 && done2, expected: false);
        Console.WriteLine("  → each thread held one lock and waited for the other: a deadlock.");

        a.Balance = 100;
        b.Balance = 100;
        var workers = Enumerable.Range(0, 8).Select(i => new Thread(() =>
        {
            for (int n = 0; n < 1000; n++)
            {
                if (i % 2 == 0)
                {
                    TransferSafely(a, b, 1);
                }
                else
                {
                    TransferSafely(b, a, 1);
                }
            }
        })).ToList();
        workers.ForEach(worker => worker.Start());
        bool allFinished = workers.All(worker => worker.Join(5000));
        Print("Same lock order: 8,000 transfers in both directions finished", allFinished, expected: true);
        Print("Total money is unchanged", a.Balance + b.Balance, expected: 200);
    }
}
