namespace CodingQuestions.Parallelism.ClassicProblems;

[Q(3_06_04, "Building H2O", Hard,
"Hydrogen and oxygen threads arrive in random order. Group them into water molecules: each group of 3 must be exactly 2 H and 1 O, and a group must finish before the next one starts. (LeetCode 1117)")]
public static class BuildingH2O
{
    // Semaphores admit at most 2 H and 1 O into the current molecule. A Barrier(3) waits until all 3 are in,
    // then its post-phase action seals the molecule and lets the next 3 in by releasing the permits.
    public class H2O
    {
        readonly SemaphoreSlim hSlots = new(2), oSlots = new(1);
        readonly Barrier bond;
        readonly Lock printGate = new();
        readonly StringBuilder current = new();
        public readonly List<string> Molecules = [];

        public H2O() => bond = new Barrier(3, _ =>
        {
            Molecules.Add(current.ToString());
            current.Clear();
            hSlots.Release(2);
            oSlots.Release(1);
        });

        public void Hydrogen()
        {
            hSlots.Wait();
            lock (printGate) current.Append('H');
            bond.SignalAndWait();
        }

        public void Oxygen()
        {
            oSlots.Wait();
            lock (printGate) current.Append('O');
            bond.SignalAndWait();
        }
    }

    public static void Run()
    {
        var water = new H2O();
        string atoms = "OOHHHHHHOHHO"; // 8 H, 4 O → 4 molecules
        var threads = atoms.Select(a => new Thread(() => { if (a == 'H') water.Hydrogen(); else water.Oxygen(); })).ToList();
        foreach (var t in threads.OrderBy(_ => Random.Shared.Next())) t.Start();
        threads.ForEach(t => t.Join());

        Console.WriteLine($"  molecules (atom arrival order inside each): {string.Join(" ", water.Molecules)}");
        Check("4 molecules", water.Molecules.Count, 4);
        Check("Each has exactly 2 H and 1 O", water.Molecules.All(m => m.Count(c => c == 'H') == 2 && m.Count(c => c == 'O') == 1), true);
    }
}
