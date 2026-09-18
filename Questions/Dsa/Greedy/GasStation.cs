namespace CSharpCodingQuestions.Questions.Dsa.Greedy;

[Question(Order = 4, Title = "Gas Station", Level = Medium, Problem = """
    Gas stations stand on a circular road. Station `i` gives `gas[i]` fuel, and driving to the next station costs `cost[i]`.
    Starting with an empty tank, from which station can you drive all the way around? Return its index, or `-1`.
    """)]
public static class GasStation
{
    [Approach(Name = "Try Every Start", Time = "O(n²)", Space = "O(1)", Idea = """
        Simulate the full trip from each station. If the tank never goes negative, that start works.
        """)]
    public static int StartBruteForce(int[] gas, int[] cost)
    {
        int n = gas.Length;
        for (int start = 0; start < n; start++)
        {
            int tank = 0;
            bool completed = true;
            for (int step = 0; step < n; step++)
            {
                int station = (start + step) % n;
                tank += gas[station] - cost[station];
                if (tank < 0)
                {
                    completed = false;
                    break;
                }
            }
            if (completed)
            {
                return start;
            }
        }
        return -1;
    }

    [Approach(Name = "Greedy: Restart After Running Dry", Time = "O(n)", Space = "O(1)", Idea = """
        Two facts make one pass enough:

        1. If total gas ≥ total cost, a valid start **exists**.
        2. If you start at `s` and run dry at station `i`, then no station between `s` and `i` can work either,
           because each of them reaches `i` with even less fuel. So the next candidate start is `i + 1`.
        """)]
    public static int StartGreedy(int[] gas, int[] cost)
    {
        int total = 0;
        int tank = 0;
        int start = 0;
        for (int i = 0; i < gas.Length; i++)
        {
            int gain = gas[i] - cost[i];
            total += gain;
            tank += gain;
            if (tank < 0)
            {
                start = i + 1;
                tank = 0;
            }
        }
        return total >= 0 ? start : -1;
    }

    public static Example[] Examples =>
    [
        new([new[] { 1, 2, 3, 4, 5 }, new[] { 3, 4, 5, 1, 2 }], 3),
        new([new[] { 2, 3, 4 }, new[] { 3, 4, 3 }], -1),
    ];
}
