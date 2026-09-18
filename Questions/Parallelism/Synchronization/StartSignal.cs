namespace CSharpCodingQuestions.Questions.Parallelism.Synchronization;

[Question(Order = 6, Title = "Wait for a Signal (ManualResetEventSlim)", Level = Medium, Problem = """
    4 worker threads must wait until the main thread says "go", then all start together.
    How should they wait?
    """)]
public static class StartSignal
{
    [Approach(Name = "Check a Flag Over and Over", Idea = """
        Each worker loops until a shared `ready` flag becomes true, sleeping a little between checks.
        It wastes CPU while checking, and adds up to one sleep interval of delay after the signal.
        `volatile` makes sure each thread sees the latest value of the flag.
        """)]
    public class PollingGate
    {
        private volatile bool ready;

        public void Wait()
        {
            while (!ready)
            {
                Thread.Sleep(10);
            }
        }

        public void Open() => ready = true;
    }

    [Approach(Name = "ManualResetEventSlim", Idea = """
        An **event** is a gate the operating system can wake threads from. `Wait()` puts the thread to sleep (no CPU used)
        until someone calls `Set()`, which wakes **all** waiting threads at once.

        Related tools:

        - `AutoResetEvent`: lets only **one** waiter through per `Set()`, like a turnstile.
        - `CountdownEvent`: opens after `N` signals (see the next question).
        """)]
    public class EventGate
    {
        private readonly ManualResetEventSlim gate = new(false);

        public void Wait() => gate.Wait();

        public void Open() => gate.Set();
    }

    public static void Demo()
    {
        var gate = new EventGate();
        int started = 0;
        var workers = Enumerable.Range(0, 4).Select(_ => new Thread(() =>
        {
            gate.Wait();
            Interlocked.Increment(ref started);
        })).ToList();
        workers.ForEach(worker => worker.Start());

        Thread.Sleep(100);
        Print("Workers started before the signal", Volatile.Read(ref started), expected: 0);
        gate.Open();
        workers.ForEach(worker => worker.Join());
        Print("Workers started after one Set()", started, expected: 4);

        var polling = new PollingGate();
        var poller = new Thread(polling.Wait);
        poller.Start();
        polling.Open();
        Print("Polling gate also works", poller.Join(1000), expected: true);
    }
}
