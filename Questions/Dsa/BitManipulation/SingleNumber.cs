namespace CodingQuestions.Dsa.BitManipulation;

[Q(1_09_03, "Single Number (XOR Tricks)", Medium,
"(1) Every element appears twice except one; find it. (2) Two elements appear once, all others twice; find both. O(n) time, O(1) space.")]
public static class SingleNumber
{
    // x ^ x = 0 and x ^ 0 = x, so pairs cancel out.
    public static int One(int[] nums) => nums.Aggregate(0, (acc, x) => acc ^ x);

    // XOR of all = a ^ b. Any set bit of that differs between a and b: split the numbers by that bit.
    public static (int, int) Two(int[] nums)
    {
        int xor = One(nums);
        int bit = xor & -xor; // lowest set bit
        int a = 0;
        foreach (int x in nums)
            if ((x & bit) != 0) a ^= x;
        int b = xor ^ a;
        return a < b ? (a, b) : (b, a);
    }

    public static void Run()
    {
        Check("One([4,1,2,1,2])", One([4, 1, 2, 1, 2]), 4);
        Check("Two([1,2,1,3,2,5])", Two([1, 2, 1, 3, 2, 5]), (3, 5));
    }
}
