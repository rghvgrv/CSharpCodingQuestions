namespace CodingQuestions.Dsa.Greedy;

[Q(1_11_02, "Jump Game I & II", Medium,
"nums[i] is the max jump length from index i. (1) Can you reach the last index? (2) What is the minimum number of jumps?")]
public static class JumpGame
{
    // Track the farthest reachable index. If we ever stand beyond it, we're stuck.
    public static bool CanReach(int[] nums)
    {
        int farthest = 0;
        for (int i = 0; i < nums.Length; i++)
        {
            if (i > farthest) return false;
            farthest = Math.Max(farthest, i + nums[i]);
        }
        return true;
    }

    // BFS by levels: [start, end] = indices reachable with `jumps` jumps.
    public static int MinJumps(int[] nums)
    {
        int jumps = 0, end = 0, farthest = 0;
        for (int i = 0; i < nums.Length - 1; i++)
        {
            farthest = Math.Max(farthest, i + nums[i]);
            if (i == end) { jumps++; end = farthest; }
        }
        return jumps;
    }

    public static void Run()
    {
        Check("CanReach([2,3,1,1,4])", CanReach([2, 3, 1, 1, 4]), true);
        Check("CanReach([3,2,1,0,4])", CanReach([3, 2, 1, 0, 4]), false);
        Check("MinJumps([2,3,1,1,4])", MinJumps([2, 3, 1, 1, 4]), 2);
        Check("MinJumps([2,3,0,1,4])", MinJumps([2, 3, 0, 1, 4]), 2);
    }
}
