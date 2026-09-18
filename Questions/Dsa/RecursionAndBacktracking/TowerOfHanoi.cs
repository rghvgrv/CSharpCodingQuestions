namespace CSharpCodingQuestions.Questions.Dsa.RecursionAndBacktracking;

[Question(Order = 1, Title = "Tower of Hanoi", Level = Easy, Problem = """
    Move `n` disks from peg A to peg C, using peg B as a helper. Rules: move one disk at a time, and never put a bigger disk on a smaller one.
    Return the list of moves.
    """)]
public static class TowerOfHanoi
{
    [Approach(Name = "Recursion", Time = "O(2ⁿ)", Space = "O(n)", Idea = """
        To move `n` disks from A to C:

        1. Move the top `n - 1` disks from A to B (using C as the helper).
        2. Move the biggest disk from A to C.
        3. Move the `n - 1` disks from B to C (using A as the helper).

        Steps 1 and 3 are the same problem with one disk fewer, so recursion handles them.
        It always takes `2ⁿ - 1` moves, and no method can do it in fewer.
        """)]
    public static List<string> SolveHanoi(int n)
    {
        var moves = new List<string>();
        MoveDisks(n, 'A', 'C', 'B', moves);
        return moves;
    }

    private static void MoveDisks(int count, char from, char to, char helper, List<string> moves)
    {
        if (count == 0)
        {
            return;
        }
        MoveDisks(count - 1, from, helper, to, moves);
        moves.Add($"disk {count}: {from} → {to}");
        MoveDisks(count - 1, helper, to, from, moves);
    }

    public static Example[] Examples =>
    [
        new([2], new[] { "disk 1: A → B", "disk 2: A → C", "disk 1: B → C" }),
        new([3], new[] { "disk 1: A → C", "disk 2: A → B", "disk 1: C → B", "disk 3: A → C", "disk 1: B → A", "disk 2: B → C", "disk 1: A → C" }),
    ];
}
