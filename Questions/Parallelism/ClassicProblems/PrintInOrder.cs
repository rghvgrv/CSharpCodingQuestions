namespace CodingQuestions.Parallelism.ClassicProblems;

[Q(3_06_01, "Print in Order", Easy,
"Three threads call First(), Second() and Third(). They may start in any order, but the output must always be \"first second third\". (LeetCode 1114)")]
public static class PrintInOrder
{
    // Two one-shot signals: Second waits for "first done", Third waits for "second done".
    public class Foo
    {
        readonly SemaphoreSlim firstDone = new(0), secondDone = new(0);

        public void First(Action print) { print(); firstDone.Release(); }
        public void Second(Action print) { firstDone.Wait(); print(); secondDone.Release(); }
        public void Third(Action print) { secondDone.Wait(); print(); }
    }

    public static void Run()
    {
        for (int trial = 0; trial < 5; trial++)
        {
            var foo = new Foo();
            var output = new ConcurrentQueue<string>();
            var threads = new List<Thread>
            {
                new(() => foo.Third(() => output.Enqueue("third"))),
                new(() => foo.Second(() => output.Enqueue("second"))),
                new(() => foo.First(() => output.Enqueue("first"))),
            };
            foreach (var t in threads.OrderBy(_ => Random.Shared.Next())) t.Start(); // random start order
            threads.ForEach(t => t.Join());
            Check($"Trial {trial + 1}", string.Join(" ", output), "first second third");
        }
    }
}
