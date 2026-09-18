namespace CodingQuestions.Tree.AdvancedTrees;

[Q(2_04_04, "Fenwick Tree (Binary Indexed Tree)", Hard,
"Support Add(index, delta) and PrefixSum(index) in O(log n) with a Fenwick tree. Then count inversions with it.")]
public static class FenwickTree
{
    // tree[i] (1-indexed) covers the range (i - lowbit(i), i], where lowbit(i) = i & -i.
    // Update climbs by adding lowbit; query descends by removing it. Less code than a segment tree.
    public class Bit(int n)
    {
        readonly long[] tree = new long[n + 1];

        public void Add(int i, long delta)
        {
            for (i++; i < tree.Length; i += i & -i) tree[i] += delta;
        }

        public long PrefixSum(int i) // sum of [0..i]
        {
            long sum = 0;
            for (i++; i > 0; i -= i & -i) sum += tree[i];
            return sum;
        }

        public long RangeSum(int l, int r) => PrefixSum(r) - (l > 0 ? PrefixSum(l - 1) : 0);
    }

    // Walk from the right: for each value, count smaller values already seen (they sit to its right).
    public static long Inversions(int[] a)
    {
        var rank = a.Distinct().Order().Select((v, i) => (v, i)).ToDictionary(x => x.v, x => x.i); // compress values
        var bit = new Bit(rank.Count);
        long count = 0;
        for (int i = a.Length - 1; i >= 0; i--)
        {
            int r = rank[a[i]];
            if (r > 0) count += bit.PrefixSum(r - 1);
            bit.Add(r, 1);
        }
        return count;
    }

    public static void Run()
    {
        int[] a = [3, 2, -1, 6, 5, 4, -3, 3, 7, 2, 3];
        var bit = new Bit(a.Length);
        for (int i = 0; i < a.Length; i++) bit.Add(i, a[i]);
        Check("PrefixSum(5)", bit.PrefixSum(5), 19L);
        Check("RangeSum(3..7)", bit.RangeSum(3, 7), 15L);
        bit.Add(3, 6); // a[3] += 6
        Check("after a[3] += 6, PrefixSum(5)", bit.PrefixSum(5), 25L);
        Check("Inversions([8,4,2,1])", Inversions([8, 4, 2, 1]), 6L);
        Check("Inversions([2,4,1,3,5])", Inversions([2, 4, 1, 3, 5]), 3L);
    }
}
