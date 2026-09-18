namespace CodingQuestions.Dsa.RecursionAndBacktracking;

[Q(1_08_03, "Permutations", Medium,
"Return all orderings of an array of distinct numbers. Then list unique permutations of a string with repeated letters.")]
public static class Permutations
{
    // Swap-based: fix position `k` by swapping each remaining element into it, recurse, swap back. O(n · n!)
    public static List<List<int>> Solve(int[] nums)
    {
        var result = new List<List<int>>();
        void Permute(int k)
        {
            if (k == nums.Length) { result.Add([.. nums]); return; }
            for (int i = k; i < nums.Length; i++)
            {
                (nums[k], nums[i]) = (nums[i], nums[k]);
                Permute(k + 1);
                (nums[k], nums[i]) = (nums[i], nums[k]);
            }
        }
        Permute(0);
        return result;
    }

    // Unique permutations: sort, use a `used` array, and skip a duplicate unless its twin before it is in use.
    public static List<string> Unique(string s)
    {
        var chars = s.Order().ToArray();
        var used = new bool[chars.Length];
        var result = new List<string>();
        var current = new StringBuilder();
        void Build()
        {
            if (current.Length == chars.Length) { result.Add(current.ToString()); return; }
            for (int i = 0; i < chars.Length; i++)
            {
                if (used[i] || (i > 0 && chars[i] == chars[i - 1] && !used[i - 1])) continue;
                used[i] = true; current.Append(chars[i]);
                Build();
                used[i] = false; current.Length--;
            }
        }
        Build();
        return result;
    }

    public static void Run()
    {
        var perms = Solve([1, 2, 3]);
        Check("[1,2,3] count", perms.Count, 6);
        Show("[1,2,3]", perms);
        Check("Unique(\"aab\")", Unique("aab"), ["aab", "aba", "baa"]);
        Check("Unique(\"abcd\").Count = 4!", Unique("abcd").Count, 24);
    }
}
