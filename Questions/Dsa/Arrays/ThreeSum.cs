namespace CSharpCodingQuestions.Questions.Dsa.Arrays;

[Question(Order = 17, Title = "Three Numbers That Sum to Zero (3Sum)", Level = Medium, Problem = """
    Find all **unique** triplets `[a, b, c]` in the array with `a + b + c = 0`.
    `[-1, 0, 1, 2, -1, -4]` → `[[-1, -1, 2], [-1, 0, 1]]`.
    """)]
public static class ThreeSum
{
    [Approach(Name = "Try Every Triplet", Time = "O(n³)", Space = "O(n)", Idea = """
        Sort first, so each triplet comes out in the same order (`[-1, 0, 1]`, never `[0, -1, 1]`).
        Then try every `i < j < k`, and use a set of text keys to skip triplets already found.
        """)]
    public static List<List<int>> ThreeSumBruteForce(int[] numbers)
    {
        int[] sorted = (int[])numbers.Clone();
        Array.Sort(sorted);
        var result = new List<List<int>>();
        var seen = new HashSet<string>();

        for (int i = 0; i < sorted.Length; i++)
        {
            for (int j = i + 1; j < sorted.Length; j++)
            {
                for (int k = j + 1; k < sorted.Length; k++)
                {
                    if (sorted[i] + sorted[j] + sorted[k] == 0)
                    {
                        string key = $"{sorted[i]},{sorted[j]},{sorted[k]}";
                        if (seen.Add(key))
                        {
                            result.Add(new List<int> { sorted[i], sorted[j], sorted[k] });
                        }
                    }
                }
            }
        }
        return result;
    }

    [Approach(Name = "Sort + Two Pointers", Time = "O(n²)", Space = "O(1) extra", Idea = """
        1. Sort the array.
        2. Fix the first number `sorted[i]`. Now find two numbers after it that sum to `-sorted[i]`, like Two Sum on a sorted array:
           put `left` just after `i` and `right` at the end. If the sum is too small, move `left` right; if too big, move `right` left.
        3. Skip values equal to the previous one, so no triplet is added twice.
        """)]
    public static List<List<int>> ThreeSumTwoPointers(int[] numbers)
    {
        int[] sorted = (int[])numbers.Clone();
        Array.Sort(sorted);
        var result = new List<List<int>>();

        for (int i = 0; i < sorted.Length - 2; i++)
        {
            if (i > 0 && sorted[i] == sorted[i - 1])
            {
                continue;
            }

            int left = i + 1;
            int right = sorted.Length - 1;
            while (left < right)
            {
                int sum = sorted[i] + sorted[left] + sorted[right];
                if (sum < 0)
                {
                    left++;
                }
                else if (sum > 0)
                {
                    right--;
                }
                else
                {
                    result.Add(new List<int> { sorted[i], sorted[left], sorted[right] });
                    while (left < right && sorted[left] == sorted[left + 1])
                    {
                        left++;
                    }
                    while (left < right && sorted[right] == sorted[right - 1])
                    {
                        right--;
                    }
                    left++;
                    right--;
                }
            }
        }
        return result;
    }

    public static Example[] Examples =>
    [
        new([new[] { -1, 0, 1, 2, -1, -4 }], new[] { new[] { -1, -1, 2 }, new[] { -1, 0, 1 } }, AnyOrder: true),
        new([new[] { 0, 0, 0, 0 }], new[] { new[] { 0, 0, 0 } }),
        new([new[] { 0, 1, 1 }], Array.Empty<int[]>()),
    ];
}
