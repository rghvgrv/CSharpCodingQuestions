namespace CodingQuestions.Dsa.DynamicProgramming;

[Q(1_12_01, "Climbing Stairs (Intro to DP)", Easy,
"You can climb 1 or 2 steps at a time. In how many distinct ways can you reach step n? Also: minimum cost climbing stairs.")]
public static class ClimbingStairs
{
    // DP recipe: 1) define state: ways(i) = ways to reach step i
    //            2) recurrence: ways(i) = ways(i-1) + ways(i-2)   (last move was 1 or 2 steps)
    //            3) base cases: ways(0) = 1, ways(1) = 1
    //            4) order: bottom-up; only 2 previous values needed → O(1) space
    public static int Ways(int n)
    {
        int prev2 = 1, prev1 = 1;
        for (int i = 2; i <= n; i++) (prev2, prev1) = (prev1, prev1 + prev2);
        return prev1;
    }

    // cost[i] = cost of stepping on stair i. Start at 0 or 1. min cost to reach the top (past the end).
    public static int MinCost(int[] cost)
    {
        int a = 0, b = 0; // min cost to stand on i-2 and i-1
        for (int i = 2; i <= cost.Length; i++) (a, b) = (b, Math.Min(b + cost[i - 1], a + cost[i - 2]));
        return b;
    }

    public static void Run()
    {
        Check("Ways(2)", Ways(2), 2);
        Check("Ways(3)", Ways(3), 3);
        Check("Ways(10)", Ways(10), 89);
        Check("MinCost([10,15,20])", MinCost([10, 15, 20]), 15);
        Check("MinCost([1,100,1,1,1,100,1,1,100,1])", MinCost([1, 100, 1, 1, 1, 100, 1, 1, 100, 1]), 6);
    }
}
