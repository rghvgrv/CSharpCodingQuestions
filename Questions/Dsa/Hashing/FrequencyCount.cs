namespace CodingQuestions.Dsa.Hashing;

[Q(1_04_01, "Frequency Count", Easy,
"Count how many times each element appears, and find the most frequent element. Use a Dictionary, then LINQ.")]
public static class FrequencyCount
{
    // Dictionary = hash table: O(1) average insert and lookup.
    public static Dictionary<int, int> Count(int[] nums)
    {
        var freq = new Dictionary<int, int>();
        foreach (int x in nums) freq[x] = freq.GetValueOrDefault(x) + 1;
        return freq;
    }

    public static void Run()
    {
        int[] nums = [1, 3, 2, 3, 4, 3, 1];
        var freq = Count(nums);
        Check("Count(1)", freq[1], 2);
        Check("Count(3)", freq[3], 3);
        Check("Most frequent", freq.MaxBy(kv => kv.Value).Key, 3);

        // Same thing with LINQ CountBy (.NET 9+)
        Check("CountBy LINQ", nums.CountBy(x => x).OrderBy(kv => kv.Key).Select(kv => $"{kv.Key}:{kv.Value}"), ["1:2", "2:1", "3:3", "4:1"]);
        Check("Char frequency \"hello\"", "hello".CountBy(c => c).Select(kv => $"{kv.Key}{kv.Value}"), ["h1", "e1", "l2", "o1"]);
    }
}
