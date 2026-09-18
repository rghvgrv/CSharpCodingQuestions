namespace CodingQuestions.Dsa.Greedy;

[Q(1_11_04, "Gas Station", Medium,
"Stations on a circular route have gas[i] fuel and cost[i] to reach the next one. Return the start index to complete the circuit, or -1.")]
public static class GasStation
{
    // If total gas >= total cost, a solution exists. If the tank goes negative at i,
    // no station between the start and i can work, so start again from i + 1. Time O(n)
    public static int Solve(int[] gas, int[] cost)
    {
        int total = 0, tank = 0, start = 0;
        for (int i = 0; i < gas.Length; i++)
        {
            int diff = gas[i] - cost[i];
            total += diff;
            tank += diff;
            if (tank < 0) { start = i + 1; tank = 0; }
        }
        return total >= 0 ? start : -1;
    }

    public static void Run()
    {
        Check("gas=[1,2,3,4,5], cost=[3,4,5,1,2]", Solve([1, 2, 3, 4, 5], [3, 4, 5, 1, 2]), 3);
        Check("gas=[2,3,4], cost=[3,4,3]", Solve([2, 3, 4], [3, 4, 3]), -1);
    }
}
