namespace CSharpCodingQuestions.Questions.Parallelism.Synchronization;

[Question(Order = 1, Title = "Protect Shared Data With lock", Level = Easy, Problem = """
    A bank account starts with 1,000. 100 threads each try to withdraw 15 at the same moment.
    The balance must never go below zero, so exactly 66 withdrawals should succeed, leaving 10.
    """)]
public static class BankAccountLock
{
    [Approach(Name = "Check, Then Withdraw (Broken)", Idea = """
        `if (balance >= amount) balance -= amount;` is two steps. Between the **check** and the **withdraw**,
        another thread can withdraw too, so both see enough money and the account can go negative.
        (`Thread.Yield()` makes that gap easier to hit, the way a real, slower program would.)
        """)]
    public class UnsafeAccount(int balance)
    {
        public int Balance => balance;

        public bool Withdraw(int amount)
        {
            if (balance >= amount)
            {
                Thread.Yield();
                balance -= amount;
                return true;
            }
            return false;
        }
    }

    [Approach(Name = "lock Around Check and Withdraw", Idea = """
        Put the check **and** the update inside one `lock`. Only one thread at a time can be inside,
        so nobody can sneak in between the check and the withdrawal.

        - Lock on a private object that only this class uses.
        - Reads that must see a consistent value (like `Balance`) take the lock too.
        """)]
    public class SafeAccount(int balance)
    {
        private readonly object gate = new object();

        public int Balance
        {
            get
            {
                lock (gate)
                {
                    return balance;
                }
            }
        }

        public bool Withdraw(int amount)
        {
            lock (gate)
            {
                if (balance < amount)
                {
                    return false;
                }
                balance -= amount;
                return true;
            }
        }
    }

    public static void Demo()
    {
        var unsafeAccount = new UnsafeAccount(1000);
        int unsafeSuccesses = WithdrawFromManyThreads(unsafeAccount.Withdraw);
        Print("No lock", $"{unsafeSuccesses} withdrawals succeeded, balance {unsafeAccount.Balance}");

        var safeAccount = new SafeAccount(1000);
        Print("With lock: withdrawals that succeeded", WithdrawFromManyThreads(safeAccount.Withdraw), expected: 66);
        Print("With lock: final balance", safeAccount.Balance, expected: 10);
    }

    private static int WithdrawFromManyThreads(Func<int, bool> withdraw)
    {
        int successes = 0;
        using var start = new ManualResetEventSlim();
        var threads = Enumerable.Range(0, 100).Select(_ => new Thread(() =>
        {
            start.Wait();   // release all threads at the same moment
            if (withdraw(15))
            {
                Interlocked.Increment(ref successes);
            }
        })).ToList();

        threads.ForEach(thread => thread.Start());
        start.Set();
        threads.ForEach(thread => thread.Join());
        return successes;
    }
}
