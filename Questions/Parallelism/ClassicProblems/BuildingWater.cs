namespace CSharpCodingQuestions.Questions.Parallelism.ClassicProblems;

[Question(Order = 4, Title = "Building H₂O", Level = Hard, Problem = """
    Hydrogen threads and oxygen threads arrive in random order. Group them into water molecules:
    every group of 3 must be exactly **2 hydrogen + 1 oxygen**, and a group must be complete before the next one starts. (LeetCode 1117)
    """)]
public static class BuildingWater
{
    [Approach(Name = "Semaphores to Admit, Barrier to Bond", Idea = """
        - Two semaphores act as doors: `hydrogenSlots` lets in 2 hydrogens and `oxygenSlots` lets in 1 oxygen per molecule.
        - A `Barrier(3)` makes the 3 admitted atoms wait for each other. When the third arrives, the barrier's action records the molecule.
        - Only **after** the barrier lets them go does each atom give its permit back, reopening the door for the next group.
          (Reopening earlier, inside the barrier's action, would let a new atom reach the barrier before it has finished the current group.)

        Extra atoms simply wait at the doors, so a molecule can never get 3 hydrogens or 2 oxygens.
        """)]
    public class WaterFactory
    {
        private readonly SemaphoreSlim hydrogenSlots = new(2);
        private readonly SemaphoreSlim oxygenSlots = new(1);
        private readonly Barrier bond;
        private readonly object gate = new object();
        private readonly StringBuilder current = new();

        public List<string> Molecules { get; } = new();

        public WaterFactory()
        {
            bond = new Barrier(3, _ =>
            {
                Molecules.Add(current.ToString());
                current.Clear();
            });
        }

        public void Hydrogen()
        {
            hydrogenSlots.Wait();
            lock (gate)
            {
                current.Append('H');
            }
            bond.SignalAndWait();
            hydrogenSlots.Release();
        }

        public void Oxygen()
        {
            oxygenSlots.Wait();
            lock (gate)
            {
                current.Append('O');
            }
            bond.SignalAndWait();
            oxygenSlots.Release();
        }
    }

    public static void Demo()
    {
        var factory = new WaterFactory();
        string atoms = "OOHHHHHHOHHO";   // 8 H and 4 O → 4 molecules
        var threads = atoms.Select(atom => new Thread(() =>
        {
            if (atom == 'H')
            {
                factory.Hydrogen();
            }
            else
            {
                factory.Oxygen();
            }
        })).ToList();

        foreach (Thread thread in threads.OrderBy(_ => Random.Shared.Next()))
        {
            thread.Start();
        }
        threads.ForEach(thread => thread.Join());

        Print("Molecules (atoms in arrival order)", string.Join(" ", factory.Molecules));
        Print("Number of molecules", factory.Molecules.Count, expected: 4);
        Print("Each has 2 H and 1 O", factory.Molecules.All(m => m.Count(c => c == 'H') == 2 && m.Count(c => c == 'O') == 1), expected: true);
    }
}
