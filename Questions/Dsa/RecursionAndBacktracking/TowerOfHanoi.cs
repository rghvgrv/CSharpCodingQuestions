namespace CodingQuestions.Dsa.RecursionAndBacktracking;

[Q(1_08_01, "Tower of Hanoi", Easy,
"Move n disks from peg A to peg C using peg B. You may move one disk at a time and never put a bigger disk on a smaller one. Print the moves.")]
public static class TowerOfHanoi
{
    // Move n-1 disks out of the way (A → B), move the biggest (A → C), move the n-1 back on top (B → C).
    // Moves = 2ⁿ - 1
    public static int Solve(int n, char from, char to, char via, List<string> moves)
    {
        if (n == 0) return 0;
        int count = Solve(n - 1, from, via, to, moves);
        moves.Add($"disk {n}: {from} → {to}");
        return count + 1 + Solve(n - 1, via, to, from, moves);
    }

    public static void Run()
    {
        var moves = new List<string>();
        Check("Solve(3) moves", Solve(3, 'A', 'C', 'B', moves), 7);
        moves.ForEach(m => Console.WriteLine("  " + m));
        Check("Solve(10) moves = 2^10 - 1", Solve(10, 'A', 'C', 'B', []), 1023);
    }
}
